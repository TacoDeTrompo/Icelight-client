using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICELIGHT_POI
{
    public partial class Registro : UserControl
    {
        public static Log log;

        public Registro()
        {
            InitializeComponent();
        }

        private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.ToggleLoginRegisterVisibility();
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            log.BtnRegisterClick();
        }
    }
}
