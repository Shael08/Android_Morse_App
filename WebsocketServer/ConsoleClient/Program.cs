using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket4Net;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            client.Setup("ws://192.168.0.14:8001", "basic", WebSocketVersion.Rfc6455);
            client.Start();
        }
    }
}
