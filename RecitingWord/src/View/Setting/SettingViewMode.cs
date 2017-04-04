using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
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

            ShowTime    = ProgramConfig.Default.ShowTime;
            FadeIn      = ProgramConfig.Default.FadeIn;
            FadeOut     = ProgramConfig.Default.FadeOut;
            ShowExplain = ProgramConfig.Default.ShowExplain;
            Random      = ProgramConfig.Default.Random;

            ran         = new System.Random();
            DelayManualResetEvent = new ManualResetEvent(true);
            SuspendManualResetEvent = new ManualResetEvent(true);
            SuspendManualResetEvent.Set();
            DelayManualResetEvent.Set();
            synth = new SpeechSynthesizer();
            synth.Volume = ProgramConfig.Default.Volume;
        }
        public SpeechSynthesizer synth { get; }
        
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
                PlayThread = new Thread(new ThreadStart(() => 
                {
                    var NextWord = false;
                    var Word = new WordMode("");
                    List<WordMode> Words = new List<WordMode>();
                    WordsRecords.Clear();
                    
                    while (true)
                    {
                        var work = Task.Run(new Func<bool>(() =>
                        {
                            Words = TypeWordViewMode.Instance.TypeWord.FindAll((index) => !index.IsOk);
                            if (Words == null) return false;
                            if (Words.Count <= 0) return false;

                    again:  if(Random)
                            {
                                WordIndex = ran.Next(0, Words.Count);
                            }
                            else
                            {
                                WordIndex += 1;
                                if (WordIndex >= Words.Count)
                                {
                                    WordIndex = 0;
                                }
                            }
                            if (WordIndex < Words.Count)
                            {
                                Word = Words[WordIndex];
                                Word.AsynTrans();
                            }
                            else
                            {
                                goto again;
                            }
                            return true;
                        }));
                        DelayManualResetEvent.Reset();
                        //Thread.Sleep((int)(ShowTime * 1000));
                        NextWord = DelayManualResetEvent.WaitOne((int)(ShowTime * 1000));
                        SuspendManualResetEvent.WaitOne();
                        if (!NextWord)
                        {
                            Status = PlayStatus.Play;

                            if (!work.Result)
                            {
                                Status = PlayStatus.Stop;
                                return;
                            }

                            if(!NextWord)
                            while (WordPlayViewMode.Instance.WordOpacity > 0)
                            {
                                WordPlayViewMode.Instance.WordExplainingOpacity = WordPlayViewMode.Instance.WordOpacity -= (1f / 30f);
                                //Thread.Sleep((int)(FadeOut / 30 * 1000));
                                if (NextWord = DelayManualResetEvent.WaitOne((int)(FadeOut / 30 * 1000))) break;
                            }


                            WordPlayViewMode.Instance.Word = Word;
                            WordPlayViewMode.Instance.Word.ShowCount++;
                            if (!NextWord)
                            while (WordPlayViewMode.Instance.WordOpacity < 1)
                            {
                                WordPlayViewMode.Instance.WordExplainingOpacity = WordPlayViewMode.Instance.WordOpacity += 1f / 30f;
                                //Thread.Sleep((int)(FadeOut / 30 * 1000));
                                if (NextWord = DelayManualResetEvent.WaitOne((int)(FadeOut / 30 * 1000))) break;
                            }
                        }
                        else
                        {
                            WordPlayViewMode.Instance.Word = Word;
                            WordPlayViewMode.Instance.Word.ShowCount++;
                        }

                        if (NextWord)
                        {
                            WordPlayViewMode.Instance.WordOpacity = 1;
                            WordPlayViewMode.Instance.WordExplainingOpacity = 1;
                        }

                        WordsRecords.Add(Word);
                        BackIndex = WordsRecords.Count - 1;
                        SpeakAsync();
                    }
                }));
                PlayThread.IsBackground = true;
                PlayThread.Start();
            }
            SuspendManualResetEvent.Set();
            DelayManualResetEvent.Set();
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
                    ProgramConfig.Default.ShowTime = value;
                    ProgramConfig.Default.Save();
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
                    ProgramConfig.Default.FadeIn = value;
                    ProgramConfig.Default.Save();
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
                    ProgramConfig.Default.FadeOut = value;
                    ProgramConfig.Default.Save();
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
                    ProgramConfig.Default.Random = value;
                    ProgramConfig.Default.Save();
                }
            }
        }
        private bool _ShowExplain;
        public bool ShowExplain
        {
            get { return _ShowExplain; }
            set
            {
                SetProperty(ref _ShowExplain, value, nameof(ShowExplain));
                ProgramConfig.Default.ShowExplain = value;
                ProgramConfig.Default.Save();
            }
        }
        private bool _WordsDistinct;
        public bool WordsDistinct
        {
            get { return _WordsDistinct; }
            set
            {
                SetProperty(ref _WordsDistinct, value, nameof(WordsDistinct));
            }
        }
        
        public int WordIndex { get; set; } = 0;
        public int BackIndex 
        {
            get;
            set;
        } = 0;

        public List<WordMode> WordsRecords { get; set; } = new List<WordMode>();
        public Thread PlayThread { get; set; }
        public Random ran { get; set; }
        private ManualResetEvent DelayManualResetEvent { get; set; }
        private ManualResetEvent SuspendManualResetEvent { get; set; }
        public bool IsPlay { get { return Status == PlayStatus.Play; } }
        public void Play()
        {
            if (Status != PlayStatus.Stop)
            {
                StartPlayClick();
            }
            Status = PlayStatus.Play;
            SuspendManualResetEvent.Set();
        }

        public void SuspendPlay()
        {
            SuspendManualResetEvent.Reset();
            Status = PlayStatus.Pause;
        }

        public void NextWord()
        {
            SuspendPlay();
            BackIndex++;
            if (BackIndex < WordsRecords.Count && BackIndex > 0)
            {
                WordPlayViewMode.Instance.Word = WordsRecords[BackIndex];
                WordPlayViewMode.Instance.Word.ShowCount++;
                SpeakAsync();
            }
            else
            {
                Task.Run(new Func<bool>(() =>
                {
                    var Word = new WordMode("");
                    List<WordMode> Words = new List<WordMode>();

                    Words = TypeWordViewMode.Instance.TypeWord.FindAll((index) => !index.IsOk);
                    if (Words == null) return false;
                    if (Words.Count <= 0) return false;

                    again: if (Random)
                    {
                        WordIndex = ran.Next(0, Words.Count);
                    }
                    else
                    {
                        WordIndex += 1;
                        if (WordIndex >= Words.Count)
                        {
                            WordIndex = 0;
                        }
                    }
                    if (WordIndex < Words.Count)
                    {
                        Word = Words[WordIndex];
                        Word.AsynTrans();
                    }
                    else
                    {
                        goto again;
                    }

                    WordPlayViewMode.Instance.Word = Word;
                    WordPlayViewMode.Instance.Word.ShowCount++;

                    WordsRecords.Add(Word);
                    BackIndex = WordsRecords.Count - 1;
                    SpeakAsync();
                    return true;
                }));
            }
        }
        public void BackWord()
        {
            SuspendPlay();
            BackIndex--;
            if (BackIndex < WordsRecords.Count && BackIndex > 0)
            {
                WordPlayViewMode.Instance.Word = WordsRecords[BackIndex];
                WordPlayViewMode.Instance.Word.ShowCount++;
                SpeakAsync();
            }
        }

        public void SpeakAsync()
        {
            synth?.SpeakAsync(WordPlayViewMode.Instance.Word.Word);
        }
    }
    enum PlayStatus
    {
        Stop,
        Pause,
        Play,

    }
}
