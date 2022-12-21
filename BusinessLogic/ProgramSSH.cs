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
            SshClient client = new("145.44.234.89", "student", "Arena-Enclose8");
            client.Connect();

            ForwardedPortLocal port = new("127.0.0.1", 1433, "127.0.0.1", 1433);
            client.AddForwardedPort(port);
            port.Start();
            Thread.Sleep(10000000);
        }
    }
}
