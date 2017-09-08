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
            ReloadWords = new MVVM.Command(ReloadWordsClick);
            

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
            synth.Volume            = ProgramConfig.Default.Volume;
            synth.SpeakCompleted    += Synth_SpeakCompleted;
            Rate                    = ProgramConfig.Default.Rate;
            RepetitionFrequency     = ProgramConfig.Default.RepetitionFrequency;
            RereadRate              = ProgramConfig.Default.RereadRate;
            MinWordLength           = ProgramConfig.Default.MinWordLength;
            MySqlConnectionString   = ProgramConfig.Default.MySqlConnectionString;
            AutoGetWord     = ProgramConfig.Default.AutoGetWord;
            Topmost         = ProgramConfig.Default.Topmost;
            ShowPhonetic    = ProgramConfig.Default.ShowPhonetic;
            WordClickFontSize = ProgramConfig.Default.WordClickFontSize;

            InstalledVoices = (from item in synth.GetInstalledVoices() where item.Enabled select item.VoiceInfo.Name).ToList();
            VoiceGender = Enum.GetValues(typeof(VoiceGender)).OfType<VoiceGender>().ToArray();
            VoiceAge = Enum.GetValues(typeof(VoiceAge)).OfType<VoiceAge>().ToArray();
        }



        private void ReloadWordsClick()
        {
            TypeWordViewMode.Instance.
                TypeWordsTextBox_TextChanged(TypeWordViewMode.Instance.TypeWordWindow.TypeWordsTextBox, null);
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
            catch (Exception ex) { /*Console.WriteLine(ex.Message);*/ }

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
        private ICommand _ReloadWords;
        public ICommand ReloadWords
        {
            get { return _ReloadWords; }
            set { SetProperty(ref _ReloadWords, value, nameof(ReloadWords)); }
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

        private int _RepetitionFrequency;
        public int RepetitionFrequency
        {
            get { return _RepetitionFrequency; }
            set
            {
                SetProperty(ref _RepetitionFrequency, value, nameof(RepetitionFrequency));
                ProgramConfig.Default.RepetitionFrequency = value;
                ProgramConfig.Default.Save();
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
        /// <summary>
        /// 单词唯一
        /// </summary>
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
        private int _ManualWordIndex;
        public int ManualWordIndex
        {
            get { return _ManualWordIndex; }
            set { SetProperty(ref _ManualWordIndex, value, nameof(ManualWordIndex)); BackIndex = WordIndex = value; }
        }
        private int _Rate;
        public int Rate
        {
            get { return _Rate; }
            set
            {
                SetProperty(ref _Rate, value, nameof(Rate));
                try
                {
                    synth.Rate = value;
                    ProgramConfig.Default.Rate = value;
                    ProgramConfig.Default.Save();
                }
                catch (Exception)
                {
                }
            }
        }
        private int _RereadRate;
        public int RereadRate
        {
            get { return _RereadRate; }
            set
            {
                SetProperty(ref _RereadRate, value, nameof(RereadRate));
                ProgramConfig.Default.RereadRate = value;
                ProgramConfig.Default.Save();
            }
        }

        private int _MinWordLength;
        public int MinWordLength
        {
            get { return _MinWordLength; }
            set
            {
                SetProperty(ref _MinWordLength, value, nameof(MinWordLength));
                ProgramConfig.Default.MinWordLength = value;
                ProgramConfig.Default.Save();
            }
        }

        private string _MySqlConnectionString;
        public string MySqlConnectionString
        {
            get { return _MySqlConnectionString; }
            set
            {
                SetProperty(ref _MySqlConnectionString, value, nameof(MySqlConnectionString));
                ProgramConfig.Default.MySqlConnectionString = value;
                ProgramConfig.Default.Save();
            }
        }

        public bool ConnectionMysql
        {
            get { return Mysql.IsHaveDatabase; }
            set {  }
        }
        private bool _AutoGetWord;
        public bool AutoGetWord
        {
            get { return _AutoGetWord; }
            set
            {
                SetProperty(ref _AutoGetWord, value, nameof(AutoGetWord));
                ProgramConfig.Default.AutoGetWord = value;
                ProgramConfig.Default.Save();
            }
        }
        private bool _Topmost;
        public bool Topmost
        {
            get { return _Topmost; }
            set
            {
                SetProperty(ref _Topmost, value, nameof(Topmost));
                ProgramConfig.Default.Topmost = value;
                ProgramConfig.Default.Save();
            }
        }
        private bool _ShowPhonetic;
        public bool ShowPhonetic
        {
            get { return _ShowPhonetic; }
            set
            {
                SetProperty(ref _ShowPhonetic, value, nameof(ShowPhonetic));
                ProgramConfig.Default.ShowPhonetic = value;
                ProgramConfig.Default.Save();
            }
        }
        private int _WordClickFontSize;
        public int WordClickFontSize
        {
            get { return _WordClickFontSize; }
            set
            {
                SetProperty(ref _WordClickFontSize, value, nameof(WordClickFontSize));
                ProgramConfig.Default.WordClickFontSize = value;
                ProgramConfig.Default.Save();
            }
        }

        private List<string> _InstalledVoices;
        public List<string> InstalledVoices
        {
            get { return _InstalledVoices; }
            set { SetProperty(ref _InstalledVoices, value, nameof(InstalledVoices)); }
        }
        private string _InstalledVoiceSelectItem;
        public string InstalledVoiceSelectItem
        {
            get { return _InstalledVoiceSelectItem; }
            set
            {
                SetProperty(ref _InstalledVoiceSelectItem, value, nameof(InstalledVoiceSelectItem));

                try
                {
                    synth.SelectVoice(InstalledVoiceSelectItem);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
        private VoiceGender[] _VoiceGender;
        public VoiceGender[] VoiceGender
        {
            get { return _VoiceGender; }
            set { SetProperty(ref _VoiceGender, value, nameof(VoiceGender)); }
        }
        private VoiceGender _VoiceGenderSelectItem;
        public VoiceGender VoiceGenderSelectItem
        {
            get { return _VoiceGenderSelectItem; }
            set
            {
                SetProperty(ref _VoiceGenderSelectItem, value, nameof(VoiceGenderSelectItem));
                try
                {
                    synth.SelectVoiceByHints(VoiceGenderSelectItem, VoiceAgeSelectItem);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
        private VoiceAge[] _VoiceAge;
        public VoiceAge[] VoiceAge
        {
            get { return _VoiceAge; }
            set { SetProperty(ref _VoiceAge, value, nameof(VoiceAge)); }
        }
        private VoiceAge _VoiceAgeSelectItem;
        public VoiceAge VoiceAgeSelectItem
        {
            get { return _VoiceAgeSelectItem; }
            set
            {
                SetProperty(ref _VoiceAgeSelectItem, value, nameof(VoiceAgeSelectItem));
                try
                {
                    synth.SelectVoiceByHints(VoiceGenderSelectItem, VoiceAgeSelectItem);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }






        //WordClickFontSize
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


        string overlayWord;
        public void SpeakAsync()
        {
            synth.Rate = Rate;
            if (synth.State == SynthesizerState.Ready)
                synth?.SpeakAsync(WordPlayViewMode.Instance.Word.Word);
            else
                overlayWord = WordPlayViewMode.Instance.Word.Word;
        }

        public void RereadAsync()
        {
            synth.Rate = RereadRate;
            if (synth.State == SynthesizerState.Ready)
                synth?.SpeakAsync(WordPlayViewMode.Instance.Word.Word);
            else
                overlayWord = WordPlayViewMode.Instance.Word.Word;
        }
        public void RereadAsync(string word)
        {
            GlobalWords.Instance.Words = word;
            synth.Rate = RereadRate;
            if (synth.State == SynthesizerState.Ready)
                synth?.SpeakAsync(word);
            else
                overlayWord = word;
        }
        private void Synth_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(overlayWord))
            {
                synth?.SpeakAsync(overlayWord);
                overlayWord = string.Empty;
            }
        }

        public void Read(string Word)
        {
            WordPlayViewMode.Instance.Word = new WordMode(Word);
            WordPlayViewMode.Instance.Word.AsynTrans();
            SpeakAsync();
        }
    }
    enum PlayStatus
    {
        Stop,
        Pause,
        Play,

    }
}
