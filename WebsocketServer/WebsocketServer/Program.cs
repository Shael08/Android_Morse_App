﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server myServer = new Server();

            myServer.Setup();
            myServer.Start();
        }
    }
}
