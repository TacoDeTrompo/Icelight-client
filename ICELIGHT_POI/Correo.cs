using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;

namespace ICELIGHT_POI
{
    public partial class Correo : Form
    {
        List<string> attachments = null;
        int attachmentsIterator = 0;
        string attachment = null;

        public Correo()
        {
            InitializeComponent();
        }

        private void IMG_BOX_CHAT_Click(object sender, EventArgs e)
        {

        }

        private void BTN_REGRESAR_CORREO_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CORREO_Load(object sender, EventArgs e)
        {

        }

        private void pb_Send_Click(object sender, EventArgs e)
        {
            try
            {
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(TB_DE2_CORREO.Text);
                mail.To.Add(new MailAddress(TB_PARA2_CORREO.Text));
                mail.Subject = TB_ASUNTO2_CORREO.Text;
                mail.IsBodyHtml = false;
                mail.Body = "Mail from: " + TB_DE2_CORREO.Text + "\r\n" + TB_MENSAJE_CORREO.Text;

                if (attachments != null)
                {
                    foreach(string FileName in attachments)
                    {
                        Attachment attachment = new Attachment(FileName, MediaTypeNames.Application.Octet);
                        ContentDisposition disposition = attachment.ContentDisposition;
                        disposition.CreationDate = File.GetCreationTime(FileName);
                        disposition.ModificationDate = File.GetLastWriteTime(FileName);
                        disposition.ReadDate = File.GetLastAccessTime(FileName);
                        disposition.FileName = Path.GetFileName(FileName);
                        disposition.Size = new FileInfo(FileName).Length;
                        disposition.DispositionType = DispositionTypeNames.Attachment;
                        mail.Attachments.Add(attachment);
                    }
                }

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("nuntius.noreply@gmail.com", "n4zvN9DmnARXgWq");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
                MessageBox.Show("SUCCESS", "UUMMFF", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK);
            }

            #region unused
            //MailMessage mail = new MailMessage(TB_DE2_CORREO.Text, TB_PARA2_CORREO.Text);
            //
            //SmtpClient client = new SmtpClient();
            //client.Port = 587;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = false;
            //client.Host = "smtp.gmail.com";
            //client.EnableSsl = false;
            //client.Credentials = new System.Net.NetworkCredential("nuntius.noreply@gmail.com", "poiboi24");
            //
            //mail.Subject = TB_ASUNTO2_CORREO.Text;
            //mail.Body = TB_MENSAJE_CORREO.Text;
            //
            //try
            //{
            //    client.Send(mail);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            //}
            #endregion
        }

        private void pb_attachment_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            openFileDialog1.Multiselect = true;
            openFileDialog1.Title = "Choose a file to attach";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                attachments = new List<string>();
                foreach (string File in openFileDialog1.FileNames)
                {
                    attachments.Add(File);
                    attachmentsIterator++;
                }
            }
        }
    }
}
