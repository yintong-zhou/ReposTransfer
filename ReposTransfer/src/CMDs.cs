using System;
using System.Collections.Generic;

namespace ReposTransfer
{
    public class Commands
    {
        public string Init {get; set;}
        public string AddOne {get; set;}
        public string AddAll {get; set;}
        public string Push {get; set;}
        public string Status {get; set;}
        
    }
    
    public class CMDs
    {
        Commands cmd = new Commands();
        public string Init()
        {
            cmd.Init = "init";
            var init = cmd.Init;
            return init;
        }
        public string AddOne()
        {
            cmd.AddOne = "add";
            var addOne = cmd.AddOne;
            return addOne;
        }
        public string AddAll()
        {
            cmd.AddAll = "add all";
            var addAll = cmd.AddAll;
            return addAll;
        }
        public string Push()
        {
            cmd.Push = "push";
            var push = cmd.Push;
            return push;
        }
        public string Status()
        {
            cmd.Status = "status";
            var status = cmd.Status;
            return status;
        }
    }
}