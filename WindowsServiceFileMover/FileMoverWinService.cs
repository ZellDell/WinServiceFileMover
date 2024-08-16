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
