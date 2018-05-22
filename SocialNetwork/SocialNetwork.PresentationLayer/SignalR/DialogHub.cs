using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading;

namespace SocialNetwork.PresentationLayer.SignalR
{
    public class DialogHub : Hub
    {
        static List<UserSignalR> Users = new List<UserSignalR>();

        public void Send(string dialogID)
        {
            Thread.Sleep(750);
            Clients.Others.addMessage(dialogID);
        }

        public void Connect()
        {
            var id = Context.ConnectionId;

            if (!Users.Any(x => x.ConnectionId == id))
            {
                Users.Add(new UserSignalR { ConnectionId = id });
            }
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                Users.Remove(item);
                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}