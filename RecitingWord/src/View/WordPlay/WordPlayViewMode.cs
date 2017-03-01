using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecitingWord
{
    class WordPlayViewMode : Microsoft.Practices.Prism.ViewModel.NotificationObject
    {
        static WordPlayViewMode _Instance = new WordPlayViewMode();
        public static WordPlayViewMode Instance
        {
            get
            {
                return _Instance;
            }

        }
        private WordPlayViewMode()
        {

        }
        private string _Word;
        public string Word
        {
            get { return _Word; }
            set
            {
                if (_Word != value)
                {
                    _Word = value;
                    RaisePropertyChanged(nameof(Word));
                }
            }
        }
        private double _WordOpacity;
        public double WordOpacity
        {
            get { return _WordOpacity; }
            set
            {
                if (_WordOpacity != value)
                {
                    if (value > 1) _WordOpacity = 1;
                    else if (value < 0) _WordOpacity = 0;
                    else _WordOpacity = value;

                    RaisePropertyChanged(nameof(WordOpacity));
                }
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

        private double _WordExplainingOpacity;
        public double WordExplainingOpacity
        {
            get { return _WordExplainingOpacity; }
            set
            {
                if (_WordExplainingOpacity != value)
                {
                    if (value > 1) _WordExplainingOpacity = 1;
                    else if (value < 0) _WordExplainingOpacity = 0;
                    else _WordExplainingOpacity = value;

                    RaisePropertyChanged(nameof(WordExplainingOpacity));
                }
            }
        }
    }
}
