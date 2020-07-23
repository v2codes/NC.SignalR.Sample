using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SignalRServer.Models
{
    /// <summary>
    /// 交互命令描述
    /// @Created by Leo 2020/7/13 18:48:28
    /// </summary>
    public class CommandSummary
    {
        public DateTimeOffset Time { get; set; }
        public string Command { get; set; }
        public object Data { get; set; }

        public CommandSummary(string command, object data)
        {
            Time = DateTimeOffset.Now;
            this.Command = command;
            this.Data = data;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
