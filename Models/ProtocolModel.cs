namespace TDDD49.Models
{
    class ProtocolModel
    {
        private string _Username;
        private int _Status;
        private Message _Message;

        public ProtocolModel(string username, int status, Message message)
        {
            _Username = username;
            _Status = status;
            _Message = message;
        }

        public string Username
        {
            get
            {
                return _Username;
            }
            set
            {
                _Username = value;
            }
        }

        public int Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
            }
        }

        public Message Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
            }
        }
    }
}
