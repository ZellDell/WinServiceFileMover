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
       
        public FileMoverWinService()
        {
            InitializeComponent();
            
        }


        protected override void OnStart(string[] args)
        {
            
        }

        protected override void OnStop()
        {
            
        }


        
    }
}
