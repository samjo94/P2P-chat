using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using TDDD49.Models;


namespace TDDD49.DataHandling
{
    class DataHandler
    {
        internal class ChatData{

            private DateTime _ChatStart;
            private string _Username;
            private List<Message> _MessageHistory;

            public ChatData(string n_Username )
            {
                _ChatStart = DateTime.Now;
                _Username = n_Username;
                _MessageHistory = new List<Message>();
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

            public List<Message> MessageHistory
            {
                get
                {
                    return _MessageHistory;
                }
                set
                {
                    _MessageHistory = value;
                }
            }

            public DateTime ChatStart{
                get
                {
                    return _ChatStart;
                }
                set
                {
                    _ChatStart = value;
                }
            }

            public void AddMessage(Message message)
            {
                MessageHistory.Add(message);
            }

        }

        private ChatData CurrentChat;
        public DataHandler()
        {
        }

        public void NewChat(string Username)
        {
            CurrentChat = new ChatData(Username);
        }

        public void SaveChat()
        {
            string data;

            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TDDD49\\ChatHistory");
            Directory.CreateDirectory(folder);
           
            var filepath = Path.Combine(folder,  "chatting_with_" + CurrentChat.Username + ".json"); //  <---------   RENAME FILE TO CHATHISTORY OR SOMETHING SIMILAR
            if (File.Exists(filepath))
            {
                var string_data = File.ReadAllText(filepath);
                var ChatDataList = JsonConvert.DeserializeObject<List<ChatData>>(string_data);
                ChatDataList.Add(CurrentChat);
                data = JsonConvert.SerializeObject(ChatDataList);
                File.WriteAllText(filepath, data);
            } 
            else
            {
                var ChatDataList = new List<ChatData>();
                ChatDataList.Add(CurrentChat);
                data = JsonConvert.SerializeObject(ChatDataList);
                File.WriteAllText(filepath, data);
            }
        }

        public ObservableCollection<string> GetSearchResults(string SearchPhrase)
        {
            ObservableCollection<string> Results = new ObservableCollection<string>();
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TDDD49\\ChatHistory");
            Directory.CreateDirectory(folder);

            var filepath = Path.Combine(folder, "chatting_with_" + "Charlie" + ".json"); //  <---------   RENAME FILE TO CHATHISTORY OR SOMETHING SIMILAR
            var string_data = File.ReadAllText(filepath);
            var data = JsonConvert.DeserializeObject<List<ChatData>>(string_data);


            var filtered_data = from conv in data
                   where conv.Username.ToUpper().Contains(SearchPhrase.ToUpper())
                   orderby conv.ChatStart descending
                   select conv;



            foreach (var user in filtered_data)
            {
                Results.Add(user.Username + " - " + user.ChatStart);
            }
            return Results;

        }

        public void AddMessage( Message message)
        {
            message.TimeStamp = DateTime.Now;
            CurrentChat.AddMessage(message);
        }

        public ObservableCollection<Message> GetChat(string timestamp)
        {
            ObservableCollection<Message> Result = new ObservableCollection<Message>();
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TDDD49\\ChatHistory");
            Directory.CreateDirectory(folder);

            var filepath = Path.Combine(folder, "chatting_with_" + "Charlie" + ".json"); //  <---------   RENAME FILE TO CHATHISTORY OR SOMETHING SIMILAR
            var string_data = File.ReadAllText(filepath);
            var data = JsonConvert.DeserializeObject<List<ChatData>>(string_data);

        

            var filtered_data = from conv in data
                                where conv.ChatStart.ToString().Contains(timestamp)
                                orderby conv.ChatStart ascending
                                select conv.MessageHistory.ToList();

            var res = filtered_data.SelectMany(d => d).ToList();

            
            foreach (var message in res)
            {
                Result.Add(message);
            }

            return Result;
        }
    }
}
