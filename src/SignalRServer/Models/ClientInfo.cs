using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Models
{
    public class ClientSummary
    {
        public string ConnectionId { get; set; }
        public string Identity { get; set; }
        public string Code { get; set; }
        public string Data { get; set; }
    }
}
