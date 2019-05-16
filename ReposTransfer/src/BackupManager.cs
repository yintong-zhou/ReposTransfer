using System.IO;
using System;
using static System.Console;

namespace ReposTransfer
{
    public class BackupManager
    {
        int FilesCount;
        int failCount;
        decimal rate;

        public void OneFile(string sourceFile, string destFile)
        {
            if(File.Exists(sourceFile))
            {
                // transfer single file and overwrite it
                try
                {
                    File.Copy(sourceFile, destFile, true);
                    WriteLine("Counting objects (1/1), done.");
                    WriteLine("Transfer objects 100%, done.");
                    WriteLine("remote: Transfer complete.");
                    WriteLine("To " + destFile);
                    
                }
                catch
                {
                    WriteLine("ERROR: Transfer failure.");
                }
            }
            else WriteLine(" Source file does not exist or could not be found: "+ sourceFile);
        }

        public void FullDirectory(string sourceDir, string destDir, bool copySubDirs)
        {
            FilesCount = 0;
            failCount = 0;
            rate = 0;

            DirectoryInfo dir = new DirectoryInfo(sourceDir);

            if(!dir.Exists)
            {
                WriteLine(" Source directory does not exist or could not be found: "
                    + sourceDir
                );
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            if(!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            FileInfo[] files = dir.GetFiles();
            foreach(FileInfo file in files)
            {
                string tempPath = Path.Combine(destDir, file.Name);
                try
                {
                    file.CopyTo(tempPath, true);
                    FilesCount++;
                }
                catch
                {
                    failCount++;
                }
                
            }

            if(copySubDirs)
            {
                string tempPath;
               
                foreach (DirectoryInfo subdir in dirs)
                {
                    tempPath = Path.Combine(destDir, subdir.Name);
                    try
                    {
                        FullDirectory(subdir.FullName, tempPath, copySubDirs);
                        FilesCount++;   
                    }
                    catch
                    {
                        failCount++;
                    }
                }
            }
        }
        public void FullDirectoryStatus(string destDir)
        {
            int diff = FilesCount - failCount;
            WriteLine("Counting objects " + "(" + diff + "/" + FilesCount + ")" + ", done.");

            if(failCount < 1)
                rate = 100;
            else rate = (failCount / FilesCount) * 100;

            WriteLine("Transfer objects " + rate + "%"+ ", done.");
            WriteLine("remote: Transfer complete.");
            WriteLine("To " + destDir);
        }
    }
}