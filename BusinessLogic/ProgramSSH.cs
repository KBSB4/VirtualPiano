using Microsoft.Data.SqlClient;
using Renci.SshNet;
using Renci.SshNet.Common;
using System.Diagnostics;

namespace BusinessLogic
{
    class ProgramSSH
    {
        static SshClient client;
        private static ForwardedPortLocal port;

        /// <summary>
        /// makes a connection to the remote database (Microsoft SQL server) on a Linux VM. 
        /// </summary>
        public static void ExecuteSshConnection()
        {
            client = new("145.44.234.89", "student", "Arena-Enclose8");
            //   client.KeepAliveInterval = TimeSpan.FromMinutes(1);
            client.Connect();

            port = new ForwardedPortLocal("127.0.0.1", Convert.ToUInt32("1433"), "127.0.0.1", Convert.ToUInt32("1433"));
            client.AddForwardedPort(port);

            port.Exception += delegate (object? sender, ExceptionEventArgs e)
            {
                Debug.WriteLine("---------------------Port error");
                Debug.WriteLine(e.Exception.Message);
            };
            port.RequestReceived += (object sender, PortForwardEventArgs e) =>
            {
                Debug.WriteLine("Tunnel connection: {0}->{1}", e.OriginatorHost, e.OriginatorPort);
            };
            port.Start();
            Thread.Sleep(10000000);

            const string connectionString =
                    "Server=127.0.0.1:1433" +
           //  "Data Source=localhost;" +
           "Initial Catalog=PianoHero;" +
           //           "Persist Security Info=True;" +
           "User ID=SA;" +
           "Password=Backing-Crumpet4;";
            //"TrustServerCertificate=True;";
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                cnn.Open();
                Debug.WriteLine("QQQQ OK");
                SqlCommand cmd = new SqlCommand("SELECT * FROM PostalCodes LIMIT 25;", cnn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    Debug.WriteLine($"{reader.GetString(1)}, {reader.GetString(2)}, {reader.GetString(3)}");

                Debug.WriteLine("Ok");

                cnn.Close();
            }

            // client.Disconnect();

            //ProcessStartInfo cmdStartInfo = new()
            //{
            //    FileName = @"C:\Windows\System32\cmd.exe",
            //    RedirectStandardOutput = true,
            //    RedirectStandardError = true,
            //    RedirectStandardInput = true,
            //    UseShellExecute = false,
            //    CreateNoWindow = true
            //};

            //Process cmdProcess = new()
            //{
            //    StartInfo = cmdStartInfo,
            //    EnableRaisingEvents = true
            //};
            //cmdProcess.Start();
            //cmdProcess.BeginOutputReadLine();
            //cmdProcess.BeginErrorReadLine();

            //cmdProcess.StandardInput.WriteLine("ssh -L 1433:localhost:1433 student@145.44.234.89");
            //cmdProcess.StandardInput.WriteLine("Arena-Enclose8");

            //cmdProcess.WaitForExit();

            //ForwardedPortDynamic port = new(1433);
            //var port = new ForwardedPortLocal("localhost", 1433, "localhost", 1433);
            //client.AddForwardedPort(port);

            //port.Exception += delegate (object? sender, ExceptionEventArgs e)
            //{
            //    Debug.WriteLine("---------------------Port error");
            //    Debug.WriteLine(e.Exception.Message);
            //};
            //port.Start();



            //client.Disconnect();

            //using SshClient client = new("145.44.234.89", "student", "Arena-Enclose8");
            //client.KeepAliveInterval = new TimeSpan(0, 2, 0);
            //client.Connect();
            //client.RunCommand("wall test");
            ////ForwardedPortDynamic port = new(1433);
            ////var port = new ForwardedPortLocal("localhost", 1433, "localhost", 1433);
            ////client.AddForwardedPort(port);

            ////port.Exception += delegate (object? sender, ExceptionEventArgs e)
            ////{
            ////    Debug.WriteLine(e.Exception.Message);
            ////};
            ////port.Start();
            ////Debug.WriteLine(port.IsStarted);
            ////System.Threading.Thread.Sleep(1000 * 60 * 60 * 8);
            ////port.Stop();
            //client.Disconnect();

            //string password = "Arena-Enclose8";
            //string user = "student";
            //string hostname = "145.44.234.89";
            //string argumentExtra = "1433:localhost:1433";
            //ProcessStartInfo startInfo = new();
            //Process process = new();
            //startInfo.FileName = "plink.exe";

            //startInfo.Arguments = $"-ssh -L {argumentExtra} {user}@{hostname} -pw {password}";
            //startInfo.CreateNoWindow = false;
            //process.StartInfo = startInfo;

            //process.Start();

            //process.OutputDataReceived += (sender, args) => Debug.WriteLine(args.Data);
            //process.ErrorDataReceived += (sender, args) => Debug.WriteLine(args.Data);
        }
    }
}
