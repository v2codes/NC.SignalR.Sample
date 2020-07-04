using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRBenchmark
{
    public class ConnectionSummary
    {
        public int TotalConnected { get; set; }
        public int TotalDisconnected { get; set; }
        public int PeakConnections { get; set; }
        public int CurrentConnections { get; set; }
        public int ReceivedCount { get; set; }
    }
}
