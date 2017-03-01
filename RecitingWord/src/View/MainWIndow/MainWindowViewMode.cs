using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RecitingWord
{
    public class MainWindowViewMode:MVVM.ViewModeBase
    {
        public static MainWindowViewMode Instance { get; } = new MainWindowViewMode();
        MainWindowViewMode()
        {
            DeleteKeyDown = new MVVM.Command(() => WordPlayViewMode.Instance.Word.IsOk = true);
            Left = new MVVM.Command(() => Console.WriteLine("左"));

            Space = new MVVM.Command(() => {
                if (SettingViewMode.Instance.IsPlay)
                {
                    SettingViewMode.Instance.SuspendPlay();
                }
                else
                {
                    SettingViewMode.Instance.Play();
                }
            });
        }


        private ICommand _DeleteKeyDown;
        public ICommand DeleteKeyDown
        {
            get { return _DeleteKeyDown; }
            set { SetProperty(ref _DeleteKeyDown, value, nameof(DeleteKeyDown)); }
        }
        private ICommand _Left;
        public ICommand Left
        {
            get { return _Left; }
            set { SetProperty(ref _Left, value, nameof(Left)); }
        }
        private ICommand _Right;
        public ICommand Right
        {
            get { return _Right; }
            set { SetProperty(ref _Right, value, nameof(Right)); }
        }
        private ICommand _Up;
        public ICommand Up
        {
            get { return _Up; }
            set { SetProperty(ref _Up, value, nameof(Up)); }
        }
        private ICommand _Down;
        public ICommand Down
        {
            get { return _Down; }
            set { SetProperty(ref _Down, value, nameof(Down)); }
        }

        private ICommand _Space;
        public ICommand Space
        {
            get { return _Space; }
            set { SetProperty(ref _Space, value, nameof(Space)); }
        }

    }


}
