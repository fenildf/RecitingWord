using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RecitingWord
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1
    {
        static Window1 _Instance = new Window1();
        public static Window1 Instance
        {
            get
            {
                return _Instance;
            }

        }
        private Window1()
        {
            InitializeComponent();

        }
    }
}
