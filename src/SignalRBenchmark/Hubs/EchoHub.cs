using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRBenchmark.Hubs
{
    public class EchoHub : Hub
    {
        private ConnectionCounter _counter;

        public EchoHub(ConnectionCounter counter)
        {
            _counter = counter;
        }

        public async Task Broadcast(int duration)
        {
            var sent = 0;

            try
            {
                var t = new CancellationTokenSource();
                t.CancelAfter(TimeSpan.FromSeconds(duration));

                while (!t.IsCancellationRequested && !Context.ConnectionAborted.IsCancellationRequested)
                {
                    await Clients.All.SendAsync("send", DateTimeOffset.UtcNow);
                    sent++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("Broadcast exited：Sent {0} messages", sent);
        }

        public override Task OnConnectedAsync()
        {
            _counter.Connected();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception ex)
        {
            _counter?.Disconnected();
            return Task.CompletedTask;
        }

        public DateTimeOffset Echo(DateTimeOffset time)
        {
            return time;
        }

        public Task EchoAll(DateTime time)
        {
            return Clients.All.SendAsync("send", time);
        }

        public void SendPayload(string payload)
        {
            _counter.Receive(payload);
        }

        public DateTimeOffset GetCurrentTime()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}
