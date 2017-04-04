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
                (sender as TypeWord).TypeWordsTextBox.TextChanged += TypeWordsTextBox_TextChanged;
                (sender as TypeWord).TypeWordsTextBox.Text = ProgramConfig.Default.WordHistory;
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
            }
        }


        Regex MatchWord = new Regex("([A-Z]|[a-z])[a-z]+", RegexOptions.Compiled);
        List<WordMode> ParseStringToWords(string Word)
        {
            if (string.IsNullOrWhiteSpace(Word)) return new List<WordMode>();
            Dictionary<WordMode, int> Words = new Dictionary<WordMode, int>();
            var MatchResult = MatchWord.Matches(Word);
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

            foreach (var Item in Words)
            {
                Item.Key.Frequency = Item.Value;
            }

            return Words.Keys.ToList();
        }


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
