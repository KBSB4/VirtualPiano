using Renci.SshNet;

namespace BusinessLogic
{
    class ProgramSSH
    {
        /// <summary>
        /// makes a connection to the remote database (Microsoft SQL server) on a Linux VM. 
        /// </summary>
        public static void ExecuteSshConnection()
        {
            ConnectionInfo connectionInfo = new("145.44.234.89", "student", new PasswordAuthenticationMethod("student", "Arena-Enclose8"));
            using SftpClient client = new(connectionInfo);
            client.Connect();


            //string password = "Arena-Enclose8";
            //string user = "student";
            //string hostname = "145.44.234.89";
            //string listen = "1433:localhost:1433";
            //int port = 22;

            //Process process = new();
            //process.StartInfo.FileName = ProjectSettings.GetPath(PianoHeroPath.BatchFolder);
            ////process.StartInfo.FileName = "plink.exe";
            //process.StartInfo.Arguments = $"-ssh {hostname} -P {port} -l {user} -L {listen}  -pw {password} -batch -N";
            //process.StartInfo.CreateNoWindow = true;
            //process.StartInfo.UseShellExecute = false;

            //process.Start();

            //process.OutputDataReceived += (sender, args) => Debug.WriteLine(args.Data);
            //process.ErrorDataReceived += (sender, args) => Debug.WriteLine(args.Data);

        }
    }
}
