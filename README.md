# ObservableWebSocketServerCS
This project is made to create a websocket communication between games made in C# (can be used for other stuff that's game of course) and a front-end website.

## How to use
Add all the dll which are under `.\bin\Release\net6.0\`
and then, try this code which is commented to help:

```cs
using Fleck;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Text;
using wsServer;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            //By default, server will be on port 8080 if no port is given.
            var server = new Server(8080);

            //Setup game api
            var gameApi = new GameApi(server);
            gameApi.Subscribe(server);

            //We set some listener to ur server. Those are custom events listener which will be helpfull.
            var logger = new Logger(); //logger is a listener made to log server status (new users, disconnected users, messages)
            logger.Subscribe(server);
        }

        /**
         * Your Game API, your rules
         * You can add some event listener like "OnAttack", "OnMove", "OnBuy", ... , and trigger them from front-end side. REMINDER: Be very carerfull! User entries are never safe (check them before using them)
         * You can also trigger events from back-end (from the game) and listen to them in the front-end. Use server.Broadcast or server.SendTo to trigger events with data
         */
        class GameApi : Observer.Observer
        {
            private Server server;

            public GameApi(Server server)
            {
                this.server = server;
            }

            public void OnOpen(int id, IWebSocketConnection socket)
            {
                //some stuff for your game on new user connection
                this.server.Broadcast("Open", new { }); //Trigger event Open front-end side to tell to your front-end that someone connected. You can pass some data like user id, username, ...
            }

            public void OnHello(int id, IWebSocketConnection socket, string message)
            {
                Console.log("Hello from " + id)
            }
        }
    }
}
```

If you want to run this server on wss, or on the internet, i've made a proxy server which is an executable: https://github.com/NathanGASC/WebsocketProxy.

Your good to go... for the back-end. Of course, you need a front-end which talk with this websocket server. For more detail about this, follow this link : TODO
