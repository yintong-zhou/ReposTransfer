using System;
using static System.Console;
using System.IO;
using System.Net;

namespace ReposTransfer
{
    class Program
    {
        #region Global Variables
        static CoverStrings _cover = new CoverStrings();
        static BackupManager _backup = new BackupManager();
        static CMDs _cmds = new CMDs();
        static StatusChecker _checker = new StatusChecker();
        static string newL = Environment.NewLine;
        static string netDir, source, dest, cmd;
        static string user, pwd;
        #endregion
        static void Main(string[] args)
        {
            #region Init Connection
            NetworkCredential netCredential;
            NetworkConnection netConn;
            string currentDir = Directory.GetCurrentDirectory();
            WriteLine("Welcome to REPOS NO GIT v0.1" + newL);

            Write(currentDir + ">");
            netDir = @"init \\192.168.1.242\homes\yzhou";

            if (netDir.StartsWith(_cmds.Init()))
            {
                string[] initConn = netDir.Split(' ');
                netDir = initConn[1];

                Write(newL + "Username: ");
                user = ReadLine();
                Write("Password: ");
                pwd = _cover.ReadPassword();

                netCredential = new NetworkCredential(user, pwd);
                try
                {
                    netConn = new NetworkConnection(netDir, netCredential);
                    WriteLine(newL + "Connect to " + netDir);
                }
                catch (Exception ex)
                {
                    WriteLine("ERROR: Connection failure.");
                    WriteLine(ex.StackTrace);
                }
            }
            #endregion

            Write(newL + Directory.GetCurrentDirectory() + ">");
            source = ReadLine();
            if (source.StartsWith(_cmds.AddOne()))
            {
                string[] initAdd;
                if (source.Contains(_cmds.AddAll()))
                {
                    source = Directory.GetCurrentDirectory();
                    string[] dirName = source.Split('\\');
                    int i = dirName.Length;
                    i--;
                    dest = Path.Combine(netDir + "\\", dirName[i]);

                CMDall:
                    ResetColor();
                    Write(newL + Directory.GetCurrentDirectory() + ">");
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
                        WriteLine("    ERROR: try command 'add all'");
                        goto CMDall;
                    }
                }
                else
                {
                    initAdd = source.Split(' ');
                    source = initAdd[1];
                    source = Directory.GetCurrentDirectory() + "\\" + source;
                    string fileName = Path.GetFileName(source);
                    dest = Path.Combine(netDir + "\\", fileName);

                CMDone:
                    ResetColor();
                    Write(newL + Directory.GetCurrentDirectory() + ">");
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
                        WriteLine("    ERROR: try command 'add file name.extension'");
                        goto CMDone;
                    }
                }
            }


            ReadKey();
        }
    }
}
