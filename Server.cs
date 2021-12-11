

namespace TDDD49
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using TDDD49.Models;

    public class Server
    {
        public TcpListener _server;
        private static ListenInfo listener;
        private bool _active = false;
        public bool active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
            }
        }

        public Server(ListenInfo con_listener)
        {
            listener = con_listener;

            _server = InitServer(con_listener.listenPort);
        }

        public TcpListener server{
            get{
                return _server;
            }
        }
        private static TcpListener InitServer(Int32 port)
        {
            TcpListener server = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, port);
                return server;
                
            }
            catch (SocketException e)
            {
                throw e;
            }
        }

        public void stop()
        {
            server.Stop();
            active = false;

        }

        public void start()
        {
            server.Start();
            active = true;

        }
        public bool is_listening()
        {
            return active;
        }
    }
}