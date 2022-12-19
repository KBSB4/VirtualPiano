using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace BusinessLogic
{
    class ProgramSSH
    {
        /// <summary>
        /// makes a connection to the remote database (Microsoft SQL server) on a Linux VM. 
        /// </summary>
       public static void ExecuteSshConnection()
        {
            string password = "Arena-Enclose8";
            string user = "student";
            string hostname = "145.44.234.89";
            string argumentExtra = "1433:localhost:1433";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            Process process = new Process();
            startInfo.FileName = "plink.exe";

            startInfo.Arguments = $"-ssh -L {argumentExtra} {user}@{hostname} -pw {password}";
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            
            process.Start();
            process.OutputDataReceived += (sender, args) => Debug.WriteLine(args.Data);
            process.ErrorDataReceived += (sender, args) => Debug.WriteLine(args.Data);
            
        }
    }
}
