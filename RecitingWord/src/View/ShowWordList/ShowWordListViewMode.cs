using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecitingWord
{
    class ShowWordListViewMode: Microsoft.Practices.Prism.ViewModel.NotificationObject
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
                RaisePropertyChanged(nameof(Words));
            }
        }

    }
}
