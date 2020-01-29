using System;
using System.Diagnostics;
using System.IO;

namespace Cmd_Wrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            string WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
            Process p = new Process();

            p.StartInfo.FileName = "cmd.exe";

            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = true;

            p.OutputDataReceived += (a, b) => Console.WriteLine(b.Data);
            p.ErrorDataReceived += (a, b) => Console.WriteLine(b.Data);

            while (true)
            {
                Console.Write(WorkingDirectory + "\b>");
                var cmd = Console.ReadLine();

                p.StartInfo.Arguments = @"/c " + cmd;
                p.StartInfo.WorkingDirectory = WorkingDirectory;
                
                if (cmd.StartsWith("cd"))
                    WorkingDirectory = Path.GetFullPath(Path.Combine(WorkingDirectory, cmd.Remove(0, 2).Trim().Trim('\"', '\'') + @"\"));

                try
                {
                    p.Start();
                }
                catch (Exception)
                {
                    WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
                }

                p.BeginErrorReadLine();
                p.BeginOutputReadLine();

                p.WaitForExit();

                p.CancelErrorRead();
                p.CancelOutputRead();
            }
        }
    }
}
