using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using Client;
using System.Net.Sockets;
using System.Windows.Forms;
using System.IO;
using AForge.Video.DirectShow;
using AForge.Video;
using System.Text.RegularExpressions;

namespace ICELIGHT_POI
{
    public partial class INICIO : Form
    {
        ///VIDEO CHAT
        Image imgchat;
        Image imgstream;
        bool haystreaming = false;
        bool videoCallAchieved = false;
        bool ExisteDispositivo = false;
        bool startedCall = false;
        bool locked = false;
        FilterInfoCollection DispositivoDeVideo;
        VideoCaptureDevice FuenteDeVideo;

        public delegate void Zumbido();

        UdpClient socketrecibevideo;

        //Puerto Servidor

        int PuertoChat = 2000 + Log.loggedUser.idUser;
        Thread tv_chat;

        internal static List<Usuario> list_Usuarios;

        internal static List<UserProfile> list_Friends;
        internal static List<Conversation> list_Conversations;
        internal static List<Messaged> list_Messages;
        internal static List<UserProfile> list_Guests;

        internal static bool justLogged = false;

        internal static List<ChatUI> list_chats;
        internal List<string> list_emoticonos;
        private List<string> list_mensajes;

        private static int indexMaximo;
        static Socket soc;

        Log loginPtr;
        bool logHidden = false;

        internal struct Emote
        {

            public string rtfformat;
            public string nombre;
            public Emote(string rtf, string nomb)
            {
                rtfformat = rtf;
                nombre = nomb;
            }
        }
        internal static List<Emote> emotes;

        #region VentanasInternas

        //EMOTICONOS emo;
        Mensajeria formMensajes;

        #endregion


        //Esto Se tiene que cambiar despues
        private delegate void DAddItem(String s);
        private delegate void DGetTv(Byte[] tv);
        private delegate void DIniLlamada();
        private delegate void ChangeStatus(String s);
        private delegate void ChangeRank(String s);

        public INICIO()
        {
            InitializeComponent();
            this.FormClosing += INICIO_FormClosing;
        }

        public INICIO(Log form)
        {
            InitializeComponent();
            loginPtr = form;
            logHidden = true;
            this.FormClosing += INICIO_FormClosing;
        }

        private void INICIO_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (soc != null)
            {
                soc.Send(Encoding.ASCII.GetBytes(""));
                soc.Shutdown(SocketShutdown.Both);
                soc.Close();
            }
            if (socketrecibevideo != null)
            {
                socketrecibevideo.Close();
            }
            if (FuenteDeVideo != null)
                if (FuenteDeVideo.IsRunning)
                {
                    TerminarFuenteDeVideo();
                    PB_YO.Image = null;
                    BTN_OK.Text = "OK";
                    BTN_INICIAR.Enabled = false;
                    COB_LLAMADA.Enabled = true;
                }
            if (logHidden)
            {
                loginPtr.Close();
            }
        }

        private void INICIO_Load(object sender, EventArgs e)
        {
            MainMenu.inicio = this;
            Mensajeria.inicio = this;
            Historia.inicio = this;
            LLAMADA.inicio = this;
            Rangos.inicio = this;


            CargarEmojis();

            indexMaximo = 0;

            socketrecibevideo = new UdpClient(11000);
            socketrecibevideo.Client.Blocking = false;
            socketrecibevideo.Client.ReceiveTimeout = 100;
            
            BuscarDispositivos();


            //Creacion de listas            
            //L_conversaciones = new List<Conversacion>();
            list_Usuarios = new List<Usuario>();

            list_Friends = new List<UserProfile>();
            list_Conversations = new List<Conversation>();
            list_Messages = new List<Messaged>();
            list_Guests = new List<UserProfile>();

            list_chats = new List<ChatUI>();
            list_emoticonos = new List<string>();
            list_mensajes = new List<string>();


            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// EMOTICONOES

            soc = Log.socket;
            byte[] data = new byte[20];

            //Conseguir Contatos del Servidor
            ReceiveUserFriends(soc, data);

            //Recibir Las conversaciones
            ReceiveUserConversations(soc, data);

            ////Recibir los participantes de conversasiones
            ReceiveUserGuests(soc, data);

            //Recibir los mensajes
            ReceiveUserMessages(soc, data);


            //Empezar a escuchar por mensajes
            Thread T = new Thread(HeyListen);
            T.Start();

            //Llenar Informacion de Cosas
            MensajeUI.Rosa = Properties.Resources.RedLine;
            MensajeUI.Azul = Properties.Resources.RedLine;


            if (this.PN_BOTONES.Controls.Count > 0)
                this.PN_BOTONES.Controls.RemoveAt(0);

            MainMenu hijo1 = new MainMenu();
            hijo1.TopLevel = false;
            hijo1.FormBorderStyle = FormBorderStyle.None;
            hijo1.Dock = DockStyle.Fill;
            this.PN_BOTONES.Controls.Add(hijo1);
            this.PN_BOTONES.Tag = hijo1;
            hijo1.Show();

            //Agregando Chats y Cosas
            foreach (Conversation conversation in list_Conversations)
            {
                list_chats.Add(new ChatUI(PN_CHAT, conversation));
            }

            //Esconder todos los chats para que no esten molestando
            foreach (ChatUI cu in list_chats)
            {
                cu.chat.VisibleChanged += cambioConversacion;                
            }

            PB_IMG_Usuario.BackColor = Color.Transparent;

        }

        private void INICIO_Shown(object sender, EventArgs e)
        {

        }

