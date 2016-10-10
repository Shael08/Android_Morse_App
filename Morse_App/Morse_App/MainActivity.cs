using System;
using System.Threading;

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Media;

using WebSocket4Net;

namespace Morse_App
{

    [Activity(Label = "Morse_App", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            Button button = FindViewById<Button>(Resource.Id.button1);
            button.Text = ("Morse");

            //Client connectiong to the server
            Client client = new Client();
            client.Setup("ws://192.168.0.14:8001", "basic", WebSocketVersion.Rfc6455);
            client.Start();

            Random rnd = new Random();

            int freq = rnd.Next(100, 2600);
            byte[] GeneratedSnd = Client.CreateSound(freq);

            button.Click += delegate
            {
                //if handshaking does not happend, trying 
                if (!client.isConncted())
                {
                    client.Setup("ws://192.168.0.14:8001", "basic", WebSocketVersion.Rfc6455);
                    client.Start();
                }
                client.SendTone(freq.ToString());

                //playing short tone
                try
                {
                    AudioTrack track = new AudioTrack(Stream.Music, 8000, ChannelOut.Mono, Encoding.Pcm16bit, 8000, AudioTrackMode.Static);
                    track.Write(GeneratedSnd, 0, (8000 / 3));
                    track.Play();
                    Thread.Sleep(333);
                    client.SendTone(freq.ToString());
                    track.Flush();
                    track.Release();
                    //withou flush and realse only 32 track play possible
                }
                catch (Java.Lang.IllegalStateException)
                {
                    client.SendTone(freq.ToString());
                }
            };


            button.LongClick += delegate
            {

                if (!client.isConncted())
                {
                    client.Setup("ws://192.168.0.14:8001", "basic", WebSocketVersion.Rfc6455);
                    client.Start();
                }

                client.SendTone(freq.ToString());

                //playing long tone
                try
                {
                    AudioTrack track = new AudioTrack(Stream.Music, 8000, ChannelOut.Mono, Encoding.Pcm16bit, 8000, AudioTrackMode.Static);
                    track.Write(GeneratedSnd, 0, 8000);
                    track.Play();
                    Thread.Sleep(1000);
                    client.SendTone(freq.ToString());
                    track.Flush();
                    track.Release();

                }
                catch (Java.Lang.IllegalStateException)
                {
                    client.SendTone(freq.ToString());
                }

            };

        }

    }
}

