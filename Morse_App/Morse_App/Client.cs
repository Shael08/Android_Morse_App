using System;
using System.Threading;

using Android.Media;

using WebSocket4Net;


namespace Morse_App
{
    class Client
    {
        private WebSocket websocketClient;

        private string url;
        private string protocol;
        private WebSocketVersion version;

        private bool isPlaying=false;
        private int prevTone=0;
        private AudioTrack track;

        public void Setup(string url, string protocol, WebSocketVersion version)
        {
            this.url = url;
            this.protocol = protocol;
            this.version = WebSocketVersion.Rfc6455;
            websocketClient = new WebSocket(this.url, this.protocol, this.version);

          //  websocketClient.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(websocketClient_Error);
          //  websocketClient.Opened += new EventHandler(websocketClient_Opened); //play short tone on connection
            websocketClient.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocketClient_MessageReceived);
        }


        public void SendTone(string freq)
        {
            websocketClient.Send(freq);
        }

        public void Start()
        {
                websocketClient.Open();
            
        }

        public bool isConncted()
        {
       /*     DateTime date = websocketClient.LastActiveTime;
            if (date < DateTime.Now.AddSeconds(15)) return false;*/

            return websocketClient.Handshaked;
            
        }
        
        /* //play short tone on cennection
        private void websocketClient_Opened(object sender, EventArgs e)
        {
       
            websocketClient.Send("2000");
            Thread.Sleep(333);
            websocketClient.Send("2000");

        }
        */

        private void websocketClient_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (!isPlaying)
            {



                byte[] GeneratedSnd = CreateSound(int.Parse(e.Message));
                track = new AudioTrack(Stream.Music, 8000, ChannelOut.Mono, Encoding.Pcm16bit, 80000, AudioTrackMode.Static);
                prevTone = int.Parse(e.Message);
                isPlaying = !isPlaying;
                track.Write(GeneratedSnd, 0, 80000);

                try
                {
                    track.Play();
                }
                catch (Java.Lang.IllegalStateException)
                {
                    track.Flush();
                    track.Release();
                }

            }
            else if (isPlaying && prevTone != int.Parse(e.Message))
            {
                isPlaying = !isPlaying;
                track.Stop();
                track.Flush();
                track.Release();

                byte[] GeneratedSnd = CreateSound(int.Parse(e.Message));
                track = new AudioTrack(Stream.Music, 8000, ChannelOut.Mono, Encoding.Pcm16bit, 80000, AudioTrackMode.Static);
                prevTone = int.Parse(e.Message);
                isPlaying = !isPlaying;
                track.Write(GeneratedSnd, 0, 80000);

                try
                {
                    track.Play();
                }
                catch (Java.Lang.IllegalStateException)
                {
                    track.Flush();
                    track.Release();
                }

            }
            else
            {
                isPlaying = !isPlaying;
                track.Stop();
                track.Flush();
                track.Release();
            }
        }

    /*    private void websocketClient_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Console.WriteLine(e.Exception.GetType() + ": " + e.Exception.Message + System.Environment.NewLine + e.Exception.StackTrace);

            if (e.Exception.InnerException != null)
            {
                Console.WriteLine(e.Exception.InnerException.GetType());
            }

            return;
        }*/

        //generating sound
        public static byte[] CreateSound(int freq)
        {

            var duration = 10;
            var sampleRate = 8000;
            var numSamples = duration * sampleRate;
            var sample = new double[numSamples];
            var freqOfTone = freq;
            byte[] generatedSnd = new byte[2 * numSamples];
            for (int i = 0; i < numSamples; ++i)
            {
                sample[i] = Math.Sin(2 * Math.PI * i / (sampleRate / freqOfTone));
            }

            int idx = 0;
            foreach (double dVal in sample)
            {
                short val = (short)(dVal * 32767);
                generatedSnd[idx++] = (byte)(val & 0x00ff);
                generatedSnd[idx++] = (byte)((val & 0xff00) >> 8);
            }

            return generatedSnd;
        }
    }
}