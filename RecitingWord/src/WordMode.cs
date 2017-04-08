using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecitingWord
{
    class WordMode:MVVM.ViewModeBase
    {
        public WordMode(string Word)
        {
            this.Word = Word;
        }

        private string _Word;
        /// <summary>
        /// 单词
        /// </summary>
        public string Word
        {
            get { return _Word; }
            set
            {
                _Word = value;
                ProperChange(nameof(Word));
            }
        }
        private string _WordExplaining;

        /// <summary>
        /// 解释
        /// </summary>
        public string WordExplaining
        {
            get
            {
                return _WordExplaining;
            }
            set
            {
                if (_WordExplaining != value)
                {
                    _WordExplaining = value;
                    ProperChange(nameof(WordExplaining));
                }
            }
        }
        private int _ShowCount = 0;
        /// <summary>
        /// 显示次数
        /// </summary>
        public int ShowCount
        {
            get { return _ShowCount; }
            set
            {
                _ShowCount = value;
                ProperChange(nameof(ShowCount));
            }
        }

        private int _Frequency = 0;
        /// <summary>
        /// 出现次数
        /// </summary>
        public int Frequency
        {
            get { return _Frequency; }
            set
            {
                _Frequency = value;
                ProperChange(nameof(Frequency));
            }
        }

        private bool _IsOk;
        /// <summary>
        /// 已经熟悉
        /// </summary>
        public bool IsOk
        {
            get { return _IsOk; }
            set
            {
                _IsOk = value;
                ProperChange(nameof(IsOk));
            }

        }

        private string _AmE;
        public string AmE
        {
            get
            {
                if (!SettingViewMode.Instance.ShowPhonetic) return string.Empty;
                return _AmE;
            }
            set
            {
                if (_AmE != value)
                {
                    _AmE = value;
                    ProperChange(nameof(AmE));
                }
            }
        }
        private string _BrE;
        public string BrE
        {
            get
            {
                if (!SettingViewMode.Instance.ShowPhonetic) return string.Empty;
                return _BrE;
            }
            set
            {
                if (_BrE != value)
                {
                    _BrE = value;
                    ProperChange(nameof(BrE));
                }
            }
        }

        public List<defs> defs { get; set; }

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
        public override string ToString()
        {
            return Word;
        }
        /// <summary>
        /// 异步翻译
        /// </summary>
        public void AsynTrans()
        {
            if (string.IsNullOrWhiteSpace(WordExplaining))
            Task.Run(() => {
                try
                {
                    var TransResult = BingTransApi.getTransResult(Word);
                    this.AmE = TransResult.AmE;
                    this.BrE = TransResult.BrE;
                    this.defs = TransResult.defs;
                    this.WordExplaining = string.Join("\r\n", defs);
                }
                catch (Exception)
                {
                    
                }
            });
        }
    }
}
