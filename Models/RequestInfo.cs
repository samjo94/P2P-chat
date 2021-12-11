using System.ComponentModel;

namespace TDDD49.Models
{
    class RequestInfo : INotifyPropertyChanged
    {
        private string _username;
        public RequestInfo()
        {        }

        public string User
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                OnPropertyChanged("Username");
            }
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
