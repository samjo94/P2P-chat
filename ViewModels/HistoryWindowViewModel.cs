using TDDD49.Commands;
using TDDD49.Models;

namespace TDDD49.ViewModels
{
    class HistoryWindowViewModel
    {
        private HistoryModel _History;
        public RelayCommand SearchCommand
        {
            get;
            private set;
        }
        public HistoryWindowViewModel()
        {
            _History = new HistoryModel();
            SearchCommand = new RelayCommand(
                Search,
                CanSearch
            );


        }

        public HistoryModel History
        {
            get
            {
                return _History;
            }
            
        }

        public void Search(object obj)
        {
            History.Search();
        }
        public bool CanSearch(object obj)
        {
            return true;
        }
        
    }
}
