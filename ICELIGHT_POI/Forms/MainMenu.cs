using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICELIGHT_POI
{
    public partial class MainMenu : Form
    {
        public static INICIO inicio;

        public MainMenu()
        {
            InitializeComponent();
            lbl_Username.Text = Log.loggedUser.username;

            SetStatusLabel();

            SetProfileImage();
        }

        private void SetStatusLabel()
        {
            switch (Log.loggedUser.status)
            {
                case 1:
                    lbl_Status.Text = "Conectado";
                    break;
                case 2:
                    lbl_Status.Text = "Ausente";
                    break;
                case 3:
                    lbl_Status.Text = "Desconectado";
                    break;
            }
        }

        public void SetProfileImage()
        {
            int rank = Log.loggedUser.points;
            switch (Log.loggedUser.status)
            {
                case 1:
                    {
                        if (rank == 0)
                            pb_Image.Image = Properties.Resources.Villens;
                        if (rank == 1)
                            pb_Image.Image = Properties.Resources.Knight1;
                        if (rank == 2)
                            pb_Image.Image = Properties.Resources.Noble1;
                        if (rank == 3)
                            pb_Image.Image = Properties.Resources.Monarch1;
                        break;
                    }
                case 2:
                    {
                        if (rank == 0)
                            pb_Image.Image = Properties.Resources.VillensAusente;
                        if (rank == 1)
                            pb_Image.Image = Properties.Resources.KnightAusente;
                        if (rank == 2)
                            pb_Image.Image = Properties.Resources.NobleAusente;
                        if (rank == 3)
                            pb_Image.Image = Properties.Resources.MonarchAusente1;
                        break;
                    }
                case 3:
                    {
                        if (rank == 0)
                            pb_Image.Image = Properties.Resources.VillensDesconectado;
                        if (rank == 1)
                            pb_Image.Image = Properties.Resources.KnightDesconectado;
                        if (rank == 2)
                            pb_Image.Image = Properties.Resources.NobleDesconectado;
                        if (rank == 3)
                            pb_Image.Image = Properties.Resources.MonarchDesconect;
                        break;
                    }
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            //inicio.FN_IC_CONF();
        }

        private void IC_MENSAJE_Click(object sender, EventArgs e)
        {
            inicio.FN_IC_MENSAJE();
        }

        private void IC_PERFIL_Click(object sender, EventArgs e)
        {
            inicio.FN_IC_MENSAJE();
        }

        private void IC_LLAMADA_Click(object sender, EventArgs e)
        {
            inicio.FN_IC_LLAMADA();
            inicio.SendRequestCall();
        }

        private void IC_CONSOLA_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            inicio.FN_IC_MENSAJE();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            inicio.FN_IC_PERFL();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            inicio.FN_IC_PERFL();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            inicio.FN_IC_PERFL();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            inicio.FN_IC_LLAMADA();
            inicio.SendRequestCall();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            inicio.FN_IC_LLAMADA();
            inicio.SendRequestCall();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }
    }
}
