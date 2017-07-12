using MVVM;
using RecitingWord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecitingWord.View
{
    public class Passage:ViewModeBase
    {
        public Passage()
        {
            Words = new System.Collections.ObjectModel.ObservableCollection<WordMode>();
        }
        public Passage(List<WordMode> words)
        {
            Words = new System.Collections.ObjectModel.ObservableCollection<WordMode>(words);
        }
        private System.Collections.ObjectModel.ObservableCollection<WordMode> _Words;
        public System.Collections.ObjectModel.ObservableCollection<WordMode> Words
        {
            get { return _Words; }
            set { SetProperty(ref _Words, value, nameof(Words)); }
        }
    }
}