        //public void FN_IC_CONF()
        //{
        //    if (this.PN_BOTONES.Controls.Count > 0)
        //        this.PN_BOTONES.Controls.RemoveAt(0);
        //    Opciones hijo1 = new Opciones();
        //    hijo1.TopLevel = false;
        //    hijo1.FormBorderStyle = FormBorderStyle.None;
        //    hijo1.Dock = DockStyle.Fill;
        //    this.PN_BOTONES.Controls.Add(hijo1);
        //    this.PN_BOTONES.Tag = hijo1;
        //    hijo1.Show();
        //}

        public void FN_IC_MENSAJE()
        {
            if (this.PN_BOTONES.Controls.Count > 0)
                this.PN_BOTONES.Controls.RemoveAt(0);

            if (formMensajes == null)
            {
                formMensajes = new Mensajeria();
                formMensajes.TopLevel = false;
                formMensajes.FormBorderStyle = FormBorderStyle.None;
                formMensajes.Dock = DockStyle.Fill;
            }
            PN_BOTONES.Controls.Add(formMensajes);
            PN_BOTONES.Tag = formMensajes;
            formMensajes.Show();
        }

        public void FN_IC_PERFL()
        {
            if (this.PN_BOTONES.Controls.Count > 0)
                this.PN_BOTONES.Controls.RemoveAt(0);
            Historia hijo1 = new Historia();
            hijo1.TopLevel = false;
            hijo1.FormBorderStyle = FormBorderStyle.None;
            hijo1.Dock = DockStyle.Fill;
            this.PN_BOTONES.Controls.Add(hijo1);
            this.PN_BOTONES.Tag = hijo1;
            hijo1.Show();
        }

        public void FN_IC_LLAMADA()
        {
            if (this.PN_BOTONES.Controls.Count > 0)
                this.PN_BOTONES.Controls.RemoveAt(0);
            LLAMADA hijo1 = new LLAMADA();
            hijo1.TopLevel = false;
            hijo1.FormBorderStyle = FormBorderStyle.None;
            hijo1.Dock = DockStyle.Fill;
            this.PN_BOTONES.Controls.Add(hijo1);
            this.PN_BOTONES.Tag = hijo1;
            hijo1.Show();
            if (!locked)
            {
                startedCall = true;
                locked = true;
            }
        }

        public void FN_BTN_SALIR_CONF()
        {
            if (this.PN_BOTONES.Controls.Count > 0)
                this.PN_BOTONES.Controls.RemoveAt(0);
            MainMenu hijo1 = new MainMenu();
            hijo1.TopLevel = false;
            hijo1.FormBorderStyle = FormBorderStyle.None;
            hijo1.Dock = DockStyle.Fill;
            this.PN_BOTONES.Controls.Add(hijo1);
            this.PN_BOTONES.Tag = hijo1;
            hijo1.Show();
        }

        public void FN_BTN_SALIR_MENS()
        {
            if (this.PN_BOTONES.Controls.Count > 0)
                this.PN_BOTONES.Controls.RemoveAt(0);
            MainMenu hijo1 = new MainMenu();
            hijo1.TopLevel = false;
            hijo1.FormBorderStyle = FormBorderStyle.None;
            hijo1.Dock = DockStyle.Fill;
            this.PN_BOTONES.Controls.Add(hijo1);
            this.PN_BOTONES.Tag = hijo1;
            hijo1.Show();
        }

        public void FN_BTN_SALIR_PERFIL()
        {
            if (this.PN_BOTONES.Controls.Count > 0)
                this.PN_BOTONES.Controls.RemoveAt(0);
            MainMenu hijo1 = new MainMenu();
            hijo1.TopLevel = false;
            hijo1.FormBorderStyle = FormBorderStyle.None;
            hijo1.Dock = DockStyle.Fill;
            this.PN_BOTONES.Controls.Add(hijo1);
            this.PN_BOTONES.Tag = hijo1;
            hijo1.Show();
        }

        // LLAMADASSSS

        public void ReturnToMainMenuSidebar()
        {
            if (this.PN_BOTONES.Controls.Count > 0)
                this.PN_BOTONES.Controls.RemoveAt(0);
            MainMenu hijo1 = new MainMenu();
            hijo1.TopLevel = false;
            hijo1.FormBorderStyle = FormBorderStyle.None;
            hijo1.Dock = DockStyle.Fill;
            this.PN_BOTONES.Controls.Add(hijo1);
            this.PN_BOTONES.Tag = hijo1;
            hijo1.Show();
        }

        //public void FN_IC_LLAMADA_LLAMADA()
        //{
        //    if (this.PN_BOTONES.Controls.Count > 0)
        //        this.PN_BOTONES.Controls.RemoveAt(0);
        //    LLAMADA_LLAMADA hijo1 = new LLAMADA_LLAMADA();
        //    hijo1.TopLevel = false;
        //    hijo1.FormBorderStyle = FormBorderStyle.None;
        //    hijo1.Dock = DockStyle.Fill;
        //    this.PN_BOTONES.Controls.Add(hijo1);
        //    this.PN_BOTONES.Tag = hijo1;
        //    hijo1.Show();
        //}

        //public void FN_IC_LLAMADA_MENSAJE()
        //{
        //    if (this.PN_BOTONES.Controls.Count > 0)
        //        this.PN_BOTONES.Controls.RemoveAt(0);
        //    LLAMADA_MENSAJE hijo1 = new LLAMADA_MENSAJE();
        //    hijo1.TopLevel = false;
        //    hijo1.FormBorderStyle = FormBorderStyle.None;
        //    hijo1.Dock = DockStyle.Fill;
        //    this.PN_BOTONES.Controls.Add(hijo1);
        //    this.PN_BOTONES.Tag = hijo1;
        //    hijo1.Show();
        //}

