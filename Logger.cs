using Fleck;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableWebSocketServerCS
{
    public class Logger : Observer.Observer
    {
        public void OnOpen(int id, string name, IWebSocketConnection socket)
        {
            Console.WriteLine(DateTime.Now + ": +" + name + "#" + id);
        }

        public void OnClose(int id, string name, IWebSocketConnection socket)
        {
            Console.WriteLine(DateTime.Now + ": -" + name + "#" + id);
        }

        public void OnMessage(int id, string name, IWebSocketConnection socket, string message)
        {
            Console.WriteLine(DateTime.Now + ": <>" + name + "#" + id + " : \r\n" + JObject.Parse(message));
        }
    }
}
