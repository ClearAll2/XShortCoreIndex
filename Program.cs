using System;
using System.Threading;
using System.Windows.Forms;

namespace XShortCoreIndex
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static double _interval = 24;
        private static String _dataPath = String.Empty;
        private static String _targetPath = String.Empty;
        

        public static string DataPath { get => _dataPath; set => _dataPath = value; }
        public static string TargetPath { get => _targetPath; set => _targetPath = value; }
        public static double Interval { get => _interval; set => _interval = value; }

        private static Mutex m_Mutex;
        [STAThread]
        static void Main(String[] args)
        {
            _dataPath = args[0];
            _targetPath = args[1];
            _interval = Double.Parse(args[2]);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            m_Mutex = new Mutex(true, "XShortBackground");

            if (m_Mutex.WaitOne(0, false))
            {
                Application.Run(new BackgroundActivity());     
            }
            else
            {
                return;
            }
            
        }
    }
}
