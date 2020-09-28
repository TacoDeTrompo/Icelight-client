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
    public partial class LLAMADA : Form
    {
        public static INICIO inicio;    

        public LLAMADA()
        {
            InitializeComponent();
        }

        private void IC_LLAMADA2_Click(object sender, EventArgs e)
        {
            //inicio.FN_IC_LLAMADA_LLAMADA();
        }

        private void IC_MENSAJE_LLAMADA_Click(object sender, EventArgs e)
        {
            //inicio.FN_IC_LLAMADA_MENSAJE();
        }

        private void PN_VIDEO_Paint(object sender, PaintEventArgs e)
        {

        }

        private void IC_CONFI_LLAMADA_Click(object sender, EventArgs e)
        {
            inicio.FN_BTN_SALIR_CONF();
        }

        private void LLAMADA_Load(object sender, EventArgs e)
        {

        }

        private void BTN_COLGAR_Click(object sender, EventArgs e)
        {
            inicio.STOP_CALL();
        }
    }
}
