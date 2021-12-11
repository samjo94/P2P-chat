using TDDD49.Commands;
using TDDD49.Models;

namespace TDDD49.ViewModels
{
    class ChatRequestViewModel
    {
        private bool _Response;
        private bool _Run;
        private bool _Accepted;
        private bool _Declined;
        public RelayCommand AcceptCommand
        {
            get;
            private set;
        }
        public RelayCommand RefuseCommand
        {
            get;
            private set;
        }
        public ChatRequestViewModel()
        {
            _Run = true;
            _Request = new RequestInfo();
            _Accepted = false;
            _Declined = false;
            AcceptCommand = new RelayCommand(
                Accept,
                CanAccept);
            RefuseCommand = new RelayCommand(
                Refuse,
                CanRefuse);
        }

        public void Accept(object obj)
        {
            Accepted = true;
        }

        public bool CanAccept(object obj)
        {
            return true;
        }

        public void Refuse(object obj)
        {
            Declined = true;
        }

        public bool CanRefuse(object obj)
        {
            return true;
        }

        public bool Response
        {
            get
            {
                return _Response;
            }
            set
            {
                _Response = value;
            }
        }
        private RequestInfo _Request;
        public RequestInfo Request
        {
            get
            {
                return _Request;
            }
        }

        public bool Run
        {
            get
            {
                return _Run;
            }
            set
            {
                _Run = value;
            }
        }
        public bool Accepted
        {
            get
            {
                return _Accepted;
            }
            set
            {
                _Accepted = value;
            }
        }
        public bool Declined
        {
            get
            {
                return _Declined;
            }
            set
            {
                _Declined = value;
            }
        }

    }
}
