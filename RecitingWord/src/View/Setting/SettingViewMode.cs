using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace RecitingWord
{
    class SettingViewMode : MVVM.ViewModeBase
    {
        static SettingViewMode _Instance = new SettingViewMode();
        public static SettingViewMode Instance
        {
            get
            {
                return _Instance;
            }

        }
        private SettingViewMode()
        {
            StartPlay = new MVVM.Command(StartPlayClick, ()=> Status == PlayStatus.Stop && TypeWordViewMode.Instance.TypeWord.Count > 0);
            StopPlay = new MVVM.Command(StopPlayClick, () => Status != PlayStatus.Stop);
            OpenFile = new MVVM.Command(OpenFileClick);
            Paste = new MVVM.Command(PasteClick);
            ShowTime = 3;
            FadeIn = 1;
            FadeOut = 1;
            ran = new System.Random();
            ManualResetEvent = new ManualResetEvent(true);
            ManualResetEvent.Set();
        }

        private void PasteClick()
        {
            TypeWordViewMode.Instance.TypeWords = System.Windows.Forms.Clipboard.GetText();
        }

        private void OpenFileClick()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Task.Run(() =>
                {
                    TypeWordViewMode.Instance.TypeWords = ReadAllFile(ofd.FileNames);
                });
            }
        }
    
        private void StopPlayClick()
        {
            try
            {
                if (PlayThread != null)
                    PlayThread.Abort();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            PlayThread = null;
            Status = PlayStatus.Stop;
        }

        private void StartPlayClick()
        {
            if (PlayThread == null)
            {
                PlayThread = new Thread(new ThreadStart(() => {
                    var Word = new WordMode("");
                    var WordIndex = 0;
                    while (true)
                    {
                        if (Random)
                        {
                            WordIndex = ran.Next(0, TypeWordViewMode.Instance.TypeWord.Count - 1);
                        }
                        else
                        {
                            WordIndex += 1;
                            if (WordIndex >= TypeWordViewMode.Instance.TypeWord.Count)
                            {
                                WordIndex = 0;
                            }
                        }
                        Word = TypeWordViewMode.Instance.TypeWord[WordIndex];
                        Word.AsynTrans();
                        Thread.Sleep((int)(ShowTime * 1000));
                        ManualResetEvent.WaitOne();

                        while (WordPlayViewMode.Instance.WordOpacity > 0)
                        {
                            WordPlayViewMode.Instance.WordExplainingOpacity = WordPlayViewMode.Instance.WordOpacity -= (1f / 30f);
                            Thread.Sleep((int)(FadeOut / 30 * 1000));
                        }

                        WordPlayViewMode.Instance.Word = Word;

                        while (WordPlayViewMode.Instance.WordOpacity < 1)
                        {
                            WordPlayViewMode.Instance.WordExplainingOpacity = WordPlayViewMode.Instance.WordOpacity += 1f / 30f;
                            Thread.Sleep((int)(FadeOut / 30 * 1000));
                        }
                    }
                }));
                PlayThread.IsBackground = true;
                PlayThread.Start();
            }

            ManualResetEvent.Set();
            Status = PlayStatus.Play;
        }
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="FileNames"></param>
        /// <returns></returns>
        string ReadAllFile(string[] FileNames)
        {
            StringBuilder Text = new StringBuilder();
            foreach (var item in FileNames)
            {
                try
                {
                    Text.AppendLine(File.ReadAllText(item));
                }
                catch (Exception)
                {
                }
            }
            return Text.ToString();
        }
        private ICommand _StartPlay;
        public ICommand StartPlay
        {
            get { return _StartPlay; }
            set
            {
                _StartPlay = value;
                ProperChange(nameof(StartPlay));
            }
        }

        private ICommand _StopPlay;
        public ICommand StopPlay
        {
            get { return _StopPlay; }
            set
            {
                if (_StopPlay != value)
                {
                    _StopPlay = value;
                    ProperChange(nameof(StopPlay));
                }
            }
        }

        private ICommand _OpenFile;
        public ICommand OpenFile
        {
            get { return _OpenFile; }
            set
            {
                if (_OpenFile != value)
                {
                    _OpenFile = value;
                    ProperChange(nameof(OpenFile));
                }
            }
        }
        private ICommand _Paste;
        public ICommand Paste
        {
            get { return _Paste; }
            set
            {
                if (_Paste != value)
                {
                    _Paste = value;
                    ProperChange(nameof(Paste));
                }
            }
        }

        private double _ShowTime;
        public double ShowTime
        {
            get { return _ShowTime; }
            set
            {
                if (_ShowTime != value)
                {
                    _ShowTime = value;
                    ProperChange(nameof(ShowTime));
                }
            }
        }

        private double _FadeIn;
        public double FadeIn
        {
            get { return _FadeIn; }
            set
            {
                if (_FadeIn != value)
                {
                    _FadeIn = value;
                    ProperChange(nameof(FadeIn));
                }
            }
        }
        private double _FadeOut;
        public double FadeOut
        {
            get { return _FadeOut; }
            set
            {
                if (_FadeOut != value)
                {
                    _FadeOut = value;
                    ProperChange(nameof(FadeOut));
                }
            }
        }

        private PlayStatus _Status = PlayStatus.Stop;
        public PlayStatus Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    ProperChange(nameof(Status));
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }
        private bool _Random = true;
        public bool Random
        {
            get { return _Random; }
            set
            {
                if (_Random != value)
                {
                    _Random = value;
                    ProperChange(nameof(Random));
                }
            }
        }
        public Thread PlayThread { get; set; }
        public Random ran { get; set; }
        public ManualResetEvent ManualResetEvent { get; set; }
    }
    enum PlayStatus
    {
        Stop,
        Play,
    }
}
