using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecitingWord
{
    class WordMode
    {
        public WordMode(string Word)
        {
            this.Word = Word;
        }
        public string Word { get; set; }
        public int ShowCount { get; set; } = 0;
        public int Frequency { get; set; } = 0;
        public bool IsOk { get; set; } = false;

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
