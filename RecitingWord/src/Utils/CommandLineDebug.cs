using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NORMAN.NRM411S7
{
    class CommandLineDebug
    {
        static CommandLineDebug _Instance = new CommandLineDebug();
        public static CommandLineDebug Instance
        {
            get
            {
                return _Instance;
            }

        }
        private bool _Showheart;
        public bool Showheart { get { return _Showheart; } set { _Showheart = value; } }

        public bool DEBUG { get; set; } = false;
        private CommandLineDebug()
        {
            foreach (var item in System.Environment.GetCommandLineArgs())
            {
                if (item.ToUpper() == "DEBUG")
                {
                    DEBUG = true;
                }
                else if(item.ToUpper() == "Console".ToUpper())
                {
                    NativeMethods.AllocConsole();
                    Console.OpenStandardOutput();
                }
            }
        }
    }
}