        //public void FN_BTN_SALIR_LLAMADA_LLAMADA()
        //{
        //    if (this.PN_BOTONES.Controls.Count > 0)
        //        this.PN_BOTONES.Controls.RemoveAt(0);
        //    LLAMADA hijo1 = new LLAMADA();
        //    hijo1.TopLevel = false;
        //    hijo1.FormBorderStyle = FormBorderStyle.None;
        //    hijo1.Dock = DockStyle.Fill;
        //    this.PN_BOTONES.Controls.Add(hijo1);
        //    this.PN_BOTONES.Tag = hijo1;
        //    hijo1.Show();
        //}

        //public void FN_BTN_SALIR_LLAMADA_MENSAJE()
        //{
        //    if (this.PN_BOTONES.Controls.Count > 0)
        //        this.PN_BOTONES.Controls.RemoveAt(0);
        //    LLAMADA hijo1 = new LLAMADA();
        //    hijo1.TopLevel = false;
        //    hijo1.FormBorderStyle = FormBorderStyle.None;
        //    hijo1.Dock = DockStyle.Fill;
        //    this.PN_BOTONES.Controls.Add(hijo1);
        //    this.PN_BOTONES.Tag = hijo1;
        //    hijo1.Show();
        //}

        private void addMessage(string mens)
        {
            if (list_chats.Count > 0)
            {
                string[] aux = mens.Split('$');
                int idcon = Int32.Parse(aux[0]);
                list_chats.Find(x => x.GetConversacion().idConversation == idcon).NuevoMensaje(aux[1]);
            }

            //CheckRequirementsLevelUp();
        }

