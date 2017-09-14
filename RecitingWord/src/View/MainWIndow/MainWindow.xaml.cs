using RecitingWord.View;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecitingWord
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //public MainWindow Instance { get; } = new MainWindow();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = MainWindowViewMode.Instance;
            this.Loaded += MainWindow_Loaded;
            this.Closed += MainWindow_Closed;

        }

        IntPtr Handle;
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Handle = new WindowInteropHelper(this).Handle;
            Handle = SetClipboardViewer(Handle);
            //SetHook();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            UnHook();
            ChangeClipboardChain(Handle, Handle);
            System.Windows.Application.Current.Shutdown();
        }






        #region 钩子
        
        int hHook;
        public Win32Api.HookProc hProc;
        public const int WH_MOUSE_LL = 14;
        int HookHandle(int nCode, Int32 wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return Win32Api.CallNextHookEx(hHook, nCode, wParam, lParam);
            }
            else
            {
                MouseQueue.Enqueue(new MouseEvent()
                {
                    MouseButton = Win32Api.GetButton(wParam),
                    EventType = Win32Api.GetEventType(wParam),
                    Info = (Win32Api.MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(Win32Api.MouseHookStruct)),
                });
                ManualResetEvent.Set();
                //this.Point = new Point(MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y);
                ////Console.WriteLine($"{button},{eventType},{Point}");
                return Win32Api.CallNextHookEx(hHook, nCode, wParam, lParam);
            }
        }

        int SetHook()
        {
            Task.Run((Action)MouseEventHandle);
            hProc = new Win32Api.HookProc(HookHandle);
            return hHook = Win32Api.SetWindowsHookEx(WH_MOUSE_LL, hProc, IntPtr.Zero, 0);
        }
        public void UnHook()
        {
            Win32Api.UnhookWindowsHookEx(hHook);
        }

        ConcurrentQueue<MouseEvent> MouseQueue = new ConcurrentQueue<MouseEvent>();
        ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        void MouseEventHandle()
        {
            MouseEvent EventnData;
            while (true)
            {
                while (MouseQueue.TryDequeue(out EventnData))
                {

                    if (EventnData.MouseButton == MouseButtons.Left &&
                        EventnData.EventType   == Win32Api.MouseEventType.MouseUp)
                    {
                        if(SettingViewMode.Instance.AutoGetWord)
                        SendKeys.SendWait("^c");
                    }
                }
                ManualResetEvent.Reset();
                ManualResetEvent.WaitOne();
            }
        }

        #endregion
        #region 监视剪切板
        //重载这个方法，使用Interop获取HwndSource
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source?.AddHook(WndProc);
        }
        //Constants for API Calls...  
        private const int WM_DRAWCLIPBOARD = 0x308;
        private const int WM_CHANGECBCHAIN = 0x30D;
        Match firstMatchWord;
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_DRAWCLIPBOARD)
            {
                try
                {
                    if (!SettingViewMode.Instance.MonitorClipboard)
                    {
                        return IntPtr.Zero;
                    }
                    var Word = System.Windows.Clipboard.GetText();
                    if (!string.IsNullOrWhiteSpace(Word))
                    {
                        firstMatchWord = TypeWordViewMode.Instance.MatchWord.Match(Word);
                        if (!string.IsNullOrWhiteSpace(firstMatchWord.Value))
                        SettingViewMode.Instance.Read(firstMatchWord.Value);
                        TypeWordViewMode.Instance.SetWords(Word);
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex);
                }
            }
            else if (msg == WM_CHANGECBCHAIN)
            {

            }

            return IntPtr.Zero;
        }


        //API declarations...  
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static public extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static public extern bool ChangeClipboardChain(IntPtr HWnd, IntPtr HWndNext);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        #endregion
    }
}
