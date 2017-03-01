using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecitingWord
{
    class WordPlayViewMode : MVVM.ViewModeBase
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
        private WordMode _Word = new WordMode("");
        public WordMode Word
        {
            get { return _Word; }
            set
            {
                if (_Word != value)
                {
                    _Word = value;
                    ProperChange(nameof(Word));
                }
            }
        }
        private double _WordOpacity = 1;
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

                    ProperChange(nameof(WordOpacity));
                }
            }
        }


        private double _WordExplainingOpacity = 1;
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

                    ProperChange(nameof(WordExplainingOpacity));
                }
            }
        }
    }
}
