namespace ICELIGHT_POI
{
    partial class LLAMADA
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.IC_CONFI_LLAMADA = new System.Windows.Forms.PictureBox();
            this.BTN_COLGAR = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.IC_CONFI_LLAMADA)).BeginInit();
            this.SuspendLayout();
            // 
            // IC_CONFI_LLAMADA
            // 
            this.IC_CONFI_LLAMADA.Image = global::ICELIGHT_POI.Properties.Resources.left_arrow;
            this.IC_CONFI_LLAMADA.Location = new System.Drawing.Point(16, 12);
            this.IC_CONFI_LLAMADA.Name = "IC_CONFI_LLAMADA";
            this.IC_CONFI_LLAMADA.Size = new System.Drawing.Size(36, 36);
            this.IC_CONFI_LLAMADA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.IC_CONFI_LLAMADA.TabIndex = 15;
            this.IC_CONFI_LLAMADA.TabStop = false;
            this.IC_CONFI_LLAMADA.Click += new System.EventHandler(this.IC_CONFI_LLAMADA_Click);
            // 
            // BTN_COLGAR
            // 
            this.BTN_COLGAR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(61)))), ((int)(((byte)(78)))));
            this.BTN_COLGAR.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BTN_COLGAR.FlatAppearance.BorderSize = 0;
            this.BTN_COLGAR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_COLGAR.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_COLGAR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(26)))), ((int)(((byte)(46)))));
            this.BTN_COLGAR.Location = new System.Drawing.Point(22, 90);
            this.BTN_COLGAR.Name = "BTN_COLGAR";
            this.BTN_COLGAR.Size = new System.Drawing.Size(193, 33);
            this.BTN_COLGAR.TabIndex = 66;
            this.BTN_COLGAR.Text = "Colgar";
            this.BTN_COLGAR.UseVisualStyleBackColor = false;
            this.BTN_COLGAR.Click += new System.EventHandler(this.BTN_COLGAR_Click);
            // 
            // LLAMADA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(26)))), ((int)(((byte)(46)))));
            this.ClientSize = new System.Drawing.Size(237, 788);
            this.Controls.Add(this.BTN_COLGAR);
            this.Controls.Add(this.IC_CONFI_LLAMADA);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LLAMADA";
            this.Text = "LLAMADA";
            this.Load += new System.EventHandler(this.LLAMADA_Load);
            ((System.ComponentModel.ISupportInitialize)(this.IC_CONFI_LLAMADA)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox IC_CONFI_LLAMADA;
        private System.Windows.Forms.Button BTN_COLGAR;
    }
}