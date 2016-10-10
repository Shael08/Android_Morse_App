using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperWebSocket;
using SuperSocket.SocketBase;
using System.Net;
using System.Net.Sockets;
using System.Data.SQLite;
using System.IO;

namespace WebsocketServer
{
    class Server
    {
        private WebSocketServer appServer;
        private SQLiteConnection m_dbConnection;
        private bool isPlaying=false;
        private string tone;
        private DateTime start;

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        public void Setup()
        {

            appServer = new WebSocketServer();

            if (!appServer.Setup(8001)) //Setup with listening port
            {
                Console.WriteLine("Failed to setup!");
                Console.ReadKey();

                return;
            }

            appServer.NewSessionConnected += new SessionHandler<WebSocketSession>(appServer_NewSessionConnected);
            appServer.SessionClosed += new SessionHandler<WebSocketSession, CloseReason>(appServer_SessionClosed);
            appServer.NewMessageReceived += new SessionHandler<WebSocketSession, string>(appServer_NewMessageReceived);

            Console.WriteLine();
        }

        public void Start()
        {
            if (!appServer.Start())
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Server running at: " + GetLocalIPAddress() + " on port: 8001");
            Console.WriteLine("The server started successfully! Press any key to see application options.");


            if (File.Exists("Morsedb.sqlite.db"))
            {
                SQLiteConnection.CreateFile("Morsedb.sqlite");
            }

            m_dbConnection = new SQLiteConnection("Data Source=Morsedb.sqlite;Version=3;");
            m_dbConnection.Open();

            string sql = "CREATE TABLE if not exists morselog (tone varchar(20), start datetime, end datetime)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();



            Console.ReadKey();

            ShowAvailableOptions();

            char keyStroked;

            while (true)
            {
                keyStroked = Console.ReadKey().KeyChar;

                if (keyStroked.Equals('q'))
                {
                    Stop();
                    return;
                }

                if (keyStroked.Equals('s'))
                {
                    Console.WriteLine();
                    Console.WriteLine("Put here your message to clients: ");

                    string message = Console.ReadLine();

                    foreach (WebSocketSession session in appServer.GetAllSessions())
                    {
                        session.Send(message);
                    }
                }

                ShowAvailableOptions();
                continue;
            }
        }

        public void Stop()
        {
            m_dbConnection.Close();
            appServer.Stop();

            Console.WriteLine();
            Console.WriteLine("The server was stopped!");
        }

        public void ShowAvailableOptions()
        {
            Console.WriteLine();
            Console.WriteLine("Available options: ");
            Console.WriteLine("Press 'q' key to stop the server.");
            Console.WriteLine("Press 's' key to send message to client.");
        }

        private void appServer_NewMessageReceived(WebSocketSession session, string message)
        {
            Console.WriteLine(message);
            Console.WriteLine(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff"));

            if (!isPlaying)
            {
                isPlaying = !isPlaying;
                tone = message;
                start = DateTime.Now;

            }
            else if(isPlaying && tone != message)
            {
                isPlaying = !isPlaying;
                string sql = "insert into morselog (tone, start, end) values ($tone, $start, $end) ";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.Parameters.AddWithValue("$tone", tone);
                command.Parameters.AddWithValue("$start", start);
                command.Parameters.AddWithValue("$end", DateTime.Now);
                command.ExecuteNonQuery();

                tone = message;
                start = DateTime.Now;
            }
            else
            {
                isPlaying = !isPlaying;
                string sql = "insert into morselog (tone, start, end) values ($tone, $start, $end) ";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.Parameters.AddWithValue("$tone", tone);
                command.Parameters.AddWithValue("$start", start);
                command.Parameters.AddWithValue("$end", DateTime.Now);
                command.ExecuteNonQuery();

            }
            foreach (WebSocketSession s in appServer.GetAllSessions())
            {
                if (s != session) s.Send(message);
            }
        }

        private void appServer_NewSessionConnected(WebSocketSession session)
        {
            Console.WriteLine();
            Console.WriteLine("New session connected! Sessions counter: " + appServer.SessionCount);
        }

        private void appServer_SessionClosed(WebSocketSession session, CloseReason value)
        {
            Console.WriteLine();
            Console.WriteLine("Client disconnected! Sessions counter: " + appServer.SessionCount);
        }




    }
}