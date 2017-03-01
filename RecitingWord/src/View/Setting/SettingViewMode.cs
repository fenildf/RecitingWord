using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RecitingWord
{
    class SettingViewMode : NotificationObject
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
            StartPlay = new DelegateCommand(StartPlayClick, () => Status == PlayStatus.Stop && TypeWordViewMode.Instance.TypeWord.Count > 0);
            StopPlay = new DelegateCommand(StopPlayClick, () => Status != PlayStatus.Stop);
            ShowTime = 3;
            FadeIn = 1;
            FadeOut = 1;
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
            ran = new System.Random();
        }

        private void StartPlayClick()
        {
            if (PlayThread != null)
                PlayThread = new Thread(new ThreadStart(() => {
                    var Word = new WordMode("");
                    var WordIndex = 0;
                    while (true)
                    {

                        while (WordPlayViewMode.Instance.WordOpacity <= 0)
                        {
                            WordPlayViewMode.Instance.WordExplainingOpacity = WordPlayViewMode.Instance.WordOpacity -= 1 / 30;
                            Thread.Sleep((int)(FadeOut / 30 * 1000));
                        }

                        if (Random)
                        {
                            WordIndex = ran.Next(0,TypeWordViewMode.Instance.TypeWord.Count - 1);
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
                        WordPlayViewMode.Instance.Word = Word.Word;
                        WordPlayViewMode.Instance.WordExplaining = Word.WordExplaining;

                        while (WordPlayViewMode.Instance.WordOpacity >= 1)
                        {
                            WordPlayViewMode.Instance.WordExplainingOpacity = WordPlayViewMode.Instance.WordOpacity += 1 / 30;
                            Thread.Sleep((int)(FadeOut / 30 * 1000));
                        }

                        Thread.Sleep((int)(ShowTime * 1000));
                    }
                }));

            Status = PlayStatus.Play;
        }

        private ICommand _StartPlay;
        public ICommand StartPlay
        {
            get { return _StartPlay; }
            set
            {
                _StartPlay = value;
                RaisePropertyChanged(nameof(StartPlay));
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
                    RaisePropertyChanged(nameof(StopPlay));
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
                    RaisePropertyChanged(nameof(OpenFile));
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
                    RaisePropertyChanged(nameof(Paste));
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
                    RaisePropertyChanged(nameof(ShowTime));
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
                    RaisePropertyChanged(nameof(FadeIn));
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
                    RaisePropertyChanged(nameof(FadeOut));
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
                    RaisePropertyChanged(nameof(Status));
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
                    RaisePropertyChanged(nameof(Random));
                }
            }
        }
        public Thread PlayThread { get; set; }
        public Random ran { get; set; }
    }
    enum PlayStatus
    {
        Stop,
        Play,
    }
}
