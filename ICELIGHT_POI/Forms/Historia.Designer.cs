namespace ICELIGHT_POI
{
    partial class Historia
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
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.rbtn_Conected = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbl_Title = new System.Windows.Forms.Label();
            this.rbtn_Absent = new System.Windows.Forms.RadioButton();
            this.grp_Status = new System.Windows.Forms.GroupBox();
            this.BTN_CERRAR = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox4
            // 
            this.pictureBox4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pictureBox4.Image = global::ICELIGHT_POI.Properties.Resources.left_arrow;
            this.pictureBox4.Location = new System.Drawing.Point(20, 580);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(40, 40);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox4.TabIndex = 58;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.Click += new System.EventHandler(this.pictureBox4_Click);
            // 
            // rbtn_Conected
            // 
            this.rbtn_Conected.AutoSize = true;
            this.rbtn_Conected.Checked = true;
            this.rbtn_Conected.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtn_Conected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(61)))), ((int)(((byte)(78)))));
            this.rbtn_Conected.Location = new System.Drawing.Point(41, 125);
            this.rbtn_Conected.Name = "rbtn_Conected";
            this.rbtn_Conected.Size = new System.Drawing.Size(121, 29);
            this.rbtn_Conected.TabIndex = 59;
            this.rbtn_Conected.TabStop = true;
            this.rbtn_Conected.Text = "Conectado";
            this.rbtn_Conected.UseVisualStyleBackColor = true;
            this.rbtn_Conected.CheckedChanged += new System.EventHandler(this.rbtn_Conected_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(61)))), ((int)(((byte)(78)))));
            this.pictureBox1.Location = new System.Drawing.Point(20, 66);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(170, 4);
            this.pictureBox1.TabIndex = 61;
            this.pictureBox1.TabStop = false;
            // 
            // lbl_Title
            // 
            this.lbl_Title.AutoSize = true;
            this.lbl_Title.Font = new System.Drawing.Font("Yu Gothic UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(61)))), ((int)(((byte)(78)))));
            this.lbl_Title.Location = new System.Drawing.Point(12, 19);
            this.lbl_Title.Name = "lbl_Title";
            this.lbl_Title.Size = new System.Drawing.Size(130, 45);
            this.lbl_Title.TabIndex = 60;
            this.lbl_Title.Text = "Historia";
            this.lbl_Title.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // rbtn_Absent
            // 
            this.rbtn_Absent.AutoSize = true;
            this.rbtn_Absent.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtn_Absent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(61)))), ((int)(((byte)(78)))));
            this.rbtn_Absent.Location = new System.Drawing.Point(40, 159);
            this.rbtn_Absent.Name = "rbtn_Absent";
            this.rbtn_Absent.Size = new System.Drawing.Size(98, 29);
            this.rbtn_Absent.TabIndex = 62;
            this.rbtn_Absent.TabStop = true;
            this.rbtn_Absent.Text = "Ausente";
            this.rbtn_Absent.UseVisualStyleBackColor = true;
            this.rbtn_Absent.CheckedChanged += new System.EventHandler(this.rbtn_Absent_CheckedChanged);
            // 
            // grp_Status
            // 
            this.grp_Status.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grp_Status.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grp_Status.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(61)))), ((int)(((byte)(78)))));
            this.grp_Status.Location = new System.Drawing.Point(18, 96);
            this.grp_Status.Name = "grp_Status";
            this.grp_Status.Size = new System.Drawing.Size(200, 111);
            this.grp_Status.TabIndex = 63;
            this.grp_Status.TabStop = false;
            this.grp_Status.Text = "Estatus";
            // 
            // BTN_CERRAR
            // 
            this.BTN_CERRAR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(61)))), ((int)(((byte)(78)))));
            this.BTN_CERRAR.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BTN_CERRAR.FlatAppearance.BorderSize = 0;
            this.BTN_CERRAR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CERRAR.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_CERRAR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(26)))), ((int)(((byte)(46)))));
            this.BTN_CERRAR.Location = new System.Drawing.Point(18, 269);
            this.BTN_CERRAR.Name = "BTN_CERRAR";
            this.BTN_CERRAR.Size = new System.Drawing.Size(200, 33);
            this.BTN_CERRAR.TabIndex = 64;
            this.BTN_CERRAR.Text = "Cerrar Sesión";
            this.BTN_CERRAR.UseVisualStyleBackColor = false;
            this.BTN_CERRAR.Click += new System.EventHandler(this.BTN_CERRAR_Click);
            // 
            // Historia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(26)))), ((int)(((byte)(46)))));
            this.ClientSize = new System.Drawing.Size(237, 668);
            this.Controls.Add(this.BTN_CERRAR);
            this.Controls.Add(this.rbtn_Absent);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbl_Title);
            this.Controls.Add(this.rbtn_Conected);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.grp_Status);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Historia";
            this.Text = "PERFIL";
            this.Load += new System.EventHandler(this.PERFIL_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.RadioButton rbtn_Conected;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbl_Title;
        private System.Windows.Forms.RadioButton rbtn_Absent;
        private System.Windows.Forms.GroupBox grp_Status;
        private System.Windows.Forms.Button BTN_CERRAR;
    }
}