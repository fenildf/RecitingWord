using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RecitingWord
{
    class Windows1ViewMode:MVVM.ViewModeBase
    {
        static Windows1ViewMode _Instance = new Windows1ViewMode();
        public static Windows1ViewMode Instance
        {
            get
            {
                return _Instance;
            }

        }
        private Windows1ViewMode()
        {
            C1 = new MVVM.Command(()=> {
                IsPopup = !IsPopup;
            });
        }

        private ICommand _C1;
        public ICommand C1
        {
            get { return _C1; }
            set { SetProperty(ref _C1, value, nameof(C1)); }
        }
        private bool _IsPopup;
        public bool IsPopup
        {
            get { return _IsPopup; }
            set { SetProperty(ref _IsPopup, value, nameof(IsPopup)); }
        }

    } 
}
