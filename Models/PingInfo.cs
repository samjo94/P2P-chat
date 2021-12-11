using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using TDDD49.ViewModels;
using TDDD49.Views;

namespace TDDD49.Models
{
    class PingInfo : INotifyPropertyChanged
    {
        private Thread PingThread;
        private Client Client;
        private ChatRequestViewModel RequestViewModel;
        private ChatRequestView RequestView;
        private ChatWindowViewModel ChatViewModel;
        private ChatWindowView ChatView;
        private bool was_listening;

        public PingInfo(string ip_adress, int port, string name = null)
        {

            RequestViewModel = new ChatRequestViewModel();
            userName = name;
            ip = ip_adress;
            connectPort = port;
        }

        private Int32 _connectPort;
        public Int32 connectPort
        {
            get
            {
                return _connectPort;
            }
            set
            {
                _connectPort = value;
                OnPropertyChanged("Client Port");
            }
        }
        private string _ip;
        private string _userName;

        public string userName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                OnPropertyChanged("ClientUsername");
            }
        }

        public string ip
        {
            get
            {
                return _ip;
            }
            set
            {
                _ip = value;
                OnPropertyChanged("Client IP");
            }
        }
        #region INotifyPropertyChanged Users

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        #endregion 


        public void ErrorMsg()
        {
            MessageBox.Show("No user listening on that port.", "Error");
        }
        public void Ping(object obj)
        {
            string data = null;
            Byte[] bytes = new Byte[1024];
            Byte[] msg_out;
            ProtocolModel protocol_in;
            ProtocolModel protocol_out;

            Client = new Client(this);
            if (!Client.client.Connected)
            {
                ErrorMsg();
            }
            else
            {   
                NetworkStream stream = Client.client.GetStream();
                protocol_out = new ProtocolModel(userName, 1, new Message(userName));
            
                string json = JsonConvert.SerializeObject(protocol_out);
                msg_out = System.Text.Encoding.ASCII.GetBytes(json);
                stream.Write(msg_out, 0, msg_out.Length);
                while (true)
                {
                    data = "";
                    
                    stream = Client.client.GetStream();
                    int i;
                    i = stream.Read(bytes, 0, bytes.Length);
                    if (i != 0)
                    {
                        data += data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    }
                    if (data != "")
                    {
                        protocol_in = JsonConvert.DeserializeObject<ProtocolModel>(data);
                        if (protocol_in.Status == 1 && ChatView == null)
                        {
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            ChatView = new ChatWindowView();

                            ChatViewModel = new ChatWindowViewModel(stream, userName, protocol_in.Username);
                                ChatView.DataContext = ChatViewModel;
                                ChatView.Closing += ChatViewModel.OnWindowClosing;
                                ChatView.Show();
                            }));
                        }
                        else if (protocol_in.Status == 2)
                        {
                            MessageBox.Show("User Closed the connection", "ok");
                            protocol_out = new ProtocolModel(userName, 0, new Message(userName));
                            json = JsonConvert.SerializeObject(protocol_out);
                            msg_out = System.Text.Encoding.ASCII.GetBytes(json);
                            stream.Write(msg_out, 0, msg_out.Length);
                            stream.Close();
                            Client.client.Close();
                            break;
                        }
                        else
                        {
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                ChatViewModel.MessageReceived(protocol_in);
                            }));
                        }
                    }
                    
                }
            }
            Console.WriteLine("End of Ping, closing client");
            Client.client.Close();

        }
        
    }

}
