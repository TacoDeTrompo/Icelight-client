using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Messaged
    {
        public int idMessage { get; set; }
        public int idSender { get; set; }
        public string message { get; set; }
        public System.DateTime timestamp { get; set; }
        public int idConversation { get; set; }

        public UserProfile senderProfile { get; set; }

        public Messaged(int idMessage, int idSender, string message, DateTime timestamp, int idConversation,
            List<UserProfile> list_Friends, UserProfile loggedUser)
        {
            this.idMessage = idMessage;
            this.idSender = idSender;
            this.message = message;
            this.timestamp = timestamp;
            this.idConversation = idConversation;

            AssignSenderName(list_Friends, loggedUser);
        }

        public void AssignSenderName(List<UserProfile> list_Friends, UserProfile loggedUser) //Change to guests
        {
            foreach (UserProfile friend in list_Friends)
            {
                if (idSender == friend.idUser)
                    senderProfile = friend;
                else
                    if (idSender == loggedUser.idUser)
                    senderProfile = loggedUser;
            }
        }

        public Messaged(UserProfile senderProfile, string message/*, int idConversation*/)
        {
            timestamp = DateTime.Now;
            this.senderProfile = senderProfile;
            this.message = message;
            idSender = senderProfile.idUser;
            //We need to set id conversaation!!!!!!!!!!!!!!!!!!!
        }
    }
}
