# Android Morse Application with websocket

This is a simple android app, wich communicate a server and send every message to the other clients.

First of all, you should change the IP address in the:

`WebscoketServer/ConsolClient/Program.cs` and the `Morse_app/MainActivity.cs` files

```
Client client = new Client();
client.Setup("ws://192.168.0.14:8001", "basic", WebSocketVersion.Rfc6455);
client.Start();
```
IP addres can be checked at the command prompt, whith the `ipconfig` command.

![alt tag](https://s22.postimg.org/k4dr9ht8x/image.jpg)

If the IP addres is correct, You can build+run the server, and the app.

Running the consol client is not necessary, but help to track the communication.

Under Solution propertis it is possible to run only the server, then we should choose the single startup project 

![alt tag](https://s17.postimg.org/rvv9c3tq7/image.jpg)

Now we are ready to start the android app. **tapping** and realising the morse button will send short tone, and **longclicking** will send long morse signal.

The server store every messages to a predefined sqlite datebase wich can be founded beside the server (default at: *\WebsocketServer\WebsocketServer\bin\Debug\Morsedb.sqlite*
