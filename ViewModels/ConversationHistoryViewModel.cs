using System;
using TDDD49.Models;

namespace TDDD49.ViewModels
{
    class ConversationHistoryViewModel
    {
        private HistoryModel _History;
        public ConversationHistoryViewModel(HistoryModel history)
        {
            _History = history;
        }
        public HistoryModel History
        {
            get
            {
                return _History;
            }
            set
            {
                _History = value;
            }
        }
    }
}
