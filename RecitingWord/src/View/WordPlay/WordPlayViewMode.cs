using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RecitingWord
{
    class WordPlayViewMode : MVVM.ViewModeBase
    {
        static WordPlayViewMode _Instance = new WordPlayViewMode();
        public static WordPlayViewMode Instance
        {
            get
            {
                return _Instance;
            }

        }
        private WordPlayViewMode()
        {
            MouseLeave = new MVVM.Command(() => Mouse = false);
            MouseEnter = new MVVM.Command(() => Mouse = true);
            MouseLeftClick = new MVVM.Command(MouseLeftClickHandle);
        }

        private void MouseLeftClickHandle()
        {
            if (SettingViewMode.Instance.IsPlay)
            {
                SettingViewMode.Instance.SuspendPlay();
            }
            else
            {
                SettingViewMode.Instance.Play();
            }
        }

        private WordMode _Word = new WordMode("");
        public WordMode Word
        {
            get { return _Word; }
            set
            {
                if (_Word != value)
                {
                    _Word = value;
                    ProperChange(nameof(Word));
                }
            }
        }
        private double _WordOpacity = 1;
        public double WordOpacity
        {
            get { return _WordOpacity; }
            set
            {
                if (_WordOpacity != value)
                {
                    if (value > 1) _WordOpacity = 1;
                    else if (value < 0) _WordOpacity = 0;
                    else _WordOpacity = value;

                    ProperChange(nameof(WordOpacity));
                }
            }
        }


        private double _WordExplainingOpacity = 1;
        public double WordExplainingOpacity
        {
            get { return (Mouse| SettingViewMode.Instance.ShowExplain) ? _WordExplainingOpacity : 0; }
            set
            {
                if (_WordExplainingOpacity != value)
                {
                    if (value > 1) _WordExplainingOpacity = 1;
                    else if (value < 0) _WordExplainingOpacity = 0;
                    else _WordExplainingOpacity = value;

                    ProperChange(nameof(WordExplainingOpacity));
                }
            }
        }

        private ICommand _MouseLeave;
        public ICommand MouseLeave
        {
            get { return _MouseLeave; }
            set
            {
                if (_MouseLeave != value)
                {
                    _MouseLeave = value;
                    ProperChange(nameof(MouseLeave));
                }
            }
        }

        private ICommand _MouseEnter;
        public ICommand MouseEnter
        {
            get { return _MouseEnter; }
            set
            {
                if (_MouseEnter != value)
                {
                    _MouseEnter = value;
                    ProperChange(nameof(MouseEnter));
                }
            }
        }

        private ICommand _MouseLeftClick;
        public ICommand MouseLeftClick
        {
            get { return _MouseLeftClick; }
            set
            {
                if (_MouseLeftClick != value)
                {
                    _MouseLeftClick = value;
                    ProperChange(nameof(MouseLeftClick));
                }
            }
        }

        private bool _Mouse;

        public bool Mouse
        {
            get { return _Mouse; }
            set
            {
                _Mouse = value;
                ProperChange(nameof(WordExplainingOpacity));
            }
        }

    }
}
