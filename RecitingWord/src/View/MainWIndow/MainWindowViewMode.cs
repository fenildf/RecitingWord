using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecitingWord
{
    public class MainWindowViewMode:Microsoft.Practices.Prism.ViewModel.NotificationObject
    {
        public static MainWindowViewMode Instance { get; } = new MainWindowViewMode();
        MainWindowViewMode()
        {

        }

        private ObservableCollection<object> _Tabs;

        public ObservableCollection<object> Tabs
        {
            get { return _Tabs; }
            set
            {
                _Tabs = value;
                RaisePropertyChanged(nameof(Tabs));
            }
        }
   

    }
}
