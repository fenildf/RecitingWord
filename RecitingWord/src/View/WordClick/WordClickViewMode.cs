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
            Words = new ObservableCollection<WordMode>();
            PopupClose = new MVVM.Command(() => { PopupViewMode.Instance.IsPopup = false; });
            Loaded = new MVVM.Command((sender) => 
            {
                var Control = (sender as UserControl);
                if (Control == null) return;
                Control.PreviewMouseDown += Control_PreviewMouseDown;
            });
            
        }

        private void Control_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            PopupViewMode.Instance.IsPopup = false;
        }

        public ObservableCollection<WordMode> Words{ get; set; }
        public void AddWrods(IEnumerable<WordMode> words)
        {
            Words.Clear();

            foreach (var item in words)
            {
                Words.Add(item);
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


    }
    //public class WordViewMode : WordMode
    //{
    //    public WordViewMode(WordMode Word):base(Word.Word)
    //    {
    //    }
    //}
}
