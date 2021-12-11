using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TDDD49.DataHandling;

namespace TDDD49.Models
{
    class ChatModel : INotifyPropertyChanged
    {
        
        private string _TextBox;
        private string _Username;
        private string _RemoteUser;
        private ObservableCollection<Message> _Messages;
        private NetworkStream _Stream;
        private DataHandler _DataHandler;
        private bool _ConnectionClosed;
        
        public ChatModel(string username, string second_user, NetworkStream stream)
        {
            _DataHandler = new DataHandler();
            _DataHandler.NewChat(second_user);
            _RemoteUser = second_user;
            _Stream = stream;
            _Username = username;
            _Messages = new ObservableCollection<Message>();
            _TextBox = "";
            _ConnectionClosed = false;

        }
        public DataHandler DataHandler
        {
            get
            {
                return _DataHandler;
            }
            set
            {
                _DataHandler = value;
            }
        }
        public string RemoteUser
        {
            get
            {
                return _RemoteUser;
            }
            set
            {
                _RemoteUser = value;
            }
        }
        public NetworkStream Stream
        {
            get
            {
                return _Stream;
            }
            set
            {
                _Stream = value;
            }
        }

        public string Username
        {
            get
            {
                return _Username;
            }
        }

        public string TextBox
        {
            get
            {
                return _TextBox;
            }
            set
            {
                _TextBox = value;
                OnPropertyChanged("TextBox");
            }
        }

        public ObservableCollection<Message> Messages
        {
            get
            {
                return _Messages;
            }
            set
            {
                _Messages = value;
                OnPropertyChanged("Messages");
            }
        }
        public void Send()
        {
            Byte[] bytes = new Byte[256];
            Byte[] msg;
            string temp = TextBox;
            
            TextBox = "";
            Message MsgToSend = new Message(Username, temp);

            AddMessage(MsgToSend);
            ProtocolModel protocol_out = new ProtocolModel(Username, 1, MsgToSend);
            
            string json = JsonConvert.SerializeObject(protocol_out);

            msg = System.Text.Encoding.ASCII.GetBytes(json);
            Stream.Write(msg, 0, msg.Length);
        }

        public void Buzz()
        {
            Byte[] bytes = new Byte[256];
            Byte[] msg;
            ProtocolModel protocol_out = new ProtocolModel(Username, 4, new Message(Username));

            string json = JsonConvert.SerializeObject(protocol_out);
            msg = System.Text.Encoding.ASCII.GetBytes(json);

            Stream.Write(msg, 0, msg.Length);
        }

        public void MessageReceived(ProtocolModel protocol_in)
        {
            if (protocol_in.Status == 4)
            {
                System.Media.SystemSounds.Asterisk.Play();
            }
            else if (protocol_in.Status == 2)
            {
                _ConnectionClosed = true;
                MessageBox.Show("User ended the connection", "Exit");
            }
            else
            {
                
                AddMessage(protocol_in.Message);
            }
        }

        public void AddMessage(Message message)
        {
            DataHandler.AddMessage(message);
            Messages.Add(message);
        }

        public void EndChat()
        {
            if (!_ConnectionClosed)
            {
                Byte[] bytes = new Byte[256];
                Byte[] msg;
                Message MsgToSend = new Message(Username);
                ProtocolModel protocol_out = new ProtocolModel(Username, 2, MsgToSend);

                string json = JsonConvert.SerializeObject(protocol_out);

                msg = System.Text.Encoding.ASCII.GetBytes(json);
                Stream.Write(msg, 0, msg.Length);
                Stream.Close();
            }
            DataHandler.SaveChat();
        }

        #region INotifyPropertyChanged Chat

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
    }
}
