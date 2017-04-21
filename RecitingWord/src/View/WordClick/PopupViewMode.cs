using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecitingWord.View
{
    class PopupViewMode:MVVM.ViewModeBase
    {
        static PopupViewMode _Instance = new PopupViewMode();
        public static PopupViewMode Instance
        {
            get
            {
                return _Instance;
            }

        }
        private PopupViewMode()
        {

        }

        private bool _IsPopup;
        public bool IsPopup
        {
            get { return _IsPopup; }
            set { SetProperty(ref _IsPopup, value, nameof(IsPopup)); }
        }


        private object _PlacementTarget;
        public object PlacementTarget
        {
            get { return _PlacementTarget; }
            set { SetProperty(ref _PlacementTarget, value, nameof(PlacementTarget)); }
        }

        private string _Text;
        public string Text
        {
            get { return _Text; }
            set { SetProperty(ref _Text, value, nameof(Text)); }
        }
    } 
}
