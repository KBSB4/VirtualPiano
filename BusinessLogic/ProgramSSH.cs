using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;

namespace BusinessLogic
{
    class ProgramSSH
    {
       public static void ExecuteSshConnection()
        {
            AuthenticationMethod method = new PasswordAuthenticationMethod("student", "Arena-Enclose8");
            ConnectionInfo connectionInfo = new ConnectionInfo("145.44.234.89", "student", method);
            SshClient client = new SshClient(connectionInfo);
            if (!client.IsConnected)
            {
                Debug.WriteLine("Client is not connected yet");
                client.Connect();
                client.KeepAliveInterval = TimeSpan.FromHours(2);
            }

            SshCommand readCommand = client.RunCommand("uname -mrs");
            Debug.WriteLine(readCommand.Result);
            SshCommand writeCommand = client.RunCommand("mkdir \"/home/student/Desktop/ssh_output\"");
            Thread.Sleep(1000);
            

        }
    }
}
