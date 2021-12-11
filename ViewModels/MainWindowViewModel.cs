namespace TDDD49.ViewModels
{
    using System;
    using TDDD49.Models;
    using TDDD49.Commands;
    using System.Threading;
    using TDDD49.Views;

    class MainWindowViewModel
    {
        private Thread ListenThread;
        private Thread PingThread;
        private ChatRequestViewModel RequestViewModel;
        private ChatRequestView RequestView;
        private ChatWindowViewModel ChatViewModel;
        private ChatWindowView ChatView;
        private HistoryWindowViewModel HistoryViewModel;
        private HistoryView HistoryView;
        
        private bool was_listening = false;

        
        private ListenInfo _Connection;
        public ListenInfo Connection
        {
            get
            {
                return _Connection;
            }
        }
        private PingInfo _Connector;
        public PingInfo Connector
        {
            get
            {
                return _Connector;
            }
        }
        public RelayCommand ListenCommand
        {
            get;
            private set;
        }
        public RelayCommand PingCommand
        {
            get;
            private set;
        }
        public RelayCommand ShowHistoryCommand
        {
            get;
            private set;
        }

        public MainWindowViewModel()
        {
            ListenCommand = new RelayCommand(
                ToggleListenThread,
                CanListen);
            PingCommand = new RelayCommand(
                StartPingThread,
                CanPing);
            ShowHistoryCommand = new RelayCommand(
                ShowHistory,
                CanShowHistory);
            _Connection = new ListenInfo("User", 6000); 
            _Connector = new PingInfo("127.0.0.1", 6000);

        }
        public void ShowHistory(object obj)
        {
            HistoryView = new HistoryView();
            HistoryViewModel = new HistoryWindowViewModel();
            HistoryView.DataContext = HistoryViewModel;
            HistoryView.Show();
        }

        public bool CanShowHistory(object obj)
        {
            return true;
        }

        public void ToggleListenThread(object obj)
        {
            if (ListenThread != null)
            {
               
                if (ListenThread.IsAlive)
                {
                    Connection.Close();
                    ListenThread.Abort();
                    Connection.DisplayString = "Start Listening";
                }
                else
                {
                    Connection.DisplayString = "Stop Listening";
                    ListenThread = new Thread(Connection.Listen);
                    ListenThread.Start();
                    
                }
                
            }
            else
            {
                ListenThread = new Thread(Connection.Listen);
                ListenThread.Start();
            }
        }

        private bool CanListen(object obj)
        {
            return (Connection.listenPort > 0 && !String.IsNullOrWhiteSpace(Connection.userName));
        }
        public void StartPingThread(object obj)
        {
            Connector.userName = Connection.userName;
            
            if (ListenThread != null)
            {
                was_listening = true;
                ListenThread.Abort();
                Connection.Close();
            }


            PingThread = new Thread(Connector.Ping);
            PingThread.Start();
        }

        public bool CanPing(object obj)
        {
            return (Connector.connectPort > 0 && !String.IsNullOrWhiteSpace(Connector.ip));
        }

        


    }
}

