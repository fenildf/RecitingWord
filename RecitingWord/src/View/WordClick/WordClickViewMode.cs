using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace RecitingWord.View
{
    public class WordClickViewMode:MVVM.ViewModeBase
    {
        public static WordClickViewMode Instance { get; } = new WordClickViewMode();
        WordClickViewMode()
        {
            Words = new ObservableCollection<Passage>();
            PopupClose = new MVVM.Command(() => { PopupViewMode.Instance.IsPopup = false; });
            Loaded = new MVVM.Command((sender) => 
            {
                var Control = (sender as UserControl);
                if (Control == null) return;
                Control.PreviewMouseDown += Control_PreviewMouseDown;
            });

            LeftDoubleClick = new MVVM.Command(LeftDoubleClickHandle);
        }

        private void LeftDoubleClickHandle()
        {
            SettingViewMode.Instance.RereadAsync(GlobalWords.Instance.Words);
        }

        private void Control_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            PopupViewMode.Instance.IsPopup = false;
        }

        public ObservableCollection<Passage> Words{ get; set; }
        public void AddWrods(IEnumerable<WordMode> words)
        {
            Words.Clear();
            GC.Collect();

            var passage = new Passage();
            foreach (var item in words)
            {
                if (item.Word != "\r\n")
                {
                    passage.Words.Add(item);
                }
                else
                {
                    Words.Add(passage);
                    passage = new Passage();
                }
            }
            if (passage.Words.Count > 0)
            {
                Words.Add(passage);
            }
        }
        void WordClick(object sender)
        {
            //var word = sender as WordViewMode;
            //if (word == null) return;
            //word.AsynTrans();
        }

        private ICommand _PopupClose;
        public ICommand PopupClose
        {
            get { return _PopupClose; }
            set { SetProperty(ref _PopupClose, value, nameof(PopupClose)); }
        }

        private ICommand _Loaded;
        public ICommand Loaded
        {
            get { return _Loaded; }
            set { SetProperty(ref _Loaded, value, nameof(Loaded)); }
        }
        private ICommand _LeftDoubleClick;
        public ICommand LeftDoubleClick
        {
            get { return _LeftDoubleClick; }
            set { SetProperty(ref _LeftDoubleClick, value, nameof(LeftDoubleClick)); }
        }


    }
}
