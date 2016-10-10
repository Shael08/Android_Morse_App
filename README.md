# Android Morse Application with websocket

This is a simple android app, wich communicate a server and send every message to the other clients.

First of all, you should change the IP address in the:

`WebscoketServer/ConsolClient/Program.cs` and the `Morse_app/MainActivity.cs` files

```
Client client = new Client();
client.Setup("ws://192.168.0.14:8001", "basic", WebSocketVersion.Rfc6455);
client.Start();
```


