using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client;

namespace ICELIGHT_POI
{
    public partial class Rangos : Form
    {

        public static INICIO inicio;

        public Rangos()
        {
            InitializeComponent();
        }

        public Rangos(UserProfile user)
        {
            InitializeComponent();
            if(Log.loggedUser.idUser == user.idUser)
                BTN_RECLAMAR.Visible = true;

            this.Text = "Rangos de " + user.username;

            pb_Level2.Visible = user.points > 0;
            pb_Level3.Visible = user.points > 1;
            pb_Level4.Visible = user.points > 2;
        }

        private void RECOMPENSA_Load(object sender, EventArgs e)
        {
            //LOGIN.usuario.getPoints;
            //0 = 19
            //1 = 121
            //2 = 266
            //3 = 401
            //4 = 536
            //5 = 752
        }

        private void BTN_REGRESAR_RECOMPENSA_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BTN_CERRAR_Click(object sender, EventArgs e)
        {
            inicio.CheckRequirementsLevelUp();

            pb_Level2.Visible = Log.loggedUser.points > 0;
            pb_Level3.Visible = Log.loggedUser.points > 1;
            pb_Level4.Visible = Log.loggedUser.points > 2;
        }
    }
}
