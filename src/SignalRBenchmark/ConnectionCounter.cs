using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRBenchmark
{
    public class ConnectionCounter
    {
        private int _totalConnectedCount;
        private int _peakConnectedCount;
        private int _totalDisconnectedCount;
        private int _receiveCount;

        private readonly object _lockObj = new object();

        public ConnectionSummary Summary
        {
            get
            {
                lock (_lockObj)
                {
                    return new ConnectionSummary()
                    {
                        CurrentConnections = _totalConnectedCount - _totalDisconnectedCount,
                        PeakConnections = _peakConnectedCount,
                        TotalConnected = _totalConnectedCount,
                        TotalDisconnected = _totalDisconnectedCount,
                        ReceivedCount = _receiveCount
                    };
                }
            }
        }

        public void Receive(string a)
        {
            lock (_lockObj)
            {
                _receiveCount += a.Length;
            }
        }

        public void Connected()
        {
            lock (_lockObj)
            {
                _totalConnectedCount++;
                _peakConnectedCount = Math.Max(_totalConnectedCount - _totalDisconnectedCount, _peakConnectedCount);
            }
        }

        public void Disconnected()
        {
            lock (_lockObj)
            {
                _totalDisconnectedCount++;
            }
        }
    }
}
