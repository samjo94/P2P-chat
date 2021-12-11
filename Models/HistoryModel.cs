using System.Collections.ObjectModel;
using System.ComponentModel;
using TDDD49.DataHandling;
using TDDD49.ViewModels;
using TDDD49.Views;

namespace TDDD49.Models
{
    class HistoryModel : INotifyPropertyChanged
    {
        private string _SearchPhrase;
        private DataHandler _DataHandler;
        private ObservableCollection<string> _Conversations;
        private string _SelectedConversation;
        private ObservableCollection<Message> _Conversation;
        public HistoryModel()
        {
            _DataHandler = new DataHandler();
            _SearchPhrase = "";
            _Conversations = _DataHandler.GetSearchResults("");
        }

        public string SelectedConversation
        {
            get
            {
                return _SelectedConversation;
            }
            set
            {
                _SelectedConversation = value;
                if (_SelectedConversation != null)
                {
                    ShowSelected();
                }
            }
        }
        public ObservableCollection<Message> Conversation
        {
            get
            {
                return _Conversation;
            }
            set
            {
                OnPropertyChanged("Conversation");
                _Conversation = value;
            }
        }

        public void Search()
        {
            Conversations = _DataHandler.GetSearchResults(SearchPhrase);
        }

        private void ShowSelected()
        {
            string timestamp = SelectedConversation.Substring(SelectedConversation.Length - 8, 8);
            Conversation = DataHandler.GetChat(timestamp);
            ConversationHistoryView HistoryView = new ConversationHistoryView();
            ConversationHistoryViewModel HistoryViewModel = new ConversationHistoryViewModel(this);
            HistoryView.DataContext = HistoryViewModel;
            HistoryView.Show();

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

        public ObservableCollection<string> Conversations
        {
            get
            {
                return _Conversations;
            }
            set
            {
                _Conversations = value;
                OnPropertyChanged("Conversations");
            }
        }

        public string SearchPhrase
        {
            get
            {
                return _SearchPhrase;
            }
            set
            {
                _SearchPhrase = value;
                OnPropertyChanged("SearchPhrase");
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
    }
}
