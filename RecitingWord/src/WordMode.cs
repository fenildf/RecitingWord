using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace RecitingWord
{
    public class WordMode:MVVM.ViewModeBase
    {
        public WordMode(string Word)
        {
            this.Word = Word;
            //ToolTipOpening = new MVVM.Command();
            //this.AsynTrans();
            Command = new MVVM.Command(WordClickHandle);
            TouchUp = new MVVM.Command(TouchUpHandle);
            TouchDown = new MVVM.Command(TouchDownHandle);
            TouchMove = new MVVM.Command(TouchMoveHandle);
            //Touch.FrameReported += Touch_FrameReported;
            PreviewMouseLeftButtonDown = new MVVM.Command(PreviewMouseLeftButtonDownHandle);
            PreviewMouseLeftButtonUp = new MVVM.Command(PreviewMouseLeftButtonUpHandle);

            //MouseMove = new MVVM.Command(MouseMoveHandle);
        }

        public bool MultiSelectModel { get; set; } = false;
        private void PreviewMouseLeftButtonUpHandle(object sender)
        {

        }
        private void PreviewMouseLeftButtonDownHandle()
        {
            if (MultiSelectModel = Keyboard.Modifiers == ModifierKeys.Control)
            {
                SelectedWordList.Instance.Words.Add(Word);
            }
            else
            {
                if (MultiSelectModel = SelectedWordList.Instance.Words.Count > 0)
                {
                    SelectedWordList.Instance.Words.Add(Word);
                    TouchUpHandle(this);
                }
            }
        }

        private void TouchMoveHandle()
        {
            SelectedWordList.Instance.Words.Add(Word);
        }

        private void TouchUpHandle(object sender)
        {
            if (SelectedWordList.Instance.Words.Count <= 1) return;
            Task.Run(()=> {
                var TouchWords = new StringBuilder();
                foreach (var Item in SelectedWordList.Instance.Words)
                {
                    if (string.IsNullOrWhiteSpace(Item)) continue;

                    TouchWords.AppendFormat($"{Item} ");
                }
                SettingViewMode.Instance.RereadAsync(TouchWords.ToString());
                SelectedWordList.Instance.Words.Clear();
                var result = GoogleTransApi.Instance.getSentenceTransResult(TouchWords.ToString());
                View.PopupViewMode.Instance.PlacementTarget = sender;
                View.PopupViewMode.Instance.IsPopup = false;
                View.PopupViewMode.Instance.IsPopup = true;
                if (result.defs.Count > 0)
                {
                    View.PopupViewMode.Instance.Text = string.Join("\r\n", (from item in result.defs select item.def).ToArray());
                }
                else
                {
                    View.PopupViewMode.Instance.Text = "翻译失败";
                }
                MultiSelectModel = false;
            });
        }

        private void TouchDownHandle(object sender)
        {
            //SelectedWordList.Instance.Words.Clear();
        }

        private void WordClickHandle(object sender)
        {
            if (SelectedWordList.Instance.Words.Count > 1 || MultiSelectModel) return;
            SelectedWordList.Instance.Words.Clear();
            SettingViewMode.Instance.RereadAsync(this.Word);


            Task.Run(() =>
            {
                Trans();

                View.PopupViewMode.Instance.PlacementTarget = sender as Button;
                View.PopupViewMode.Instance.IsPopup = false;
                View.PopupViewMode.Instance.IsPopup = true;
                View.PopupViewMode.Instance.Text = WordExplaining + $"\r\n{AmE}\t{BrE}";
                WordPlayViewMode.Instance.Word = this;
            });
        }

        public ICommand Command { get; set; }
        public ICommand ToolTipOpening { get; set; }
        public ICommand TouchUp { get; set; }
        public ICommand TouchDown { get; set; }
        public ICommand TouchMove { get; set; }
        public ICommand PreviewMouseLeftButtonDown { get; set; }
        public ICommand PreviewMouseLeftButtonUp { get; set; }
        public ICommand MouseMove { get; set; }

        
        #region 折叠
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
            if (string.IsNullOrWhiteSpace(WordExplaining) && !string.IsNullOrWhiteSpace(Word) && Word != ".")
                Task.Run(() =>
                {
                    try
                    {
                        var TransResult = BingTransApi.getTransResult(Word);
                        //var TransResult = GoogleTransApi.Instance.getTransResult(Word);
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
        public void Trans()
        {
            if (string.IsNullOrWhiteSpace(WordExplaining) && !string.IsNullOrWhiteSpace(Word))
            {
                try
                {
                    var TransResult = BingTransApi.getTransResult(Word);
                    //var TransResult = GoogleTransApi.Instance.getTransResult(Word);
                    this.AmE = TransResult.AmE;
                    this.BrE = TransResult.BrE;
                    this.defs = TransResult.defs;
                    this.WordExplaining = string.Join("\r\n", defs);
                }
                catch (Exception)
                {

                }
            }
        }
        #endregion
    }

    class SelectedWordList
    {
        static SelectedWordList _Instance = new SelectedWordList();
        public static SelectedWordList Instance
        {
            get
            {
                return _Instance;
            }

        }
        private SelectedWordList()
        {
            Words = new HashSet<string>();
        }
        public HashSet<string> Words { get; set; }
    }
}
#region MyRegion
//var button = sender as Button;
//if (button == null) return;

//var text = new TextBlock();
//var p = new Popup();
//text.Text = Word;
//p.Child = text;
//p.IsOpen = true; 
#endregion
