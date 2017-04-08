using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecitingWord
{
    class ShowWordListViewMode:MVVM.ViewModeBase
    {
        static ShowWordListViewMode _Instance = new ShowWordListViewMode();
        public static ShowWordListViewMode Instance
        {
            get
            {
                return _Instance;
            }

        }
        private ShowWordListViewMode()
        {

        }


        private List<WordMode> _Words;
        public List<WordMode> Words
        {
            get { return _Words; }
            set
            {
                _Words = value;
                ProperChange(nameof(Words));
            }
        }
        private int _SelectIndex;
        public int SelectIndex
        {
            get { return _SelectIndex; }
            set
            {
                SetProperty(ref _SelectIndex, value, nameof(SelectIndex));
                SettingViewMode.Instance.BackIndex =
                SettingViewMode.Instance.ManualWordIndex =
                SettingViewMode.Instance.WordIndex = value;
            }
        }
    }
}