        private void HeyListen()
        {
            byte[] data = new byte[1024];
            int recv = 0;

            while (soc.Connected)
            {
                try
                {
                    recv = soc.Receive(data);
                    string stringData = Encoding.ASCII.GetString(data, 0, recv);
                    string[] datos = stringData.Split('$');
                    if (datos[0] != "SERVER")
                        this.Invoke(new DAddItem(addMessage), stringData);
                    else
                    {
                        byte[] menaux = Encoding.ASCII.GetBytes("hola");
                        switch (datos[1])
                        {
                            case "ASKCHAT":
                                Thread threadask = new Thread(() => AskChat(datos));
                                threadask.Start();
                                break;
                            case "ACCCHAT":
                                this.Invoke(new DIniLlamada(iniciarLlamada));
                                break;
                            case "ADDUSER":
                                break;
                            case "ADDCONVE":
                                break;
                            case "CHANGESTATE":
                                this.Invoke(new ChangeStatus(ChangeStatusRealtime),stringData);
                                break;
                            case "LEVELUP":
                                this.Invoke(new ChangeRank(ChangeRankRealtime), stringData);
                                break;
                            case "BUZZ":
                                PerformLocalBuzz();
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    break;
                    throw;
                }
            }
        }

        public void ChangeStatusRealtime(string stringData)
        {
            string[] datos = stringData.Split('$');

            foreach (Conversation conversation in list_Conversations)
            {
                UserProfile toModify = conversation.participants.FirstOrDefault(x => x.idUser == int.Parse(datos[2]));
                if (toModify != null)
                    toModify.status = int.Parse(datos[3]);
            }

            RefreshFriendStatus();
            if (formMensajes != null)
            {
                formMensajes.RefreshProfileImage();
            }
        }

        public void ChangeRankRealtime(string stringData)
        {
            string[] datos = stringData.Split('$');

            foreach (Conversation conversation in list_Conversations)
            {
                UserProfile toModify = conversation.participants.FirstOrDefault(x => x.idUser == int.Parse(datos[2]));
                if (toModify != null)
                    toModify.points = int.Parse(datos[3]);
            }

            RefreshFriendStatus();
            if (formMensajes != null)
            {
                formMensajes.RefreshProfileImage();
            }
        }

        private void AskChat(string[]datos)
        {
            byte[] menaux;
            UserProfile usaux = list_Friends.Find(x => x.idUser == int.Parse(datos[2]));

            DialogResult res = MessageBox.Show("El usuario " + usaux.username + " quiere iniciar una video llamada", "Llamada Entrante", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {                                
                menaux = Encoding.ASCII.GetBytes("SERVER$ACCCHAT$"+ Log.loggedUser.idUser.ToString()+ "$" + datos[2]);
                soc.Send(menaux, menaux.Length, SocketFlags.None);
            }
            else
            {
                menaux = Encoding.ASCII.GetBytes("SERVER$DENCHAT$"+ Log.loggedUser.idUser.ToString() + "$" + datos[2]);
                soc.Send(menaux, menaux.Length, SocketFlags.None);
            }
        }

        private void TB_CHAT_INICIO_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void TB_CHAT_INICIO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                EnviarMensaje();
                e.Handled = true;
            }          
        }

        private void EnviarMensaje()
        {                        
            string aEnviar = "";
            string sepi = @"\cf1\f0\fs17";
            string sepf = @"\par";
            int indi = TB_CHAT_INICIO.Rtf.IndexOf(sepi);
            indi += sepi.Length;
            int indf = TB_CHAT_INICIO.Rtf.LastIndexOf(sepf);
            if (TB_CHAT_INICIO.Rtf[indi] != '{')
                indi++;
            if ((indf - indi) == 0)
                return;
            string prueba = TB_CHAT_INICIO.Rtf.Substring(indi, indf - indi);
            //\cf1\f0\fs17
            int indaux1 = prueba.IndexOf("{");
            int indaux2 = prueba.IndexOf("}");
            if (indaux1 != -1 && indaux2 != -1)
            {
                string imagenrtf = prueba.Substring(indaux1, indaux2 - indaux1 + 1);

                foreach (Emote em in emotes)
                {
                    prueba = prueba.Replace(em.rtfformat, em.nombre);
                }
            }
            aEnviar = prueba.Replace(@"\par", "");
            aEnviar = aEnviar.Replace("\r\n", "");
            if (chb_encode.Checked)
            {
                aEnviar = Base64Encode(aEnviar);
                aEnviar = "-_ENCODED_-%\r\n%" + aEnviar;
            }
            int a = 0;
            if (soc != null && aEnviar != "")
            {
                soc.Send(Encoding.ASCII.GetBytes(
                    list_chats[indexMaximo].GetConversacion().idConversation + "$" + Log.loggedUser.username + "-" + aEnviar));
            }
            TB_CHAT_INICIO.Clear();

        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        internal static void CargarMensajes(Conversation conversation)
        {

            list_chats[indexMaximo].chat.Visible = false;
            indexMaximo = list_Conversations.FindIndex(x => x.idConversation == conversation.idConversation);
            list_chats[indexMaximo].chat.Visible = false;
            list_chats[indexMaximo].chat.Visible = true;
            list_chats[indexMaximo].chat.BringToFront();            
        }

        private void cambioConversacion(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;

            Conversation conversation = list_chats[indexMaximo].GetConversacion();

            int rank = 0;

            if (conversation.participants.Count > 1)
            {
                rank = Log.loggedUser.points;
                bool isAnybodyThere = StatusConversation();

                switch (rank)
                {
                    case 0:
                        {
                            if (isAnybodyThere)
                                PB_IMG_Usuario.Image = Properties.Resources.Villens;
                            else
                                PB_IMG_Usuario.Image = Properties.Resources.VillensDesconectado;
                            break;
                        }
                    case 1:
                        {
                            if (isAnybodyThere)
                                PB_IMG_Usuario.Image = Properties.Resources.Knight1;
                            else
                                PB_IMG_Usuario.Image = Properties.Resources.KnightDesconectado;
                            break;
                        }
                    case 2:
                        {
                            {
                                if (isAnybodyThere)
                                    PB_IMG_Usuario.Image = Properties.Resources.Noble1;
                                else
                                    PB_IMG_Usuario.Image = Properties.Resources.NobleAusente;
                                break;
                            }
                        }
                    case 3:
                        {
                            {
                                if (isAnybodyThere)
                                    PB_IMG_Usuario.Image = Properties.Resources.Monarch1;
                                else
                                    PB_IMG_Usuario.Image = Properties.Resources.MonarchDesconect;
                                break;
                            }
                        }
                }
            }
            else
            {
                rank = conversation.participants[0].points;

                String displayProfile = conversation.GetNameUsuarios();

                LB_INFO_NOMBRE.Text = displayProfile;

                switch (conversation.participants[0].status)
                {
                    case 1:
                        {
                            if(rank == 0)
                                PB_IMG_Usuario.Image = Properties.Resources.Villens;
                            if (rank == 1)
                                PB_IMG_Usuario.Image = Properties.Resources.Knight1;
                            if (rank == 2)
                                PB_IMG_Usuario.Image = Properties.Resources.Noble1;
                            if (rank == 3)
                                PB_IMG_Usuario.Image = Properties.Resources.Monarch1;
                            break;
                        }
                    case 2:
                        {
                            if (rank == 0)
                                PB_IMG_Usuario.Image = Properties.Resources.VillensAusente;
                            if (rank == 1)
                                PB_IMG_Usuario.Image = Properties.Resources.KnightAusente;
                            if (rank == 2)
                                PB_IMG_Usuario.Image = Properties.Resources.NobleAusente;
                            if (rank == 3)
                                PB_IMG_Usuario.Image = Properties.Resources.MonarchAusente1;
                            break;
                        }
                    case 3:
                        {
                            if (rank == 0)
                                PB_IMG_Usuario.Image = Properties.Resources.VillensDesconectado;
                            if (rank == 1)
                                PB_IMG_Usuario.Image = Properties.Resources.KnightDesconectado;
                            if (rank == 2)
                                PB_IMG_Usuario.Image = Properties.Resources.NobleDesconectado;
                            if (rank == 3)
                                PB_IMG_Usuario.Image = Properties.Resources.MonarchDesconect;
                            break;
                        }
                }
            }


        }     
        
        private void RefreshFriendStatus()
        {
            Conversation conversation = list_chats[indexMaximo].GetConversacion();

            int rank = 0;

            if (conversation.participants.Count > 1)
            {
                rank = Log.loggedUser.points;
                bool isAnybodyThere = StatusConversation();

                switch (rank)
                {
                    case 0:
                        {
                            if (isAnybodyThere)
                                PB_IMG_Usuario.Image = Properties.Resources.Villens;
                            else
                                PB_IMG_Usuario.Image = Properties.Resources.VillensDesconectado;
                            break;
                        }
                    case 1:
                        {
                            if (isAnybodyThere)
                                PB_IMG_Usuario.Image = Properties.Resources.Knight1;
                            else
                                PB_IMG_Usuario.Image = Properties.Resources.KnightDesconectado;
                            break;
                        }
                    case 2:
                        {
                            {
                                if (isAnybodyThere)
                                    PB_IMG_Usuario.Image = Properties.Resources.Noble1;
                                else
                                    PB_IMG_Usuario.Image = Properties.Resources.NobleAusente;
                                break;
                            }
                        }
                    case 3:
                        {
                            {
                                if (isAnybodyThere)
                                    PB_IMG_Usuario.Image = Properties.Resources.Monarch1;
                                else
                                    PB_IMG_Usuario.Image = Properties.Resources.MonarchDesconect;
                                break;
                            }
                        }
                }
            }
            else
            {
                rank = conversation.participants[0].points;

                String displayProfile = conversation.GetNameUsuarios();

                LB_INFO_NOMBRE.Text = displayProfile;

                switch (conversation.participants[0].status)
                {
                    case 1:
                        {
                            if (rank == 0)
                                PB_IMG_Usuario.Image = Properties.Resources.Villens;
                            if (rank == 1)
                                PB_IMG_Usuario.Image = Properties.Resources.Knight1;
                            if (rank == 2)
                                PB_IMG_Usuario.Image = Properties.Resources.Noble1;
                            if (rank == 3)
                                PB_IMG_Usuario.Image = Properties.Resources.Monarch1;
                            break;
                        }
                    case 2:
                        {
                            if (rank == 0)
                                PB_IMG_Usuario.Image = Properties.Resources.VillensAusente;
                            if (rank == 1)
                                PB_IMG_Usuario.Image = Properties.Resources.KnightAusente;
                            if (rank == 2)
                                PB_IMG_Usuario.Image = Properties.Resources.NobleAusente;
                            if (rank == 3)
                                PB_IMG_Usuario.Image = Properties.Resources.MonarchAusente1;
                            break;
                        }
                    case 3:
                        {
                            if (rank == 0)
                                PB_IMG_Usuario.Image = Properties.Resources.VillensDesconectado;
                            if (rank == 1)
                                PB_IMG_Usuario.Image = Properties.Resources.KnightDesconectado;
                            if (rank == 2)
                                PB_IMG_Usuario.Image = Properties.Resources.NobleDesconectado;
                            if (rank == 3)
                                PB_IMG_Usuario.Image = Properties.Resources.MonarchDesconect;
                            break;
                        }
                }
            }

        }

        private bool StatusConversation()
        {

            foreach (UserProfile user in list_chats[indexMaximo].GetConversacion().participants)
            {
                if (user.status == 1)
                {
                    return true;
                }
            }

            return false;
        }

        private void IC_EMOTICON_Click(object sender, EventArgs e)
        {
            PN_EMOTICONES.Visible = true;
            PN_EMOTICONES.BringToFront();
        }

        private void IC_ZUMBIDO_Click(object sender, EventArgs e)
        {
            SendBuzz();
        }

        private void PerformLocalBuzz()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            // getting root path
            string rootLocation = typeof(Program).Assembly.Location;

            rootLocation = rootLocation.Replace("ICELIGHT_POI.exe", "bell.wav");

            player.SoundLocation = rootLocation;
            player.Play();

            ZOOM();
        }

        private void ZOOM()
        {
            if (this.InvokeRequired)
            {
                Zumbido z = new Zumbido(ZOOM);
                this.Invoke(z);

            }
            else
            {
                zumb();
            }
        }

        public void zumb()
        {
            //MessageBox.Show("kek");
            Point WinLoc = default(Point);
            Point WinLocDef = default(Point);

            WinLocDef = this.Location;
            WinLoc = this.Location;

            for (int i = 0; i <= 50; i++)
            {
                for (int x = 0; x <= 4; x++)
                {
                    switch (x)
                    {
                        case 0:
                            WinLoc.X = WinLocDef.X + 10;
                            break;
                        case 1:
                            WinLoc.X = WinLocDef.X - 10;
                            break;
                        case 2:
                            WinLoc.Y = WinLocDef.Y - 10;
                            break;
                        case 3:
                            WinLoc.Y = WinLocDef.Y + 10;
                            break;
                        case 4:
                            WinLoc = WinLocDef;
                            break;
                    }
                    this.Location = WinLoc;
                }
                //Me.Refresh() // if needed for more form refresh event
            }
            this.Location = WinLocDef;
            this.Refresh();
        }

        private void BTN_E_Click(object sender, EventArgs e)
        {
            Button BTN_emoticono = (Button)sender;
            Bitmap emoticon = new Bitmap(BTN_emoticono.BackgroundImage);
            Bitmap resize = new Bitmap(emoticon, new Size(emoticon.Width / 3, emoticon.Height / 3));
            Clipboard.SetDataObject(resize);
            DataFormats.Format myFormat = DataFormats.GetFormat(DataFormats.Bitmap);
            if (TB_CHAT_INICIO.CanPaste(myFormat))
            {
                TB_CHAT_INICIO.Paste(myFormat);
            }
        }
        private void TB_CHAT_INICIO_TextChanged_1(object sender, EventArgs e)
        {
            int a = 0;
        }

        private void TB_CHAT_INICIO_Click(object sender, EventArgs e)
        {
            PN_EMOTICONES.Visible = false;
        }

        private void CargarEmojis()
        {
            emotes = new List<Emote>();

            Button[] emo = { BTN_E_2, BTN_E_3, BTN_E_4, BTN_E_5, BTN_E_7, BTN_E_8, BTN_E_9, BTN_E_10, BTN_E_12, BTN_E_13,
            BTN_E_14,BTN_E_15, BTN_E_17,BTN_E_18,BTN_E_19,BTN_E_20};

            foreach (Button bm in emo)
            {
                Bitmap emoticon = new Bitmap(bm.BackgroundImage);
                Bitmap resize = new Bitmap(emoticon, new Size(emoticon.Width / 3, emoticon.Height / 3));
                Clipboard.SetDataObject(resize);
                DataFormats.Format myFormat = DataFormats.GetFormat(DataFormats.Bitmap);
                if (TB_CHAT_INICIO.CanPaste(myFormat))
                {
                    TB_CHAT_INICIO.Paste(myFormat);
                }
                string sepi = @"\cf1\f0\fs17";
                string sepf = @"\par";
                int indi = TB_CHAT_INICIO.Rtf.IndexOf(sepi);
                indi += sepi.Length;
                int indf = TB_CHAT_INICIO.Rtf.LastIndexOf(sepf);
                if (TB_CHAT_INICIO.Text != " ")
                    indi++;
                string prueba = TB_CHAT_INICIO.Rtf.Substring(indi, indf - indi);
                //\cf1\f0\fs17
                string imagenrtf = prueba.Substring(prueba.IndexOf("{"));
                emotes.Add(new Emote(imagenrtf, bm.Tag.ToString()));

                TB_CHAT_INICIO.Clear();
            }
        }

        
        //STREAMING
        public void ReceiveStream(Byte[] byteImg)
        {
            //haystreaming
            if (haystreaming == true)
            {
                imgchat = byteArrayToImage(byteImg);
                PB_OTRO.SizeMode = PictureBoxSizeMode.StretchImage;
                PB_OTRO.Image = imgchat;
            }
        }       //RECIBE EL ARREGLO DE BYTES Y LO CONVIERTE A IMAGEN, LA CUAL SE PASA AL PICTUREBOX QUE MUESTRA AL OTRO USUARIO

        public Byte[] imageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }//CONVIERTE LA IMAGEN A BYTE[]

