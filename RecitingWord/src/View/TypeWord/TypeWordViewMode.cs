using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace RecitingWord
{
    class TypeWordViewMode: MVVM.ViewModeBase
    {
        public static TypeWordViewMode Instance { get; } = new TypeWordViewMode();
        TypeWordViewMode()
        {
            Load = new MVVM.Command(LoadHandle);
            TypeWord = ParseStringToWords(ProgramConfig.Default.WordHistory);
        }

        private void LoadHandle(object sender)
        {
            if (sender as TypeWord != null)
            {
                TypeWordWindow = (sender as TypeWord);
                TypeWordWindow.TypeWordsTextBox.TextChanged += TypeWordsTextBox_TextChanged;
                TypeWordWindow.TypeWordsTextBox.Text = ProgramConfig.Default.WordHistory;
            }

        }

        public void TypeWordsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var Textbox = sender as TextBox;
            if (Textbox != null)
            {
                TypeWord = ParseStringToWords(Textbox.Text);
                ProgramConfig.Default.WordHistory = Textbox.Text;
                ProgramConfig.Default.Save();
                SettingViewMode.Instance.BackIndex = 0;
                SettingViewMode.Instance.WordIndex = 0;
                SettingViewMode.Instance.WordsRecords.Clear();
            }
        }

        public void SetWords(string Text)
        {
            TypeWord = ParseStringToWords(Text);
            ProgramConfig.Default.WordHistory = Text;
            ProgramConfig.Default.Save();
            SettingViewMode.Instance.BackIndex = 0;
            SettingViewMode.Instance.WordIndex = 0;
            SettingViewMode.Instance.WordsRecords.Clear();
        }

        public Regex MatchWord = new Regex(@"(([A-Za-z]|[a-z]|['])+)|([^\r\n\s])|(\r\n)|( )", RegexOptions.Compiled);
        List<WordMode> ParseStringToWords(string Word)
        {
            var Words = ParseString(Word);
            if (Words == null) return new List<WordMode>();
            if (Words.Count <= 0) return new List<WordMode>();
            //Words = (from item in Words where item.Word.Length >= SettingViewMode.Instance.MinWordLength select item).ToList();
            View.WordClickViewMode.Instance.AddWrods(Words);
            return Words;
        }

        private List<WordMode> ParseString(string Word)
        {
            if (string.IsNullOrWhiteSpace(Word)) return new List<WordMode>();
            Dictionary<WordMode, int> Words = new Dictionary<WordMode, int>();
            var WordList = new List<WordMode>();

            var MatchResult = MatchWord.Matches(Word);
            if (SettingViewMode.Instance.WordsDistinct) //如果唯一
            {
                if (SettingViewMode.Instance.RepetitionFrequency > 0) //如果设置重复频率
                {
                    foreach (Match item in MatchResult)
                    {
                        if (Words.ContainsKey(new WordMode(item.Value)))
                        {
                            Words[new WordMode(item.Value)]++;

                            if (Words[new WordMode(item.Value)] >= SettingViewMode.Instance.RepetitionFrequency) //如果到达重复
                            {
                                foreach (var Item in Words)
                                {
                                    Item.Key.Frequency = Item.Value;
                                }
                                WordList.AddRange(from wi in Words select wi.Key);
                                Words.Clear();
                            }
                        }
                        else
                        {
                            Words.Add(new WordMode(item.Value), 1);
                        }
                    }
                }
                else//如果没设置重复频率
                {
                    foreach (Match item in MatchResult)
                    {
                        if (Words.ContainsKey(new WordMode(item.Value)))
                        {
                            Words[new WordMode(item.Value)]++;
                        }
                        else
                        {
                            Words.Add(new WordMode(item.Value), 1);
                        }
                    }
                }
                foreach (var Item in Words)
                {
                    Item.Key.Frequency = Item.Value;
                    WordList.Add(Item.Key);
                }
                return WordList;
            }
            else//如果允许重复
            {
                if (SettingViewMode.Instance.RepetitionFrequency > 0)//如果设置重复频率
                {
                    foreach (Match item in MatchResult)
                    {
                        if (Words.ContainsKey(new WordMode(item.Value)))
                        {
                            Words[new WordMode(item.Value)]++;

                            if (Words[new WordMode(item.Value)] >= SettingViewMode.Instance.RepetitionFrequency) //如果到达重复
                            {
                                foreach (var Item in Words)
                                {
                                    Item.Key.Frequency = Item.Value;
                                }
                                WordList.AddRange(from wi in Words select wi.Key);
                                Words.Clear();
                            }
                        }
                        else
                        {
                            Words.Add(new WordMode(item.Value), 1);
                        }
                    }
                    foreach (var Item in Words)
                    {
                        Item.Key.Frequency = Item.Value;
                        WordList.Add(Item.Key);
                    }
                    return WordList;
                }
                else //如果没设置重复频率
                {
                    var words = new List<WordMode>();
                    foreach (Match item in MatchResult)
                    {
                        words.Add(new WordMode(item.Value));
                    }
                    return words;
                }
            }
        }

        public TypeWord TypeWordWindow { get; set; }

        /// <summary>
        /// 单词
        /// </summary>
        private List<WordMode> _TypeWord = new List<WordMode>();
        public List<WordMode> TypeWord
        {
            get { return _TypeWord; }
            set
            {
                _TypeWord = value;
                ProperChange(nameof(TypeWord));
                ShowWordListViewMode.Instance.Words = value;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _TypeWords;

        public string TypeWords
        {
            get { return _TypeWords; }
            set
            {
                _TypeWords = value;
                ProperChange(nameof(TypeWords));
            }
        }

        private ICommand _Load;

        public ICommand Load
        {
            get { return _Load; }
            set
            {
                _Load = value;
                ProperChange(nameof(Load));
            }
        }

    }
}
