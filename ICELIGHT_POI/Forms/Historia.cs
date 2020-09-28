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
    public partial class Historia : Form
    {
        public static INICIO inicio;

        public Historia()
        {
            InitializeComponent();
        }

        private void PERFIL_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            inicio.FN_BTN_SALIR_PERFIL();
        }

        private void rbtn_Conected_CheckedChanged(object sender, EventArgs e)
        {
            inicio.SendChangeProfileState(1);
        }

        private void rbtn_Absent_CheckedChanged(object sender, EventArgs e)
        {
            inicio.SendChangeProfileState(2);
        }

        private void BTN_CERRAR_Click(object sender, EventArgs e)
        {
            inicio.SendChangeProfileState(3);
            inicio.openLoginForm();
            inicio.Hide();
            inicio.Close();
        }
    }
}
