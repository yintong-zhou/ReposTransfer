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
    }
}