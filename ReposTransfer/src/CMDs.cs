using System;
using System.Collections.Generic;

namespace ReposTransfer
{
    public class CMDs
    {
        public string Start() => "rtn"; // ReposTransfer start comand
        public string Init() =>  "init"; // init connection with host
        public string AddOne() => "add"; // select a single file
        public string AddAll() => "add-all"; // select all directory
        public string Push() => "push"; // backup local project to host
        public string Status() => "status"; // show local project status
        public string Pull() => "pull"; // restore local project from another host
        public string Help() => "help"; // show comands

        public string HelpInfo() // show all comand
        {
            string comands = string.Empty;
            string nl = Environment.NewLine;

            comands = $"usage: rtn [--version] [--help] <commands>{nl}" +
                $"These are common ReposTranfer used in various situantions:{nl}" +
                $"{nl}" +
                $"  {Start()}        {""}{nl}" +
                $"  {Init()}         {""}{nl}" +
                $"  {AddOne()}       {""}{nl}" +
                $"  {AddAll()}       {""}{nl}" +
                $"  {Push()}         {""}{nl}" +
                $"  {Status()}       {""}{nl}" +
                $"  {Pull()}         {""}{nl}" +
                $"{nl}" +
                $"'rtn help' list available commands." ;

            return comands;
        }
    }
}