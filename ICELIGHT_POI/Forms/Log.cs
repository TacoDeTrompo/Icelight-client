using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Client;

namespace ICELIGHT_POI
{
    public partial class Log : Form
    {
        InicioSesion Login;
        Registro Register;

        internal static UserProfile loggedUser;
        internal static Usuario usuario;
        internal static Socket socket;
        internal static string ServerIP = "192.168.0.10";//"192.168.1.75";//20 Escritorio 15 Lap"192.168.0.20"192.168.43.213 Haus192.168.1.156

        public Log()
        {
            InitializeComponent();

            Login = new InicioSesion();
            Register = new Registro();

            pnlMain.Controls.Add(Login);
            pnlMain.Controls.Add(Register);

            Login.Location = new Point(this.Width / 2 - Login.Width / 2, this.Height / 2 - Login.Height / 2);
            Register.Location = Login.Location;

            Login.Dock = DockStyle.Fill;
            Register.Dock = DockStyle.Fill;

            Login.BringToFront();
            Register.Visible = false;
        }

        private void Log_Load(object sender, EventArgs e)
        {
            InicioSesion.log = this;
            Registro.log = this;
        }

        public void BtnLoginClick()
        {
            if (connect(true))
            {
                this.Hide();
                INICIO ini = new INICIO(this);
                ini.Show();
                //this.Close();
            }
        }

        public void BtnRegisterClick()
        {
            if (connect(false))
            {
                ToggleLoginRegisterVisibility();
            }
        }

        public void ToggleLoginRegisterVisibility()
        {
            Login.Visible = !Login.Visible;
            Register.Visible = !Register.Visible;
        }

        public bool connect(bool login)
        {
            if (!IsLogValid(login))
                return false;
                
            byte[] data = new byte[1024];
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ServerIP), 9050);

            //Declaración del socket para conectar con el servidor
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                server.Connect(ipep);
                socket = server;

                int receiveRawData = 0;

                if (login)
                {
                    data = Encoding.ASCII.GetBytes("LOGIN#" + Login.tbUsername.Text + "#" + Login.tbPassword.Text);
                    server.Send(data, data.Length, SocketFlags.None);

                    data = new byte[1024];

                    receiveRawData = server.Receive(data, SocketFlags.None);
                    string aceptado = Encoding.ASCII.GetString(data, 0, receiveRawData);

                    //[1] - idUser
                    //[2] - name
                    //[3] - username
                    //[4] - email
                    //[5] - status
                    //[6] - points
                    string[] receivedStrings = aceptado.Split('#'); 

                    if (receivedStrings[0] == "LoginCorrecto")
                    {
                        usuario = new Usuario(Int32.Parse(receivedStrings[1]), receivedStrings[2]);

                        loggedUser = new UserProfile(Int32.Parse(receivedStrings[1]), receivedStrings[2],
                            receivedStrings[3], receivedStrings[4], Int32.Parse(receivedStrings[5]),
                            Int32.Parse(receivedStrings[6]));

                        if (receivedStrings.Length > 3)
                        {
                            usuario.SetEmail(receivedStrings[4]);
                            usuario.setPoints(Int32.Parse(receivedStrings[6]));
                        }

                        server.Send(Encoding.ASCII.GetBytes(receivedStrings[3] + " conectado."));
                        socket = server;
                    }
                    else
                    {
                        MessageBox.Show("Usuario o contraseña incorrectos.", "Nuntius");
                        socket = null;
                    }
                }
                else
                {
                    data = Encoding.ASCII.GetBytes("REG#" + Register.tbName.Text + "#" + Register.tbUsername.Text + "#" + Register.tbEmail.Text + "#" + Register.tbPassword.Text);
                    server.Send(data, data.Length, SocketFlags.None);
                    data = new byte[1024];
                    receiveRawData = server.Receive(data, SocketFlags.None);
                    string registrado = Encoding.ASCII.GetString(data, 0, receiveRawData);

                    MessageBox.Show(registrado, "Nuntius");
                }
            }
            catch (SocketException excep)
            {
                MessageBox.Show("Imposible conectar con el servidor.", "Nuntius");
                socket = null;
            }
            if (socket == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        //Validaciones antes de mandar el login/registro al servidor.
        public bool IsLogValid(bool login)
        {
            if (Login.tbUsername.Text == "" && login == true)
            {
                MessageBox.Show("No se permiten registros vacios.", "Nuntius");
                return false;
            }

            if ((Register.tbName.Text == "" || Register.tbUsername.Text == "" || Register.tbEmail.Text == "" ||
                Register.tbPassword.Text == "") && login == false)
            {
                MessageBox.Show("No se permiten registros vacios.", "Nuntius");
                return false;
            }

            return true;
        }
    }
}
