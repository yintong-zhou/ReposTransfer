using System;
using static System.Console;
using System.IO;
using System.Net;

namespace ReposTransfer
{
    class Program
    {
        #region Variables
        static ReposInfo repos = new ReposInfo();
        static KeysGenerator generator = new KeysGenerator();
        static CoverPwd _cover = new CoverPwd();
        static BackupManager _backup = new BackupManager();
        static CMDs _cmds = new CMDs();
        static StatusChecker _checker = new StatusChecker();
        static NetworkCredential netCredential;
        static NetworkConnection netConn;

        static string newL = Environment.NewLine;
        static string netDir, source, dest, cmd, input;
        static string user, pwd;
        #endregion

        static void Main(string[] args)
        {
            #region Init Terminal
            WriteLine("Welcome to REPOS TRANSFER v0.9.0" + newL);
            Write("Source Directory: ");

            string sourceDir = ReadLine();
            bool reposEx = repos.InfoFileFinder(sourceDir);
            if (reposEx)
            {
                var netStr = repos.ReadInfoFile(sourceDir);
                var netSplit = netStr.Split(';');
                netDir = netSplit[0].ToString();
                var user = netSplit[1].ToString();
                var pwd = netSplit[2].ToString();

                try
                {
                    netCredential = new NetworkCredential(user, pwd);
                    netConn = new NetworkConnection(netDir, netCredential);
                    WriteLine("Connected to " + netDir);
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message.ToString());
                }
            }
            else
            { 
                WriteLine("Write " + "init \\\\0.0.0.0\\dir\\subdir".ToUpper() + " for connect to the Server");
            }
        #endregion

        InputCMD:
            Write(newL + sourceDir + ">");
            input = ReadLine(); 

            #region Init Connection
            if (input.StartsWith(_cmds.Init()))
            {
                Write(newL + "Username: ");
                user = ReadLine();
                Write("Password: ");
                pwd = _cover.ReadPassword();

                var code = generator.HashCode(64);
                var remote = input.Split(' ');
                var netAuth = remote[1] + ";" + user + ";" + pwd + ";" + code;

                repos.CreateInfoFile(sourceDir, netAuth);
                WriteLine("Initialized remote {0}", remote[1]);

                netCredential = new NetworkCredential(user, pwd);
                try
                {
                    netConn = new NetworkConnection(remote[1], netCredential);
                    WriteLine(newL + "Connect to " + remote[1]);
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
                string[] initAdd;
                
                if (source.Contains(_cmds.AddAll()))
                {
                    #region Add All to Push
                    source = sourceDir;
                    string[] dirName = source.Split('\\');
                    int i = dirName.Length;
                    i--;
                    dest = Path.Combine(netDir + "\\", dirName[i]);

                CMDall:
                    ResetColor();
                    Write(newL + sourceDir + ">");
                    cmd = ReadLine();

                    try
                    {
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
                    catch (Exception ex)
                    {
                        WriteLine(ex.Message.ToString());
                    }
                    #endregion
                }
                else
                {
                    #region Add One to Push
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

                    try
                    {
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
                    catch(Exception ex)
                    {
                        WriteLine(ex.Message.ToString());
                    }
                    #endregion
                }
            }

            if (input.StartsWith(_cmds.Pull()))
            {
                try
                {
                    string localRepos = repos.ReadInfoFile(sourceDir);
                    var localInfo = localRepos.Split(';');
                    var localKey = localInfo[3];

                    var subDir = Path.GetFileName(sourceDir);
                    var remoteDir = localInfo[0] + "\\" + subDir;

                    string remoteRepos = repos.ReadInfoFile(remoteDir);
                    var remoteInfo = remoteRepos.Split(';');
                    var remoteKey = remoteInfo[3];

                    if (localKey != remoteKey)
                    {
                        WriteLine(">>ERROR: ID Key is corrupted.");
                    }
                    else
                    {
                        _backup.FullDirectory(remoteDir, sourceDir, true); 
                        _backup.FullDirectoryStatus(sourceDir);
                    }
                }
                catch(Exception ex)
                {
                    WriteLine(ex.Message.ToString());
                }
            }

            goto InputCMD;
        }
    }
}
