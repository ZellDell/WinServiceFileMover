using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceFileMover
{
    public partial class FileMoverWinService : ServiceBase
    {
        private FileSystemWatcher _watcher;
        private readonly string sourcePath = @"C:\Folder1";
        private readonly string destinationPath = @"C:\Folder2";
        private readonly string logDirectory = @"C:\FileMoverLog";


        public FileMoverWinService()
        {
            InitializeComponent();
            SetupService();


        }

        private void SetupService()
        {
            DoesDirectoryExists(sourcePath);
            DoesDirectoryExists(destinationPath);
            DoesDirectoryExists(logDirectory);

            // Setup Logging in Event Viewer
            ((ISupportInitialize)this.EventLog).BeginInit();
            if (!EventLog.SourceExists(this.ServiceName))
            {
                EventLog.CreateEventSource(this.ServiceName, "Application");
            }
            ((ISupportInitialize)this.EventLog).EndInit();

            this.EventLog.Source = this.ServiceName;
            this.EventLog.Log = "Application";


            SetupFileSystemWatcher();
        }

        private void SetupFileSystemWatcher()
        {
            _watcher = new FileSystemWatcher(sourcePath)
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
                Filter = "*.*"
            };
            _watcher.Created += OnCreated;


        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DoesDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                
            }
        }

        protected override void OnStart(string[] args)
        {
            
        }

        protected override void OnStop()
        {
            
        }


        
    }
}
