using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using Forms = System.Windows.Forms;

namespace ShockCollar
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);
        private readonly Forms.NotifyIcon notifyIcon;
        const Int32 SW_SHOWNORMAL = 1;
        const Int32 SW_SHOW = 5;
        const Int32 SW_SHOWMAXIMIZED = 3;
        public App()
        {
            Process currentProcess = Process.GetCurrentProcess();
            var runningProcess = (from process in Process.GetProcesses()
                                  where
                                    process.Id != currentProcess.Id &&
                                    process.ProcessName.Equals(
                                      currentProcess.ProcessName,
                                      StringComparison.Ordinal)
                                  select process).FirstOrDefault();
            if (runningProcess != null)
            {
               // bool test= ShowWindow(runningProcess.MainWindowHandle, SW_SHOW);
              //  bool test = ShowWindow(runningProcess.MainWindowHandle, SW_SHOWMAXIMIZED);
                System.Windows.Application.Current.Shutdown();
            }
            else
            {
                notifyIcon = new Forms.NotifyIcon();
            }
        }
        //[STAThread]
        //public static void Main()
        //{
        //    ShockCollar.App app = new ShockCollar.App();
        //    app.InitializeComponent();
        //    app.Run();
        //}
  
        protected override void OnStartup(StartupEventArgs e)
        {
            
         
            notifyIcon.Icon = new System.Drawing.Icon("xrshock.ico");
            notifyIcon.Visible = true;
            notifyIcon.Click += NotifyIcon_Click;
            base.OnStartup(e);
        }

        private void NotifyIcon_Click(object? sender, EventArgs e)
        {
           MainWindow.WindowState= WindowState.Normal;
            MainWindow.Activate();
            
           
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (notifyIcon != null) { notifyIcon.Dispose(); }
            base.OnExit(e);
        }
        

        
}

}