        public Image byteArrayToImage(Byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }       //CONVIERTE EL BYTE[] A IMAGEN

        public void GetStream()
        {
            //startstream
            while (haystreaming)     //SI HAY UN STREAMING, RECIBE LA IMAGEN COMO ARREGLO DE BYTES
            {
                try
                {
                    // ipServer , Puerto en el servidor 
                    int portini = 0;
                    if (startedCall)
                    {
                        portini = 2000;
                    }
                    else
                    {
                        portini = 3000;
                    }
                    IPEndPoint ip = new IPEndPoint(IPAddress.Parse(Log.ServerIP), portini);
                    Byte[] receiveBytes = socketrecibevideo.Receive(ref ip);
                    //ReceiveStream(receiveBytes);
                    this.Invoke(new DGetTv(ReceiveStream), receiveBytes);
                }
                catch (Exception e)
                {
                }
                Thread.Sleep(10);
            }
        }       //FUNCIÓN DEL HILO QUE CHECA SI HAY UN STREAMING ACTIVO

        public void SendStream(Image img)
        {
            try
            {
                int portini = 0;
                if (startedCall)
                {
                    portini = 3000;
                }
                else
                {
                    portini = 2000;
                }
                UdpClient tempSocket = new UdpClient();
                Byte[] sendBytes = imageToByteArray(img);
                IPEndPoint tempipep = new IPEndPoint(IPAddress.Parse(Log.ServerIP), portini);
                tempSocket.Send(sendBytes, sendBytes.Length, tempipep);
                StreamWriter sr = new StreamWriter("succSend.txt", true);
                sr.WriteLine(sendBytes.ToString());
                sr.Close();
                tempSocket.Close();
            }
            catch (Exception e)
            {
                StreamWriter sr = new StreamWriter("errorSend.txt");
                sr.WriteLine(e.ToString());
                sr.Close();
            }
            //}
        }       //ENVÍA EL BYTE[] EEN QUE SE TRANSFORMO LA IMAGEN E INDICA QUE UN STREAMING HA COMENZADO


