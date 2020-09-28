using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Conversation
    {
        public int idConversation { get; set; }
        public string name { get; set; }

        public UserProfile loggedUser { get; set; }
        public List<UserProfile> participants { get; set; }
        public List<Messaged> messages { get; set; }

        public Conversation(int idConversation)
        {
            messages = new List<Messaged>();
            participants = new List<UserProfile>();

            this.idConversation = idConversation;
        }

        public Conversation(int idConversation, string name)
        {
            messages = new List<Messaged>();
            participants = new List<UserProfile>();

            this.idConversation = idConversation;
            this.name = name;
        }

        public void changethings()
        {

        }

        internal string GetNameUsuarios()
        {
            string result = "";

            result += participants[0].username;
            for (int i = 1; i < participants.Count; i++)
            {
                result += "," + participants[i].username;
            }
            return result;
        }
    }
}
