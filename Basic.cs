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

        public void OnNameChange(int id, string name, IWebSocketConnection socket, string message)
        {
            var json = JObject.Parse(message);
            var pseudo = json["data"]?["name"]?.ToString();
            if (pseudo == null)
            {
                var resError = new
                {
                    code = 403,
                    statusMessage = "Forbidden",
                    message = "Given data don't contain name field"
                };
                socket.Send(JObject.FromObject(resError).ToString());
                return;
            }

            var index = server.clients.FindIndex((item) =>
            {
                if (item.Item3 == socket) return true;
                return false;
            });

            server.clients[index] = new Tuple<int, string, IWebSocketConnection>(id, pseudo, socket);

            var res = new
            {
                code = 200,
                statusMessage = "OK",
                message = "Rename successfully as " + pseudo + "#" + id
            };
            socket.Send(JObject.FromObject(res).ToString());
        }

        public void OnHelp(int id, string name, IWebSocketConnection socket, string message)
        {
            var json = new
            {
                code = 200,
                statusMessage = "OK",
                id = id,
                name = name,
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
