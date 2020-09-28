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

namespace ICELIGHT_POI
{
    public partial class InicioSesion : UserControl
    {
        public static Log log;

        public InicioSesion()
        {
            InitializeComponent();
        }

        private void lnkRegister_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.ToggleLoginRegisterVisibility();
        }

        private void btnLogIn_Click_1(object sender, EventArgs e)
        {
            log.BtnLoginClick();
        }
    }
}
