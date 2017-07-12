using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecitingWord
{
    class GlobalWords
    {
        static GlobalWords _Instance = new GlobalWords();
        public static GlobalWords Instance
        {
            get
            {
                return _Instance;
            }

        }
        private GlobalWords()
        {
            _Words = string.Empty;
        }

        private string _Words;

        public string Words
        {
            get { return _Words; }
            set
            {
                _Words = value == null ? string.Empty : value;
            }
        }

    } 
}
