using Fleck;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableWebSocketServerCS
{
    public class Basic: Observer.Observer
    {
        Server server;  
        public Basic(Server server)
        {
            this.server = server;
        }

        public void OnHelp(int id, IWebSocketConnection socket, string message)
        {
            var json = new
            {
                code = 200,
                statusMessage = "OK",
                id = "@"+id,
                message = "Here some information to let you know who you are for the server and some basics information about us.",
                credits = new
                {
                    name = "NathanGASC",
                    github = "https://github.com/NathanGASC",
                }
            };
            socket.Send(JObject.FromObject(json).ToString());
        }
    }
}
