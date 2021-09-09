using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace XShortCoreIndex
{
    public partial class BackgroundActivity : Form
    {
        private bool exit = false;
        private string dataPath = String.Empty;
        private string targetPath = String.Empty;
        private string generalStartMenu = "C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs";
        private string userStartMenu = "AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs";
        private BackgroundWorker backgroundWorker;
        private double interval = 24;
        internal struct LASTINPUTINFO
        {
            public uint cbSize;

            public uint dwTime;
        }
        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        public static uint GetIdleTime()
        {
            LASTINPUTINFO lastInPut = new LASTINPUTINFO();
            lastInPut.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
            GetLastInputInfo(ref lastInPut);

            return ((uint)Environment.TickCount - lastInPut.dwTime);
        }


        public BackgroundActivity()
        {
            InitializeComponent();
            dataPath = Program.DataPath;
            targetPath = Program.TargetPath;
            interval = Program.Interval;
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.RunWorkerAsync();
            
        }

        

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((int)e.Result != 0)
            {
                Process.Start(Path.Combine(Application.StartupPath, "XShortCore.exe"), "reload");
                File.Copy(Path.Combine(dataPath, "temp1"), Path.Combine(dataPath, "folders"), true);
                File.Copy(Path.Combine(dataPath, "temp2"), Path.Combine(dataPath, "files"), true);
                File.WriteAllText(Path.Combine(dataPath, "index"), String.Empty);
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = 1;
            try
            {
                if (File.Exists(Path.Combine(dataPath, "index")))
                {
                    if (File.GetLastWriteTime(Path.Combine(dataPath, "index")).AddHours(interval) > DateTime.Now)
                    {
                        e.Result = 0;
                        return;
                    }
                }
                File.WriteAllText(Path.Combine(dataPath, "temp1"), String.Empty);
                File.WriteAllText(Path.Combine(dataPath, "temp2"), String.Empty);

                SearchFileAndFolder(generalStartMenu);
                SearchFileAndFolder(Path.Combine(targetPath, userStartMenu));
                if (targetPath != "Enhanced")
                    SearchFileAndFolder(targetPath);
                else
                {
                    foreach (var drive in DriveInfo.GetDrives())
                    {
                        if (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).Contains(drive.RootDirectory.FullName))
                            SearchFileAndFolder(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
                        else
                            SearchFileAndFolder(drive.RootDirectory.FullName);
                    }
                }
            }
            catch
            {
                e.Result = 0;
                return;
            }
        }

        private void SearchFileAndFolder(string dir)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(dir);
                DirectoryInfo[] dir1 = di.GetDirectories("*" + "*.*");
                string alldir = String.Empty;
                for (int i = 0; i < dir1.Count(); i++)
                {
                    if (!dir1[i].Attributes.HasFlag(FileAttributes.Hidden) && !dir1[i].Name.StartsWith("."))
                        alldir += dir1[i].FullName + Environment.NewLine;
                }
                File.AppendAllText(Path.Combine(dataPath, "temp1"), alldir);

                FileInfo[] files = di.GetFiles("*" + "*.*");
                string allfiles = String.Empty;
                for (int i = 0; i < files.Count(); i++)
                {
                    if (!files[i].Attributes.HasFlag(FileAttributes.Hidden) && !files[i].Name.StartsWith("."))
                        allfiles += files[i].FullName + Environment.NewLine;
                }
                File.AppendAllText(Path.Combine(dataPath, "temp2"), allfiles);

                DirectoryInfo[] dirs = di.GetDirectories();
                if (dirs == null || dirs.Length < 1)
                    return;
                foreach (DirectoryInfo sdir in dirs)
                {
                    while (GetIdleTime() <= 10000)
                    {
                        Thread.Sleep(500);
                        if (exit)
                        {
                            return;
                        }
                        Thread.Sleep(500);
                    }
                    if (!sdir.Attributes.HasFlag(FileAttributes.Hidden) && !sdir.Name.StartsWith("."))
                        SearchFileAndFolder(sdir.FullName);
                    Thread.Sleep(0);
                }
            }
            catch
            {
                return;
            }
        }

        private void BackgroundActivity_Load(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate { Hide(); }));
        }

        private void BackgroundActivity_FormClosing(object sender, FormClosingEventArgs e)
        {
            exit = true;
        }

        private void timerBackgroundCheck_Tick(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(dataPath, "interval")))
            {
                RegistryKey r = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ClearAll\\XShort\\Data", true);
                interval = Double.Parse((string)r.GetValue("Interval"));
                r.Close();
                r.Dispose();
                File.Delete(Path.Combine(dataPath, "interval"));
            }
            if (!backgroundWorker.IsBusy)
                backgroundWorker.RunWorkerAsync();
            
        }
    }

}
