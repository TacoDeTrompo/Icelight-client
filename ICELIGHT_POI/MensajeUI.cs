using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Client;
using System.Text.RegularExpressions;

namespace ICELIGHT_POI
{
    class MensajeUI
    {
        Label nombreEmisor;
        Label horaMensaje;
        PictureBox Luz;
        public static Image Azul;
        public static Image Rosa;
        RichTextBox Mensajes;
        //Usuario userMensaje;
        Panel Padre;
        private int widthSize;
        private int YInicioChat;
        private int XInicioChat;
        private int XInicioPropio;
        private int XInicioExterno;

        UserProfile senderProfile;

        public MensajeUI(Messaged message, Panel dueno)
        {
            Padre = dueno;
            nombreEmisor = new Label();
            horaMensaje = new Label();
            Luz = new PictureBox();
            Mensajes = new RichTextBox();

            widthSize = 475;
            YInicioChat = Padre.Size.Height;


            //Usar la lista de usuarios de INICIO
            this.senderProfile = message.senderProfile;

            //Agregar que ponga 0 en los numeros menores a 10
            string fechaHora = message.timestamp.Hour.ToString() + ":" + message.timestamp.Minute.ToString();
            horaMensaje.Text = fechaHora;

            //Por ahora Locacion Inicial Usuario Principal
            // x = 395; 
            XInicioPropio = 395;
            //Por ahora Locacion Inicial Usuario otro
            // x = 105
            // La Y tiene que aumentar con canda mensaje en un total de nombreemisor.height luz.height Mensajes.height
            XInicioExterno = 90;//105
            //men.getUsuario().GetIdUsuario() == LOGIN.usuario.GetIdUsuario()
            if (senderProfile.idUser == Log.loggedUser.idUser)
            {
                XInicioChat = XInicioPropio;                
            }
            else
            {
                XInicioChat = XInicioExterno;
            }


            //nombreEmisor.BackColor = dueno.BackColor;
            nombreEmisor.BackColor = Padre.BackColor;
            horaMensaje.BackColor = Padre.BackColor;
            //Luz.BackColor = Padre.BackColor;
            Luz.BackColor = Padre.BackColor;
            Mensajes.BackColor = Padre.BackColor;//Padre.BackColor
            Mensajes.ForeColor = Color.FromArgb(41, 39, 63);
            nombreEmisor.ForeColor = Color.FromArgb(41, 39, 63);
            horaMensaje.ForeColor = Color.FromArgb(41, 39, 63);

            CreateMessage(message);
        }
        public void CreateMessage(Messaged message)
        {
            Padre.Controls.Add(nombreEmisor);
            Padre.Controls.Add(horaMensaje);
            Padre.Controls.Add(Mensajes);
            Padre.Controls.Add(Luz);

            // La perte del igual hay que cambiarla al Cambiar el Usuarios de lugar debe estar en INICIO.Usuarios

            nombreEmisor.Text = senderProfile.username;


            //Aqui va algo como Inicio.UsuarioActual
            //nombreEmisor.Text = mensaje.idUsuario.ToString();


            //mensaje.getUsuario().GetIdUsuario() == LOGIN.usuario.GetIdUsuario()
            if (message.senderProfile.idUser == Log.loggedUser.idUser)//if (mensaje.idUsuario == ChatUI.idUsuarioActual)
                Luz.Image = Azul;
            else
                Luz.Image = Rosa;

            //Luz.Width = 633;
            //Luz.Height = 30;
            Luz.Size = new Size(430, 20);
            Luz.SizeMode = PictureBoxSizeMode.StretchImage;
            //Luz.BackColor = Padre.BackColor;
            

            Mensajes.AutoSize = true;
            Mensajes.MaximumSize = new Size(widthSize, 1000);
            Mensajes.MinimumSize = new Size(widthSize, 0);
            
            Mensajes.BorderStyle = BorderStyle.None;
            Mensajes.Margin = new Padding(6, 3, 3, 3);

            nombreEmisor.MaximumSize = new Size(widthSize, 50);
            nombreEmisor.MinimumSize = new Size(widthSize, 0);
            horaMensaje.MaximumSize = new Size(40, 50);
            horaMensaje.MinimumSize = new Size(40, 0);


            string mensajeemoteado = message.message;

            string[] mensajin = Regex.Split(mensajeemoteado, "%\r\n%");

            if(mensajin[0] == "-_ENCODED_-")
            {
                mensajeemoteado = INICIO.Base64Decode(mensajin[1]);
            }

            foreach (INICIO.Emote em in INICIO.emotes)
            {
                mensajeemoteado = mensajeemoteado.Replace(em.nombre, em.rtfformat);
            }
            int indaux = Mensajes.Rtf.LastIndexOf(@"\par");
            Mensajes.Rtf = Mensajes.Rtf.Insert(indaux, mensajeemoteado);            

            //Posiciones Iniciales de los Objetos
            nombreEmisor.Location = new Point(XInicioChat, YInicioChat - nombreEmisor.Height - Luz.Height - Mensajes.Height);            
            Luz.Location = new Point(XInicioChat, nombreEmisor.Location.Y + nombreEmisor.Height);
            Mensajes.Location = new Point(XInicioChat, nombreEmisor.Location.Y + nombreEmisor.Height + Luz.Height);


            //Correcto          mensaje.getUsuario().GetIdUsuario() == LOGIN.usuario.GetIdUsuario()  
            if (message.idSender == Log.loggedUser.idUser)
            {
                //Si eres tu 
                nombreEmisor.TextAlign = ContentAlignment.BottomRight;
                horaMensaje.TextAlign = ContentAlignment.BottomLeft;
                //Mensajes.TextAlign = ContentAlignment.TopRight;
                horaMensaje.Location = new Point(XInicioChat, YInicioChat - nombreEmisor.Height - Luz.Height - Mensajes.Height);
            }
            else
            {
                //Si es algun otro de la conversacion
                nombreEmisor.TextAlign = ContentAlignment.BottomLeft;
                horaMensaje.TextAlign = ContentAlignment.MiddleRight;
                horaMensaje.Location = new Point(XInicioChat + widthSize - horaMensaje.Width , YInicioChat - nombreEmisor.Height - Luz.Height - Mensajes.Height);
                //Mensajes.TextAlign = ContentAlignment.TopLeft;
            }
            
            Mensajes.RightToLeft = RightToLeft.No;

            Mensajes.ReadOnly = true;

            nombreEmisor.Show();
            Mensajes.Show();
            horaMensaje.Show();
            Luz.Show();
            horaMensaje.BringToFront();
            Padre.Refresh();            
        }
        public void moveUpp(MensajeUI mess)
        {
            nombreEmisor.Location = new Point(XInicioChat, mess.nombreEmisor.Location.Y - nombreEmisor.Height - Luz.Height - Mensajes.Height);
            horaMensaje.Location = new Point(horaMensaje.Location.X, mess.nombreEmisor.Location.Y - nombreEmisor.Height - Luz.Height - Mensajes.Height);
            Luz.Location = new Point(XInicioChat, nombreEmisor.Location.Y + nombreEmisor.Height);
            Mensajes.Location = new Point(XInicioChat, nombreEmisor.Location.Y + nombreEmisor.Height + Luz.Height);
            Padre.Refresh();
        }
    }
}