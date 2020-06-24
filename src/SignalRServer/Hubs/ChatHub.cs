using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Hubs
{
    public class ChatHub : Hub
    {
        static Dictionary<IClientProxy, ClientInfo> ClientList = new Dictionary<IClientProxy, ClientInfo>();
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task Register(string identity, string code)
        {
            Console.WriteLine("====> Register：identity={0}    code={0}", identity, code);
            ClientList.Add(Clients.Caller, new ClientInfo
            {
                Identity = identity,
                Code = code
            });
            await Clients.Caller.SendAsync("AcceptConnection", identity, code);
        }

        //public override Task OnConnectedAsync()
        //{
        //    //Clients
        //    Clients.Caller.SendAsync("ReceiveMessage", "Server", "Connected!");
        //    return base.OnConnectedAsync();
        //}
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine("====> OnDisconnectedAsync");
            ClientList.Remove(Clients.Caller);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
