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
    public partial class Mensajeria : Form
    {
        public static INICIO inicio;
        internal static List<ContactoUI> contactos = new List<ContactoUI>();
        internal Panel PN_Contactos = new Panel() { Size = new Size(209, 550), Location = new Point(16, 187) };
        private const int MaxContactosUI = 10;
        private int HeightCon;
        public Mensajeria()
        {
            InitializeComponent();


        }

        public void RefreshProfileImage()
        {
            foreach (ContactoUI contacto in contactos)
            {
                contacto.RefreshProfileImage();
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }
        private void BTN_SALIR_MENS_Click(object sender, EventArgs e)
        {
            inicio.FN_BTN_SALIR_MENS();
        }

        private void BTN_CORREO_Click(object sender, EventArgs e)
        {
            Correo co = new Correo();
            co.Show();
        }
        private void MENSAJES_Load(object sender, EventArgs e)
        {


            if (contactos.Count == 0)
                foreach (Conversation conversation in INICIO.list_Conversations)
                {
                    contactos.Add(
                               new ContactoUI(
                                   conversation.participants,
                                   new Point(0, contactos.Count == 0 ? 0 : contactos.Last().fondo.Location.Y + ContactoUI.Height + 6),
                                   panel_Contactos
                                   )
                           );
                    contactos.Last().setConversation(conversation);
                }
            else
                foreach (ContactoUI con in contactos)
                {
                    panel_Contactos.Controls.Add(con.fondo);
                    con.fondo.Visible = true;
                }
        }

        private void IC_MENSAJE_Click(object sender, EventArgs e)
        {

        }

        private void IC_CONFI_LLAMADA_Click(object sender, EventArgs e)
        {
            inicio.ReturnToMainMenuSidebar();
        }

        private void PB_MASUSUARIOS_Click(object sender, EventArgs e)
        {

        }
    }
}
//
//for (int i = 0; i<INICIO.L_conversaciones.Count; i++)
//                {



//                    if (i<INICIO.list_Usuarios.Count)
//                    {
//                        contactos.Add(
//                            new ContactoUI(
//                                INICIO.list_Usuarios[i],
//                                new Point(0, contactos.Count == 0 ? 0 : contactos.Last().fondo.Location.Y + ContactoUI.Height + 6),
//                                panel_Contactos
//                                )
//                        );

//                        contactos.Last().setConversacion(INICIO.L_conversaciones[i]);
//                    }
//                    else
//                    {
//                        contactos.Add(
//                            new ContactoUI(
//                                INICIO.L_conversaciones[i].usuarios,
//                                new Point(0, contactos.Count == 0 ? 0 : contactos.Last().fondo.Location.Y + ContactoUI.Height + 6),
//                                panel_Contactos
//                                )
//                        );
//                        contactos.Last().setConversacion(INICIO.L_conversaciones[i]);
//                    }
//                }