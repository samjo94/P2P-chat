using System;
using TDDD49.Commands;
using TDDD49.Models;
using System.Net.Sockets;
using System.ComponentModel;

namespace TDDD49.ViewModels
{
    class ChatWindowViewModel
    {
        private NetworkStream stream;
        private ChatModel _Chat;
        private string _CurrentUser;
        
        public string CurrentUser
        {
            get
            {
                return _CurrentUser;
            }
            set
            {
                _CurrentUser = value;
            }
        }

        public RelayCommand BuzzCommand
        {
            get;
            private set;
        }

        public RelayCommand SendCommand
        {
            get;
            private set;
        }

        public ChatWindowViewModel(NetworkStream incoming_stream, string current_user, string username)
        {
            stream = incoming_stream;
            CurrentUser = current_user;
            _Chat = new ChatModel(current_user, username, incoming_stream); 
            SendCommand = new RelayCommand(
                Send,
                CanSend
            );

            BuzzCommand = new RelayCommand(
                Buzz,
                CanBuzz
            );
        }

        public void Buzz(object obj)
        {
            Chat.Buzz();
        }

        public bool CanBuzz(object obj)
        {
            return true;
        }

        public void Send(object obj)
        {
            Chat.Send();
        }

        public void MessageReceived(ProtocolModel protocol_in)
        {
            Chat.MessageReceived(protocol_in);
        }

        public bool CanSend(object obj)
        {
            return (!String.IsNullOrWhiteSpace(Chat.TextBox) && Chat.TextBox.Length < 500 );
        }

        public ChatModel Chat
        {
            get
            {
                return _Chat;
            }
        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Chat.EndChat();
        }
    }
}
