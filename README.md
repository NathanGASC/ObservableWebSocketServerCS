# ObservableWebSocketServerCS
This project is made to create a websocket communication between games made in C# (can be used for other stuff that's game of course) and a front-end website.

## How to use
Very simple, add all the dll which are under `.\bin\Release\net6.0\`,
aslo install some dependencies from NuGet which are : 
- Chance.Net 2.2.0
- Fleck 1.2.0
- Newtonsoft.Json 13.0.1
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
            var basic = new Basic(server); //basic is a listener which implement some basic events which can be usefull
            logger.Subscribe(server);
            basic.Subscribe(server);
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

            public void OnOpen(int id, string name, IWebSocketConnection socket)
            {
                //some stuff for your game on new user connection
                this.server.Broadcast("Open", new { }); //Trigger event Open front-end side to tell to your front-end that someone connected. You can pass some data like user id, username, ...
            }
        }
    }
}
```
Your good to go... for the back-end. Of course, you need a front-end which talk with this websocket server. For more detail about this, follow this link : TODO

## TODO
- Do front-end library which will talk with this server
- Make a tunnel between localhost & internet with ngrok
- Setup wss communication with auto generated certificate
- Find a better way to bundle dependencies