        // OBTENER EL VIDEO/IMAGEN DE LA CAMARA

        public void CargarDispositivos(FilterInfoCollection Dispositivos)
        {
            for (int i = 0; i < Dispositivos.Count; i++) ;

            COB_LLAMADA.Items.Add(Dispositivos[0].Name.ToString());
            COB_LLAMADA.Text = COB_LLAMADA.Items[0].ToString();
        }

        public void BuscarDispositivos()
        {
            try
            {
                DispositivoDeVideo = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (DispositivoDeVideo.Count == 0)
                {
                    ExisteDispositivo = false;
                }
                else
                {
                    ExisteDispositivo = true;
                    CargarDispositivos(DispositivoDeVideo);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No tienes cámara");
            }
        }

        public void TerminarFuenteDeVideo()
        {
            if (!(FuenteDeVideo == null))
                if (FuenteDeVideo.IsRunning)
                {
                    FuenteDeVideo.SignalToStop();
                    FuenteDeVideo = null;
                }

            //CheckRequirementsLevelUp();
        }

        public void Video_NuevoFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Bitmap Imagen = (Bitmap)eventArgs.Frame.Clone();
                PB_YO.SizeMode = PictureBoxSizeMode.StretchImage;
                PB_YO.Image = Imagen;
                imgstream = Imagen;
            }
            catch(Exception e)
            {
                StreamWriter sr = new StreamWriter("errorNuevoFrame.txt");
                sr.WriteLine(e.ToString());
                sr.Close();
            }
        }

        private void btn_selectcamera_Click(object sender, EventArgs e)
        {

        }       //SELECCIONAR LA CAMARA DEL COMBOBOX

        private void btn_beginstream_Click(object sender, EventArgs e)
        {

        }       //INICIA EL STREAMING (BANDERAS)

        private void tim_stream_Tick(object sender, EventArgs e)
        {

        }       //(esta tambien)
 

        public void iniciarLlamada()
        {
            panel_VChat.Visible = true;
            panel_VChat.BringToFront();
            PB_YO.Visible = true;
            PB_OTRO.Visible = true;
        }

        private void BTN_OK_Click(object sender, EventArgs e)
        {

            if (BTN_OK.Text == "Firmar")
            {
                if (ExisteDispositivo)
                {
                    FuenteDeVideo = new VideoCaptureDevice(DispositivoDeVideo[COB_LLAMADA.SelectedIndex].MonikerString);
                    FuenteDeVideo.NewFrame += new NewFrameEventHandler(Video_NuevoFrame);
                    FuenteDeVideo.Start();
                    BTN_OK.Text = "X";
                    COB_LLAMADA.Enabled = false;
                    BTN_INICIAR.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Error: No se encuenta el Dispositivo");
                }
            }
            else
            {
                if (FuenteDeVideo.IsRunning)
                {
                    TerminarFuenteDeVideo();
                    PB_YO.Image = null;
                    BTN_OK.Text = "Firmar";
                    BTN_INICIAR.Enabled = false;
                    COB_LLAMADA.Enabled = true;
                }
            }
        }
        private void BTN_INICIAR_Click(object sender, EventArgs e)
        {
            if (haystreaming == false)
            {
                TIC.Enabled = true;
                haystreaming = true;
                TIC.Start();
            }
            else
            {
                TIC.Enabled = false;
                haystreaming = false;
            }
        }

        private void TIC_Tick(object sender, EventArgs e)
        {
            if (haystreaming == true)
            {
                videoCallAchieved = true;
                if (imgstream != null)
                {
                    Image resize = new Bitmap(imgstream, imgstream.Width / 2, imgstream.Height / 2);
                    SendStream(resize);
                    if (tv_chat == null)
                    {
                        tv_chat = new Thread(GetStream);
                        tv_chat.Start();
                    }
                }
            }
            else
            {
                tv_chat = null;
            }
        }

        private void BTN_RECOMPENSA_EMO_Click(object sender, EventArgs e)
        {
            Rangos re = new Rangos(Log.loggedUser);

            re.Show();
        }

        private void panel_VChat_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void ReceiveUserFriends(Socket soc, byte[] data)
        {
            int recv;
            string listLenghtString;
            int listLenght;

            //Recibir Cuantos Contactos contactos tiene que recibir
            recv = soc.Receive(data);
            listLenghtString = Encoding.ASCII.GetString(data, 0, recv);
            listLenght = Int32.Parse(listLenghtString);
            soc.Send(Encoding.ASCII.GetBytes("Yes" + listLenghtString));

            for (int i = 0; i < listLenght; i++)
            {
                data = new byte[1024];
                recv = soc.Receive(data);
                string[] receivedStrings = Encoding.ASCII.GetString(data, 0, recv).Split('$');
                UserProfile auxFriend = new UserProfile(Int32.Parse(receivedStrings[0]), receivedStrings[1],
                            receivedStrings[2], receivedStrings[3], Int32.Parse(receivedStrings[4]),
                            Int32.Parse(receivedStrings[5]));

                list_Friends.Add(auxFriend);
                soc.Send(Encoding.ASCII.GetBytes("Yes" + i.ToString()));
            }
        }

        private void ReceiveUserConversations(Socket soc, byte[] data)
        {
            int recv;
            string listLenghtString;
            int listLenght;

            data = new byte[10];
            recv = soc.Receive(data);
            listLenghtString = Encoding.ASCII.GetString(data, 0, recv);
            listLenght = Int32.Parse(listLenghtString);
            soc.Send(Encoding.ASCII.GetBytes("Listo Num Conv"));

            for (int i = 0; i < listLenght; i++)
            {
                data = new byte[1024];
                recv = soc.Receive(data);
                string[] datos = Encoding.ASCII.GetString(data, 0, recv).Split('#');
                //Conversacion conversacion = new Conversacion(Log.usuario, Int32.Parse(datos[0]));
                Conversation conversation = new Conversation(Int32.Parse(datos[0]), datos[1]);

                //Agregar a las Conversaciones del Usuario
                data = new byte[5];
                soc.Send(Encoding.ASCII.GetBytes("UsuariosYes"));

                list_Conversations.Add(conversation);
            }
        }

        private void ReceiveUserGuests(Socket soc, byte[] data)
        {
            int recv;
            string listLenghtString;
            int listLenght;

            data = new byte[10];
            recv = soc.Receive(data);
            listLenghtString = Encoding.ASCII.GetString(data, 0, recv);
            listLenght = Int32.Parse(listLenghtString);
            soc.Send(Encoding.ASCII.GetBytes("Listo Num participantes"));

            for (int i = 0; i < listLenght; i++)
            {
                data = new byte[1024];
                recv = soc.Receive(data);
                string[] datos = Encoding.ASCII.GetString(data, 0, recv).Split('#');

                UserProfile guest = new UserProfile(Int32.Parse(datos[0]), datos[1], datos[2],
                    datos[3], Int32.Parse(datos[4]), Int32.Parse(datos[5]));

                //Agregar a las Conversaciones del Usuario
                data = new byte[5];
                soc.Send(Encoding.ASCII.GetBytes("MessageYes"));

                if(Log.loggedUser.idUser != Int32.Parse(datos[0]))
                {
                    list_Guests.Add(guest);
                    list_Conversations.Find(x => x.idConversation == Int32.Parse(datos[6])).participants.Add(guest);
                }

            }
        }

        private void ReceiveUserMessages(Socket soc, byte[] data)
        {
            int recv;
            string listLenghtString;
            int listLenght;

            data = new byte[10];
            recv = soc.Receive(data);
            listLenghtString = Encoding.ASCII.GetString(data, 0, recv);
            listLenght = Int32.Parse(listLenghtString);
            soc.Send(Encoding.ASCII.GetBytes("Listo Num Messages"));

            for (int i = 0; i < listLenght; i++)
            {
                data = new byte[1024];
                recv = soc.Receive(data);
                string[] datos = Encoding.ASCII.GetString(data, 0, recv).Split('#');

                Messaged message = null;

                try
                {
                    message = new Messaged(Int32.Parse(datos[0]), Int32.Parse(datos[1]), datos[2], DateTime.Parse(datos[3]), Int32.Parse(datos[4]), list_Friends, Log.loggedUser);
                }
                catch (Exception ex)
                {
                    string[] dateTimeHandler = Regex.Split(datos[3], " ");

                    if(dateTimeHandler[2] == "a.m.")
                    {
                        dateTimeHandler[2] = "a. m.";
                    }
                    else if (dateTimeHandler[2] == "p.m.")
                    {
                        dateTimeHandler[2] = "p. m.";
                    }
                    datos[3] = dateTimeHandler[0] + " " + dateTimeHandler[1] + " " + dateTimeHandler[2];
                    message = new Messaged(Int32.Parse(datos[0]), Int32.Parse(datos[1]), datos[2], DateTime.Parse(datos[3]), Int32.Parse(datos[4]), list_Friends, Log.loggedUser);
                }

                //Agregar a las Conversaciones del Usuario
                data = new byte[5];
                soc.Send(Encoding.ASCII.GetBytes("MessageYes"));

                list_Messages.Add(message);

                list_Conversations.Find(x => x.idConversation == Int32.Parse(datos[4])).messages.Add(message);
            }
        }

        public void CheckRequirementsLevelUp()
        {
            //Log.loggedUser
            int points = 0;

            if ((list_Messages.Count > 19) && (list_Friends.Count > 5) && (haystreaming) && (Log.loggedUser.points == 2))
            {
                points = 3;
                SendProfileLevelUp(points);
            }

            if ((list_Messages.Count > 9) && (list_Friends.Count > 1) && (Log.loggedUser.points == 1))
            {
                points = 2;
                SendProfileLevelUp(points);
            }

            if ((list_Messages.Count > 4) && (Log.loggedUser.points == 0))
            {
                points = 1;
                SendProfileLevelUp(points);
            }

        }

        private void pb_Send_Click(object sender, EventArgs e)
        {
            EnviarMensaje();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void IC_ARCHIVO_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            FN_IC_LLAMADA();
            SendRequestCall();
        }

        private void CB_LLAMADA_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void pb_Invocar_Click(object sender, EventArgs e)
        {
            if (haystreaming == false)
            {
                TIC.Enabled = true;
                haystreaming = true;
                TIC.Start();
            }
            else
            {
                TIC.Enabled = false;
                haystreaming = false;
            }
        }

        private void BTN_INICIAR_Click_1(object sender, EventArgs e)
        {
            if (haystreaming == false)
            {
                TIC.Enabled = true;
                haystreaming = true;
                TIC.Start();
            }
            else
            {
                TIC.Enabled = false;
                haystreaming = false;
            }
        }

        #region Senders

        public void SendRequestCall()
        {
            if (list_Conversations[indexMaximo].participants.Count == 1)
                if (soc != null)
                {
                    soc.Send(Encoding.ASCII.GetBytes("SERVER$ASKCHAT$" + Log.loggedUser.idUser.ToString() + "$" + list_Conversations[indexMaximo].participants[0].idUser));
                }
        }

        public void SendChangeProfileState(int status)
        {
            if (soc != null)
            {
                soc.Send(Encoding.ASCII.GetBytes("SERVER$CHANGESTATE$" + Log.loggedUser.idUser.ToString() + "$" + status.ToString()));
            }

            Log.loggedUser.status = status;
        }

        public void SendProfileLevelUp(int points)
        {
            if (soc != null)
            {
                soc.Send(Encoding.ASCII.GetBytes("SERVER$LEVELUP$" + Log.loggedUser.idUser.ToString() + "$" + points.ToString()));
            }

            Log.loggedUser.points = points;
        }

        public void SendBuzz()
        {
            if (soc != null)
            {
                soc.Send(Encoding.ASCII.GetBytes("SERVER$BUZZ$" + list_Conversations[indexMaximo].idConversation.ToString()));
            }
        }

        #endregion

        public void openLoginForm()
        {
            if (loginPtr != null)
            {
                loginPtr.Show();
                logHidden = false;
            }
        }

        private void btn_Dispel_Click(object sender, EventArgs e)
        {
            STOP_CALL();
        }

        public void STOP_CALL()
        {
            TIC.Enabled = false;
            haystreaming = false;
            TIC.Stop();

            PB_OTRO.Image = null;
        }
    }
}

