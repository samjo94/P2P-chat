using System;
using System.Net.Sockets;
using TDDD49.Models;

namespace TDDD49
{
    class Client
    {
        private TcpClient _client;
        private static PingInfo Connector;
        public Client(PingInfo con_client)
        {
            Connector = con_client;

            _client = Connect(con_client.ip, con_client.connectPort); 
        }
       

       public TcpClient client{
           get
           {
               return _client;
           }
       }

       public TcpClient Connect(String ip, int port)
       {
            try
            {
                TcpClient client = new TcpClient(ip, port);
                return client;
            }
            catch (ArgumentNullException e)
            {
                throw e;
            }
            catch (SocketException e)
            {
                TcpClient client = new TcpClient();
                return client;
                throw e;
            }
           
       }


    }

}

