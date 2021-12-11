using System;

namespace TDDD49.Models
{
    class Message
    {
        private DateTime _TimeStamp;
        private string _Text;
        private string _Author;
        public Message(string author, string message = null)
        {
            _Author = author;
            _Text = message;

        }
        public string Text
        {
            get { 
                return _Text; }
            set
            {
                _Text = value;
            }
        }
        public string Author
        {
            get { return _Author; }
            set
            {
                _Author = value;
            }
        }
        public DateTime TimeStamp
        {
            get
            {
                return _TimeStamp;
            }
            set
            {
                _TimeStamp = value;
            }
        }
    }
}
