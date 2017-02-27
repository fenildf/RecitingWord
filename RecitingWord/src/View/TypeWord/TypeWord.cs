using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
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
    class TypeWordViewMode: NotificationObject
    {
        public static TypeWordViewMode Instance { get; } = new TypeWordViewMode();
        TypeWordViewMode()
        {
            Load = new DelegateCommand<RecitingWord.TypeWord>(LoadHandle);
        }

        private void LoadHandle(RecitingWord.TypeWord sender)
        {
            sender.TypeWordsTextBox.TextChanged += TypeWordsTextBox_TextChanged;
        }

        private void TypeWordsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var Textbox = sender as TextBox;
            if (Textbox != null)
            {
                TypeWord = ParseStringToWords(Textbox.Text);
            }
        }


        Regex MatchWord = new Regex("([A-Z]|[a-z])[a-z]+", RegexOptions.Compiled);
        List<WordMode> ParseStringToWords(string Word)
        {
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
        List<WordMode> TypeWord { get; set; }

        private string _TypeWords;

        public string TypeWords
        {
            get { return _TypeWords; }
            set
            {
                _TypeWords = value;
                RaisePropertyChanged(nameof(TypeWords));
            }
        }

        private ICommand _Load;

        public ICommand Load
        {
            get { return _Load; }
            set
            {
                _Load = value;
                RaisePropertyChanged(nameof(Load));
            }
        }

    }
}
