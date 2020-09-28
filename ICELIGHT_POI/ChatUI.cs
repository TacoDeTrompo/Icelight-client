using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Client;
namespace ICELIGHT_POI
{
    class ChatUI
    {
        public Panel chat;
        Control Padre;
        //Conversacion actual;
        Conversation actualConversation;
        //Esto va en la ventana Inicial
                     
        List<MensajeUI> mensajes;

        public ChatUI(Control dueno)
        {
            Padre = dueno;
            chat = new Panel
            {
                Location = new Point(30, 121),
                Size = new Size(882, 542),
                BackColor = Color.AliceBlue
            };
            mensajes = new List<MensajeUI>();
            dueno.Controls.Add(chat);
            chat.Show();
            chat.BringToFront();
        }
        public ChatUI(Control dueno,Conversation conversation)
        {
            Padre = dueno;
            chat = new Panel
            {
                Location = new Point(68, 55),
                Size = new Size(800, 500),
                BackColor = Color.FromArgb(230, 200, 140)
            };
            mensajes = new List<MensajeUI>();
            dueno.Controls.Add(chat);
            chat.Show();
            chat.BringToFront();
            setCurrentConversacion(conversation);
        }

        public void NuevoMensaje(string mens)
        {
            string aux = mens.Substring(0, mens.IndexOf('-'));
            string Resto = mens.Substring(mens.IndexOf('-') + 1);
            UserProfile a;
            if (aux == Log.loggedUser.username)
                a = Log.loggedUser;
            else
                a = INICIO.list_Guests.Find(x => x.username == aux);

            actualConversation.messages.Add(new Messaged(a, Resto));            
            mensajes.Add(new MensajeUI(actualConversation.messages.Last(), chat));

            for (int i = mensajes.Count - 2; i >= 0; i--)
            {
                mensajes[i].moveUpp(mensajes[i + 1]);
            }
        }

        public void setCurrentConversacion(Conversation conversation)
        {
            actualConversation = conversation;
            CargarConvoToUI(conversation);
        }

        private void CargarConvoToUI(Conversation conversation)
        {
            chat.Controls.Clear();
            mensajes.Clear();
            
            foreach(Messaged message in conversation.messages)
            {
                mensajes.Add(new MensajeUI(message, chat));
                for (int i = mensajes.Count - 2; i >= 0; i--)
                {
                    mensajes[i].moveUpp(mensajes[i + 1]);
                }
            }
        }

        public Conversation GetConversacion()
        {
            return actualConversation;
        }
    }
}
