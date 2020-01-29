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

            while (true)
            {
                using (Process p = new Process())
                {
                    Console.Write(WorkingDirectory + "\b>");
                    var cmd = Console.ReadLine();

                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.Arguments = @"/c " + cmd;
                    p.StartInfo.WorkingDirectory = WorkingDirectory;

                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardInput = true;
                    
                    p.OutputDataReceived += (a, b) => Console.WriteLine(b.Data);
                    p.ErrorDataReceived += (a, b) => Console.WriteLine(b.Data);
                    
                    p.Start();
                    
                    p.BeginErrorReadLine();
                    p.BeginOutputReadLine();

                    p.WaitForExit();

                    if (cmd.StartsWith("cd"))
                        WorkingDirectory = Path.GetFullPath(Path.Combine(WorkingDirectory, cmd.Remove(0, 2).Trim().Trim('\"', '\'') + @"\"));
                }
            }
        }
    }
}
