using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using System.Text.RegularExpressions;
using Fleck;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace ObservableWebSocketServerCS
{
    public class Server : Observer.Observable
    {
        public List<Tuple<int, IWebSocketConnection>> clients = new List<Tuple<int, IWebSocketConnection>>();
        static int SocketId = 0;
        public Server(int port = 8080)
        {
            var thread = new Thread(new ThreadStart(() =>
            {
                var server = new WebSocketServer("ws://0.0.0.0:" + port);
                server.Start(socket =>
                {
                    socket.OnOpen = () =>
                    {
                        var id = Server.SocketId++;
                        this.clients.Add(new Tuple<int, IWebSocketConnection>(id, socket));
                        var data = new Object[] { id,socket };
                        this.Notify("Open", data);
                    };
                    socket.OnClose = () =>
                    {
                        var item = this.clients.Find((item) =>
                        {
                            if (item.Item2 == socket) return true;
                            return false;
                        });
                        var data = new Object[] { item.Item1, item.Item2 };
                        this.Notify("Close", data);
                    };
                    socket.OnMessage = message =>
                    {
                        var item = this.clients.Find((item) =>
                        {
                            if (item.Item2 == socket) return true;
                            return false;
                        });
                        var data = new Object[] { item.Item1, socket, message };
                        this.Notify("Message", data);
                        try
                        {
                            var json = JObject.Parse(message);
                            if(json["event"] != null && json["data"] != null)
                            {
                                var e = json["event"]!.ToString();
                                this.Notify(e, data);
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }catch(Exception e)
                        {
                            Console.WriteLine("Client service send a message using websocket which isn't in the right format. Client should only send message which respect this format : ");
                            Console.WriteLine("{\n\r\tevent:'EventName',\n\r\tdata:{\n\r\t\ta:'some data',\n\r\t\t...\n\r\t}\n\r}");
                        }
                    };
                });
                Console.Read();
            }));
            thread.Start();
        }

        public void Broadcast(string e, Object json)
        {
            this.clients.ForEach((client) =>
            {
                this.SendTo(client.Item1, e, json);
            });
        }

        public void SendTo(int id, string e, Object json)
        {
            var data = new { };
            string _json = "{}";
            JObject _data;
            try {
                _json = JObject.FromObject(json).ToString();
            }
            catch(Exception error)
            {
                Console.Error.WriteLine("The given json isn't in a json format.");
            }
            _data = JObject.FromObject(data);
            _data["event"] = e;
            _data["data"] = _json;

            var client = this.getClientFromId(id);
            client.Item2.Send(_data.ToString());
        }

        public Tuple<int, IWebSocketConnection> getClientFromId(int id)
        {
            var tuple = this.clients.Find(item =>
            {
                if (item.Item1 == id) return true;
                return false;
            });
            if (tuple == null) throw new Exception("No client with given id");
            return tuple;
        }
    }
}
