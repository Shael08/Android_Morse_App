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

If the IP addres is correct, You can build+run the server, and the app.

Running the consol client is not necessary, but help to track the communication.

Under Solution propertis it is possible to run only the server

![alt tag](https://s17.postimg.org/rvv9c3tq7/image.jpg)
