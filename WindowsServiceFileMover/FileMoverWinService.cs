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
        private readonly string logFileName = "FileMoverService.log";
        private readonly int maxLogFileSizeInMB = 5;

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
            try
            {

                string fileName = Path.GetFileName(e.FullPath);

                string sourceFile = Path.Combine(sourcePath, fileName);
                string destFile = Path.Combine(destinationPath, fileName);

                File.Move(sourceFile, destFile);

                string message = $"Moved file {fileName} from {sourcePath} to {destinationPath}.";
                LogToEventViewer(message);
                LogToFile(message);


            }
            catch (Exception ex)
            {
                string errorMessage = $"Error: {ex.Message}";
                LogToEventViewer(errorMessage, EventLogEntryType.Error);
                LogToFile(errorMessage);
            }
        }

        private void LogToEventViewer(string message, EventLogEntryType type = EventLogEntryType.Information)
        {
            this.EventLog.WriteEntry(message, type);
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
            base.OnStart(args);
            _watcher.EnableRaisingEvents = true;
            this.EventLog.WriteEntry("FileMoverService started.");
            LogToFile("FileMoverService started.");
        }

        protected override void OnStop()
        {
            _watcher.EnableRaisingEvents = false;
            this.EventLog.WriteEntry("FileMoverService stopped.");
            LogToFile("FileMoverService stopped.");
        }

        private void LogToFile(string message)
        {
            try
            {
                string logFilePath = Path.Combine(logDirectory, logFileName);

                // Check if file needs rolling
                FileInfo fileInfo = new FileInfo(logFilePath);
                if (fileInfo.Exists && fileInfo.Length > maxLogFileSizeInMB * 1024 * 1024)
                {
                    string newFileName = $"{DateTime.Now:yyyyMMddHHmmss}_{logFileName}";
                    string newLogFilePath = Path.Combine(logDirectory, newFileName);
                    File.Move(logFilePath, newLogFilePath);
                }

                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    sw.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions related to file operations
                LogToEventViewer($"Error logging to file: {ex.Message}", EventLogEntryType.Error);
            }
        }

    }
}
