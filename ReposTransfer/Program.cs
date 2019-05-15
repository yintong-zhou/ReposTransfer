using System;
using static System.Console;
using System.IO;
using System.Net;

namespace ReposTransfer
{
    class Program
    {
        static ReposInfo repos = new ReposInfo();
        static CoverPwd _cover = new CoverPwd();
        static BackupManager _backup = new BackupManager();
        static CMDs _cmds = new CMDs();
        static StatusChecker _checker = new StatusChecker();
        static NetworkCredential netCredential;
        static NetworkConnection netConn;

        static string newL = Environment.NewLine;
        static string netDir, source, dest, cmd, input;
        static string user, pwd;

        static void Main(string[] args)
        {
            WriteLine("Welcome to REPOS TRANSFER v0.1" + newL);
            Write("Source Directory: ");
            string sourceDir = ReadLine();

            bool reposEx = repos.InfoFileFinder(sourceDir);
            if (reposEx)
            {
                repos.ReadInfoFile(sourceDir);
            }

            InputCMD:
            Write(newL + sourceDir + ">");
            input = ReadLine(); // Server repos path

            #region Init Connection
            if (input.StartsWith(_cmds.Init()))
            {
                Write(newL + "Username: ");
                user = ReadLine();
                Write("Password: ");
                pwd = _cover.ReadPassword();

                var remote = input.Split(' ');
                var netAuth = remote[1] + ";" + user + ";" + pwd;

                if (!reposEx)
                {
                    repos.CreateInfoFile(sourceDir, netAuth);
                }
                else
                {
                    repos.CreateInfoFile(sourceDir, netAuth);
                    WriteLine("Reinitialized remote {0}", remote[1]);
                }

                netCredential = new NetworkCredential(user, pwd);
                try
                {
                    netConn = new NetworkConnection(remote[1], netCredential);
                    WriteLine(newL + "Connect to " + remote[1]);
                    goto InputCMD;
                }
                catch (Exception ex)
                {
                    WriteLine(">>ERROR: Connection failure.");
                    WriteLine(ex.Message.ToString());
                }
            }
            #endregion

            if (input.StartsWith(_cmds.AddOne()))
            {
                source = input;
                var netStr = repos.ReadInfoFile(sourceDir);
                var netSplit = netStr.Split(';');
                netDir = netSplit[0].ToString();

                string[] initAdd;
                if (source.Contains(_cmds.AddAll()))
                {
                    source = sourceDir;
                    string[] dirName = source.Split('\\');
                    int i = dirName.Length;
                    i--;
                    dest = Path.Combine(netDir + "\\", dirName[i]);

                CMDall:
                    ResetColor();
                    Write(newL + sourceDir + ">");
                    cmd = ReadLine();
                    if (cmd.StartsWith(_cmds.Push()))
                    {
                        _backup.FullDirectory(source, dest, true);
                        _backup.FullDirectoryStatus(dest);
                    }
                    else if (cmd.StartsWith(_cmds.Status()))
                    {
                        _checker.Status(cmd, source);
                        goto CMDall;
                    }
                    else
                    {
                        WriteLine(">>ERROR: try command 'add all'");
                        goto CMDall;
                    }
                }
                else
                {
                    initAdd = source.Split(' ');
                    source = sourceDir + "\\" + initAdd[1];

                    string[] dirName = source.Split('\\');
                    int i = dirName.Length;
                    i--;

                    string fileName = Path.GetFileName(source);
                    dest = Path.Combine(netDir + "\\", dirName[1] + "\\" + fileName);

                CMDone:
                    ResetColor();
                    Write(newL + sourceDir + ">");
                    cmd = ReadLine();
                    if (cmd.StartsWith(_cmds.Push()))
                    {
                        _backup.OneFile(source, dest);
                    }
                    else if (cmd.StartsWith(_cmds.Status()))
                    {
                        _checker.Status(cmd, source);
                        goto CMDone;
                    }
                    else
                    {
                        WriteLine(">>ERROR: try command 'add file name.extension'");
                        goto CMDone;
                    }
                }
            }

            goto InputCMD;

            ReadKey();
        }
    }
}
