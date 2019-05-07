using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ReposTransfer
{
    
    public class ReposInfo
    {
        Cryptography aes = new Cryptography();

        public void CreateInfoFile(string sourceDir, string contents)
        {
            string aesContents = aes.EncryptText(contents);
            string filePath = sourceDir + @"\.reposinfo.txt";
            File.AppendAllText(filePath, aesContents);
        }

        public string ReadInfoFile(string sourceDir)
        {
            string filePath = sourceDir + @"\.reposinfo.txt";
            var aesContents = File.ReadAllLines(filePath);
            string Contents = aes.DecryptText(aesContents[0].ToString());

            return Contents;
        }

        public bool InfoFileFinder(string sourceDir)
        {
            string filePath = sourceDir + @"/.reposinfo.txt";
            bool exist = File.Exists(filePath);
            return exist; 
        }
    }
}
