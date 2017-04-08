using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecitingWord.View
{
    class LoadViewMode:MVVM.ViewModeBase
    {
        static LoadViewMode _Instance = new LoadViewMode();
        public static LoadViewMode Instance
        {
            get
            {
                return _Instance;
            }

        }
        private LoadViewMode()
        {
            Task.Run(() => {
                while (true)
                {
                    Title = "加载中，请稍等。。。";
                    System.Threading.Thread.Sleep(500);
                    Title = "加载中，请稍等。。。。";
                    System.Threading.Thread.Sleep(500);
                    Title = "加载中，请稍等。。。。。";
                    System.Threading.Thread.Sleep(500);
                }
            });
        }
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, nameof(Title)); }
        }


    } 
}
