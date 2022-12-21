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
<<<<<<< Updated upstream
            ConnectionInfo connectionInfo = new("145.44.234.89", "student", new PasswordAuthenticationMethod("student", "Arena-Enclose8"));
            using SftpClient client = new(connectionInfo);
            client.Connect();

=======
            client = new("145.44.234.89", "student", "Arena-Enclose8");
            client.Connect();

            port = new ForwardedPortLocal("127.0.0.1", 1433, "127.0.0.1", 1433);
            client.AddForwardedPort(port);

            //port.Exception += delegate (object? sender, ExceptionEventArgs e)
            //{
            //    Debug.WriteLine("---------------------Port error");
            //    Debug.WriteLine(e.Exception.Message);
            //};
            //port.RequestReceived += (object sender, PortForwardEventArgs e) =>
            //{
            //    Debug.WriteLine("Tunnel connection: {0}->{1}", e.OriginatorHost, e.OriginatorPort);
            //};
            port.Start();
            Thread.Sleep(10000000);

            // const string connectionString =
            //         "Server=127.0.0.1:1433" +
            ////  "Data Source=localhost;" +
            //"Initial Catalog=PianoHero;" +
            ////           "Persist Security Info=True;" +
            //"User ID=SA;" +
            //"Password=Backing-Crumpet4;";
            // //"TrustServerCertificate=True;";
            // using (SqlConnection cnn = new SqlConnection(connectionString))
            // {
            //     cnn.Open();
            //     Debug.WriteLine("QQQQ OK");
            //     SqlCommand cmd = new SqlCommand("SELECT * FROM PostalCodes LIMIT 25;", cnn);

            //     SqlDataReader reader = cmd.ExecuteReader();

            //     while (reader.Read())
            //         Debug.WriteLine($"{reader.GetString(1)}, {reader.GetString(2)}, {reader.GetString(3)}");

            //     Debug.WriteLine("Ok");

            //     cnn.Close();
            // }

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
>>>>>>> Stashed changes

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
