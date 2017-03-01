using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecitingWord
{
    class WordMode:Microsoft.Practices.Prism.ViewModel.NotificationObject
    {
        public WordMode(string Word)
        {
            this.Word = Word;
        }

        private string _Word;
        public string Word
        {
            get { return _Word; }
            set
            {
                _Word = value;
                RaisePropertyChanged(nameof(Word));
            }
        }
        private string _WordExplaining;
        public string WordExplaining
        {
            get { return _WordExplaining; }
            set
            {
                if (_WordExplaining != value)
                {
                    _WordExplaining = value;
                    RaisePropertyChanged(nameof(WordExplaining));
                }
            }
        }
        private int _ShowCount = 0;
        public int ShowCount
        {
            get { return _ShowCount; }
            set
            {
                _ShowCount = value;
                RaisePropertyChanged(nameof(ShowCount));
            }
        }

        private int _Frequency = 0;
        public int Frequency
        {
            get { return _Frequency; }
            set
            {
                _Frequency = value;
                RaisePropertyChanged(nameof(Frequency));
            }
        }

        private bool _IsOk;
        public bool IsOk
        {
            get { return _IsOk; }
            set
            {
                _IsOk = value;
                RaisePropertyChanged(nameof(IsOk));
            }

        }



        public override bool Equals(object obj)
        {
            var data = obj as WordMode;
            if (data == null) return false;
            if (data.Word != this.Word) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return this.Word.GetHashCode();
        }
    }
}
