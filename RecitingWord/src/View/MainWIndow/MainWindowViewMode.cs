using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RecitingWord
{
    public class MainWindowViewMode:MVVM.ViewModeBase
    {
        public static MainWindowViewMode Instance { get; } = new MainWindowViewMode();
        MainWindowViewMode()
        {
            DeleteKeyDown = new MVVM.Command(() => WordPlayViewMode.Instance.Word.IsOk = true);
            Right = new MVVM.Command(() => SettingViewMode.Instance.NextWord());
            Left = new MVVM.Command(() => SettingViewMode.Instance.BackWord());
            Loaded = new MVVM.Command(LoadedHandle);
            End = new MVVM.Command(EndHandle);
            Numer0 = new MVVM.Command(Numer0Handle);
            Up = new MVVM.Command(() =>
            {
                try
                {
                    SettingViewMode.Instance.synth.Volume += 5;
                    ProgramConfig.Default.Volume = SettingViewMode.Instance.synth.Volume;
                    ProgramConfig.Default.Save();
                }
                catch (Exception)
                {
                }
            });
            Down = new MVVM.Command(() =>
            {
                try
                {
                    SettingViewMode.Instance.synth.Volume -= 5;
                    ProgramConfig.Default.Volume = SettingViewMode.Instance.synth.Volume;
                    ProgramConfig.Default.Save();
                }
                catch (Exception)
                {
                }
            });

            //WheelClick = new MVVM.Command((sender) => { SettingViewMode.Instance.synth.Volume--; });

            Esc = new MVVM.Command(() => 
            { /*System.Windows.Application.Current.Shutdown();*/
                Window.WindowState = System.Windows.WindowState.Minimized;
                SettingViewMode.Instance.SuspendPlay();
            });
            Enter = new MVVM.Command(() =>
            {
                if (Window.WindowState!= System.Windows.WindowState.Maximized)
                {
                    Window.WindowState = System.Windows.WindowState.Maximized;
                }
                else
                {
                    Window.WindowState = System.Windows.WindowState.Normal;
                }
            });
            
            Space = new MVVM.Command(() => 
            {
                if (SettingViewMode.Instance.IsPlay)
                    SettingViewMode.Instance.SuspendPlay();
                else
                    SettingViewMode.Instance.Play();
            });
        }

        private void Numer0Handle()
        {
            SettingViewMode.Instance.ShowExplain = !SettingViewMode.Instance.ShowExplain;

            if (SettingViewMode.Instance.ShowExplain)
            {
                WordPlayViewMode.Instance.WordExplainingOpacity = 1;
            }
            else
            {
                WordPlayViewMode.Instance.WordExplainingOpacity = 0;
            }
        }

        private void EndHandle()
        {

        }

        private void LoadedHandle(object sender)
        {
            Window = sender as MainWindow;
            Window.KeyDown += Window_KeyDown;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
          
        }

        private MainWindow _Window;
        public MainWindow Window
        {
            get { return _Window; }
            set { SetProperty(ref _Window, value, nameof(Window)); }
        }

        private ICommand _DeleteKeyDown;
        public ICommand DeleteKeyDown
        {
            get { return _DeleteKeyDown; }
            set { SetProperty(ref _DeleteKeyDown, value, nameof(DeleteKeyDown)); }
        }
        private ICommand _Left;
        public ICommand Left
        {
            get { return _Left; }
            set { SetProperty(ref _Left, value, nameof(Left)); }
        }
        private ICommand _Right;
        public ICommand Right
        {
            get { return _Right; }
            set { SetProperty(ref _Right, value, nameof(Right)); }
        }
        private ICommand _Up;
        public ICommand Up
        {
            get { return _Up; }
            set { SetProperty(ref _Up, value, nameof(Up)); }
        }
        private ICommand _Down;
        public ICommand Down
        {
            get { return _Down; }
            set { SetProperty(ref _Down, value, nameof(Down)); }
        }

        private ICommand _Space;
        public ICommand Space
        {
            get { return _Space; }
            set { SetProperty(ref _Space, value, nameof(Space)); }
        }
        private ICommand _Esc;
        public ICommand Esc
        {
            get { return _Esc; }
            set { SetProperty(ref _Esc, value, nameof(Esc)); }
        }

        private ICommand _Enter;
        public ICommand Enter
        {
            get { return _Enter; }
            set { SetProperty(ref _Enter, value, nameof(Enter)); }
        }
        private ICommand _End;
        public ICommand End
        {
            get { return _End; }
            set { SetProperty(ref _End, value, nameof(End)); }
        }

        private ICommand _Numer0;
        public ICommand Numer0
        {
            get { return _Numer0; }
            set { SetProperty(ref _Numer0, value, nameof(Numer0)); }
        }


        private ICommand _WheelClick;
        public ICommand WheelClick
        {
            get { return _WheelClick; }
            set { SetProperty(ref _WheelClick, value, nameof(WheelClick)); }
        }

        private ICommand _Loaded;
        public ICommand Loaded
        {
            get { return _Loaded; }
            set { SetProperty(ref _Loaded, value, nameof(Loaded)); }
        }
    }


}
