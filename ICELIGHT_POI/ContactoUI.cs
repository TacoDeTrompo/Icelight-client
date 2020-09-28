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
    //internal enum statusContacto
    //{
    //    conectado,
    //    desconectado,
    //    ausente
    //}
    class ContactoUI
    {
        int status;
        int rank;

        internal const int Height=43;
        private List<UserProfile> usuarios;
        private Conversation conversation;
        
        //private statusContacto status;
        private PictureBox Circulo;

        public static Image[] imConected;
        public static Image[] imDisconected;
        public static Image[] imAbsent;

        private PictureBox fotoUsuario;
        private PictureBox decor;
        private Label nombreUsuario;
        private Panel Padre;
        internal Panel fondo;

        public UserProfile getUsuario()
        {
            return usuarios[0];
        }
        public UserProfile getUsuario(int i)
        {
            return usuarios[i];
        }

        public ContactoUI(List<UserProfile> users, Point spawn, Panel pan)
        {
            imConected = new Image[4];
            imAbsent = new Image[4];
            imDisconected = new Image[4];

            Padre = pan;
            this.usuarios = new List<UserProfile>();
            this.usuarios.AddRange(users);

            imConected[0] = Properties.Resources.Villens;
            imConected[1] = Properties.Resources.Knight1;
            imConected[2] = Properties.Resources.Noble1;
            imConected[3] = Properties.Resources.Monarch1;

            imAbsent[0] = Properties.Resources.VillensAusente;
            imAbsent[1] = Properties.Resources.KnightAusente;
            imAbsent[2] = Properties.Resources.NobleAusente;
            imAbsent[3] = Properties.Resources.MonarchAusente1;

            imDisconected[0] = Properties.Resources.VillensDesconectado;
            imDisconected[1] = Properties.Resources.KnightDesconectado;
            imDisconected[2] = Properties.Resources.NobleDesconectado;
            imDisconected[3] = Properties.Resources.MonarchDesconect;

            fondo = new Panel() { Size = new Size(160 + 6 + 43, Height), Location = spawn, BackColor = Padre.BackColor };

            Circulo = new PictureBox()
            {
                Location = new Point(0, 0),
                Image = imConected[0],
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(43, 43),
                BackColor = Color.Transparent
            };

            decor = new PictureBox()
            {
                Location = new Point(Circulo.Location.X + Circulo.Size.Width + 6, 31),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(160, 12),
                Image = Properties.Resources.RedLine
            };

            fotoUsuario = new PictureBox()
            {
                Location = new Point(0, 0),
                Size = Circulo.Size,
                Image = usuarios[0].photo,
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            string nombreEntero = usuarios[0].username;

            if (usuarios.Count > 1)
            {
                for (int i = 1; i < usuarios.Count; i++)
                    nombreEntero += "," + usuarios[i].username;

                rank = Log.loggedUser.points;
                bool isAnybodyThere = StatusConversation();

                switch (rank)
                {
                    case 0:
                        {
                            if (isAnybodyThere)
                                Circulo.Image = Properties.Resources.Villens;
                            else
                                Circulo.Image = Properties.Resources.VillensDesconectado;
                            break;
                        }
                    case 1:
                        {
                            if (isAnybodyThere)
                                Circulo.Image = Properties.Resources.Knight1;
                            else
                                Circulo.Image = Properties.Resources.KnightDesconectado;
                            break;
                        }
                    case 2:
                        {
                                if (isAnybodyThere)
                                Circulo.Image = Properties.Resources.Noble1;
                                else
                                Circulo.Image = Properties.Resources.NobleAusente;
                                break;
                        }
                    case 3:
                        {
                                if (isAnybodyThere)
                                Circulo.Image = Properties.Resources.Monarch1;
                                else
                                Circulo.Image = Properties.Resources.MonarchDesconect;
                                break;
                        }
                }
            }
            else
            {
                rank = usuarios[0].points;

                switch (usuarios[0].status)
                {
                    case 1:
                        {
                            if (rank == 0)
                                Circulo.Image = Properties.Resources.Villens;
                            if (rank == 1)
                                Circulo.Image = Properties.Resources.Knight1;
                            if (rank == 2)
                                Circulo.Image = Properties.Resources.Noble1;
                            if (rank == 3)
                                Circulo.Image = Properties.Resources.Monarch1;
                            break;
                        }
                    case 2:
                        {
                            if (rank == 0)
                                Circulo.Image = Properties.Resources.VillensAusente;
                            if (rank == 1)
                                Circulo.Image = Properties.Resources.KnightAusente;
                            if (rank == 2)
                                Circulo.Image = Properties.Resources.NobleAusente;
                            if (rank == 3)
                                Circulo.Image = Properties.Resources.MonarchAusente1;
                            break;
                        }
                    case 3:
                        {
                            if (rank == 0)
                                Circulo.Image = Properties.Resources.VillensDesconectado;
                            if (rank == 1)
                                Circulo.Image = Properties.Resources.KnightDesconectado;
                            if (rank == 2)
                                Circulo.Image = Properties.Resources.NobleDesconectado;
                            if (rank == 3)
                                Circulo.Image = Properties.Resources.MonarchDesconect;
                            break;
                        }
                }
            }

            nombreUsuario = new Label()
            {
                ForeColor = Color.White,
                Text = nombreEntero,
                Location = new Point(Circulo.Location.X + Circulo.Size.Width + 6, 12),
                MaximumSize = new Size(160, 17),
                MinimumSize = new Size(160, 17),

            };
            nombreUsuario.Font = new Font(nombreUsuario.Font.FontFamily, 9.5f);

            //quitar
            Circulo.Location = new Point(100, 100);

            Padre.Controls.Add(fondo);
            fondo.Controls.Add(Circulo);
            fondo.Controls.Add(decor);
            fondo.Controls.Add(nombreUsuario);
            fondo.Controls.Add(fotoUsuario);

            fondo.Click += Click_Contacto;
            nombreUsuario.Click += Click_Contacto;
            decor.Click += Click_Contacto;
            Circulo.Click += Click_Contacto;
            fotoUsuario.Click += Click_Contacto;

            Circulo.Location = new Point(0, 0);
            fondo.Show();
            nombreUsuario.Show();
            decor.Show();
            Circulo.Show();
            fotoUsuario.Show();
        }

        public void setConversation(Conversation conversation)
        {
            this.conversation = conversation;
        }

        private void Click_Contacto(object obj, EventArgs e)
        {            
            INICIO.CargarMensajes(conversation);
        }

        public void RefreshProfileImage()
        {

            if (usuarios.Count > 1)
            {
                rank = Log.loggedUser.points;
                bool isAnybodyThere = StatusConversation();

                switch (rank)
                {
                    case 0:
                        {
                            if (isAnybodyThere)
                                Circulo.Image = Properties.Resources.Villens;
                            else
                                Circulo.Image = Properties.Resources.VillensDesconectado;
                            break;
                        }
                    case 1:
                        {
                            if (isAnybodyThere)
                                Circulo.Image = Properties.Resources.Knight1;
                            else
                                Circulo.Image = Properties.Resources.KnightDesconectado;
                            break;
                        }
                    case 2:
                        {
                            if (isAnybodyThere)
                                Circulo.Image = Properties.Resources.Noble1;
                            else
                                Circulo.Image = Properties.Resources.NobleAusente;
                            break;
                        }
                    case 3:
                        {
                            if (isAnybodyThere)
                                Circulo.Image = Properties.Resources.Monarch1;
                            else
                                Circulo.Image = Properties.Resources.MonarchDesconect;
                            break;
                        }
                }
            }
            else
            {
                rank = usuarios[0].points;

                switch (usuarios[0].status)
                {
                    case 1:
                        {
                            if (rank == 0)
                                Circulo.Image = Properties.Resources.Villens;
                            if (rank == 1)
                                Circulo.Image = Properties.Resources.Knight1;
                            if (rank == 2)
                                Circulo.Image = Properties.Resources.Noble1;
                            if (rank == 3)
                                Circulo.Image = Properties.Resources.Monarch1;
                            break;
                        }
                    case 2:
                        {
                            if (rank == 0)
                                Circulo.Image = Properties.Resources.VillensAusente;
                            if (rank == 1)
                                Circulo.Image = Properties.Resources.KnightAusente;
                            if (rank == 2)
                                Circulo.Image = Properties.Resources.NobleAusente;
                            if (rank == 3)
                                Circulo.Image = Properties.Resources.MonarchAusente1;
                            break;
                        }
                    case 3:
                        {
                            if (rank == 0)
                                Circulo.Image = Properties.Resources.VillensDesconectado;
                            if (rank == 1)
                                Circulo.Image = Properties.Resources.KnightDesconectado;
                            if (rank == 2)
                                Circulo.Image = Properties.Resources.NobleDesconectado;
                            if (rank == 3)
                                Circulo.Image = Properties.Resources.MonarchDesconect;
                            break;
                        }
                }
            }
        }

        //public void setStatus(int status)
        //{
        //    if (this.status != status)
        //    {
        //        this.status = status;
        //        cambiarStatus();
        //    }
        //}

        //private void cambiarStatus()
        //{
        //    switch (status)
        //    {
        //        case 1:
        //            {
        //                if (rank == 0)
        //                    PB_IMG_Usuario.Image = Properties.Resources.Villens;
        //                if (rank == 1)
        //                    PB_IMG_Usuario.Image = Properties.Resources.Knight1;
        //                if (rank == 2)
        //                    PB_IMG_Usuario.Image = Properties.Resources.Noble1;
        //                if (rank == 3)
        //                    PB_IMG_Usuario.Image = Properties.Resources.Monarch1;
        //                Circulo.Image = conectado;
        //                break;
        //            }
        //        case 2:
        //            Circulo.Image = ausente;
        //            break;
        //        case 3:
        //            Circulo.Image = desconectado;
        //            break;
        //    }
        //}

        private bool StatusConversation()
        {
            foreach (UserProfile user in usuarios)
            {
                if (user.status == 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
