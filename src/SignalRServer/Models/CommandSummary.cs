using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Models
{
    /// <summary>
    /// 交互命令描述
    /// </summary>
    public class CommandSummary
    {
        public DateTime Time { get; set; }
        public string Command { get; set; }
        public object Data { get; set; }

        public CommandSummary(string command,object data)
        {
            Time = DateTime.Now;
            this.Command = command;
            this.Data = data;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
