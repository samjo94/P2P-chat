


namespace TDDD49.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Net.Sockets;
    using System.Threading;
    using System.Windows;
    using TDDD49.ViewModels;
    using TDDD49.Views;

    public class ListenInfo : INotifyPropertyChanged
    {
        private ChatRequestViewModel RequestViewModel;
        private ChatRequestView RequestView;
        private ChatWindowViewModel ChatViewModel;
        private ChatWindowView ChatView;
        private Thread ListenThread;
        private Server Server;
        private string _DisplayString;
        public string DisplayString
        {
            get
            {
                return _DisplayString;
            }
            set
            {
                _DisplayString = value;
                OnPropertyChanged("DisplayString");
            }
        }
        public ListenInfo(String name, int port)
        {
            DisplayString = "Start Listening";
            RequestViewModel = new ChatRequestViewModel();
            userName = name;
            listenPort = port;
        }

        private Int32 _listenPort;
        public Int32 listenPort
        {
            get
            {
                return _listenPort;
            }
            set
            {
                _listenPort = value;
                OnPropertyChanged("Listen Port");
            }
        }
        private String _userName;
        public String userName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                OnPropertyChanged("Username");
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


        

        [STAThread]
        public void Listen(object obj)
        {
            DisplayString = "Stop Listening";
            Server = new Server(this);
            Byte[] bytes = new Byte[1024];
            String data = null;
            Byte[] msg;
            Byte[] msg_out;
            ProtocolModel protocol_in;
            ProtocolModel protocol_out;
            Server.start();
            while (Server.active) 
            {
                TcpClient client = Server.server.AcceptTcpClient();
                
                NetworkStream stream = client.GetStream();
                int i;
                i = stream.Read(bytes, 0, bytes.Length);
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                protocol_in = JsonConvert.DeserializeObject<ProtocolModel>(data);
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    RequestView = new ChatRequestView();
                    RequestView.DataContext = RequestViewModel;
                    RequestViewModel.Request.User = protocol_in.Username; 
                    RequestView.Show();
                }));
                bool Confirm = false;
                while (RequestViewModel.Run)
                {
                    if (RequestViewModel.Accepted == true)
                    {
                        Confirm = true;
                        break;
                    }
                    else if (RequestViewModel.Declined == true)
                    {
                        
                        break;
                    }
                }
                RequestViewModel.Declined = false;
                RequestViewModel.Accepted = false;

                if (Confirm) 
                {
                    protocol_out = new ProtocolModel(userName, 1, new Message(userName));
                    string json = JsonConvert.SerializeObject(protocol_out);
                    msg_out = System.Text.Encoding.ASCII.GetBytes(json);
                    stream.Write(msg_out, 0, msg_out.Length);
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        RequestView.Close();
                 
                        ChatView = new ChatWindowView();
                        ChatViewModel = new ChatWindowViewModel(stream, userName, protocol_in.Username);
                        ChatView.DataContext = ChatViewModel;
                        ChatView.Closing += ChatViewModel.OnWindowClosing;
                        ChatView.Show();
                    }));
    
                }
                else
                {

                    protocol_out = new ProtocolModel(userName, 2, new Message(userName));
                    string json = JsonConvert.SerializeObject(protocol_out);
                    msg_out = System.Text.Encoding.ASCII.GetBytes(json);
                    stream.Write(msg_out, 0, msg_out.Length);
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        RequestView.Close();
                    }));

                }
                
                while (Confirm)
                {
                    data = "";
                    
                    stream = client.GetStream();
                    try
                    {
                        i = stream.Read(bytes, 0, bytes.Length);
                    }
                    catch(IOException error)
                    {
                        MessageBox.Show("User Closed the connection", "ok");
                    }
                    
                    if(i != 0)
                    {
                        data += System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                    }
                    if(data != ""){
                        protocol_in = JsonConvert.DeserializeObject<ProtocolModel>(data);

                        if (protocol_in.Status == 0)
                        {
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
                stream.Close();
                client.Close();

            }
            
            Server = null;
        }

        public void Close()
        {
            Server.stop();
        }
    }
}
