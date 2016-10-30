namespace WindowsFormsApplication1
{
    partial class Form2
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
            this.studystateBar = new System.Windows.Forms.Panel();
            this.btn_note = new System.Windows.Forms.PictureBox();
            this.btn_closeStudy = new System.Windows.Forms.PictureBox();
            this.pic_study = new System.Windows.Forms.PictureBox();
            this.btn_strength = new System.Windows.Forms.PictureBox();
            this.btn_syllable = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.studystateBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_note)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_closeStudy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_study)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_strength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_syllable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // studystateBar
            // 
            this.studystateBar.BackColor = System.Drawing.Color.Transparent;
            this.studystateBar.Controls.Add(this.btn_syllable);
            this.studystateBar.Controls.Add(this.btn_strength);
            this.studystateBar.Controls.Add(this.btn_note);
            this.studystateBar.Controls.Add(this.btn_closeStudy);
            this.studystateBar.Location = new System.Drawing.Point(1, 0);
            this.studystateBar.Name = "studystateBar";
            this.studystateBar.Size = new System.Drawing.Size(670, 93);
            this.studystateBar.TabIndex = 17;
            this.studystateBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.studystateBar_MouseDown);
            this.studystateBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.studystateBar_MouseMove);
            this.studystateBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.studystateBar_MouseUp);
            // 
            // btn_note
            // 
            this.btn_note.BackColor = System.Drawing.Color.Transparent;
            this.btn_note.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn_note.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_note.Image = global::WindowsFormsApplication1.Properties.Resources.btn_s_note_orig;
            this.btn_note.Location = new System.Drawing.Point(11, 16);
            this.btn_note.Name = "btn_note";
            this.btn_note.Size = new System.Drawing.Size(74, 52);
            this.btn_note.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_note.TabIndex = 35;
            this.btn_note.TabStop = false;
            this.btn_note.Click += new System.EventHandler(this.btn_note_Click);
            this.btn_note.MouseLeave += new System.EventHandler(this.btn_note_MouseLeave);
            this.btn_note.MouseHover += new System.EventHandler(this.btn_note_MouseHover);
            // 
            // btn_closeStudy
            // 
            this.btn_closeStudy.BackColor = System.Drawing.Color.Transparent;
            this.btn_closeStudy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_closeStudy.Image = global::WindowsFormsApplication1.Properties.Resources.btn_closeform_orig;
            this.btn_closeStudy.Location = new System.Drawing.Point(635, 22);
            this.btn_closeStudy.Name = "btn_closeStudy";
            this.btn_closeStudy.Size = new System.Drawing.Size(24, 30);
            this.btn_closeStudy.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_closeStudy.TabIndex = 34;
            this.btn_closeStudy.TabStop = false;
            this.btn_closeStudy.Click += new System.EventHandler(this.btn_closeStudy_Click);
            this.btn_closeStudy.MouseLeave += new System.EventHandler(this.btn_closeStudy_MouseLeave);
            this.btn_closeStudy.MouseHover += new System.EventHandler(this.btn_closeStudy_MouseHover);
            // 
            // pic_study
            // 
            this.pic_study.Location = new System.Drawing.Point(1, 115);
            this.pic_study.Name = "pic_study";
            this.pic_study.Size = new System.Drawing.Size(670, 430);
            this.pic_study.TabIndex = 19;
            this.pic_study.TabStop = false;
            // 
            // btn_strength
            // 
            this.btn_strength.BackColor = System.Drawing.Color.Transparent;
            this.btn_strength.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn_strength.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_strength.Image = global::WindowsFormsApplication1.Properties.Resources.btn_s_strength_orig;
            this.btn_strength.Location = new System.Drawing.Point(222, 19);
            this.btn_strength.Name = "btn_strength";
            this.btn_strength.Size = new System.Drawing.Size(82, 49);
            this.btn_strength.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_strength.TabIndex = 36;
            this.btn_strength.TabStop = false;
            this.btn_strength.Click += new System.EventHandler(this.btn_strength_Click);
            this.btn_strength.MouseLeave += new System.EventHandler(this.btn_strength_MouseLeave);
            this.btn_strength.MouseHover += new System.EventHandler(this.btn_strength_MouseHover);
            // 
            // btn_syllable
            // 
            this.btn_syllable.BackColor = System.Drawing.Color.Transparent;
            this.btn_syllable.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn_syllable.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_syllable.Image = global::WindowsFormsApplication1.Properties.Resources.btn_s_syllable_orig;
            this.btn_syllable.Location = new System.Drawing.Point(100, 17);
            this.btn_syllable.Name = "btn_syllable";
            this.btn_syllable.Size = new System.Drawing.Size(105, 49);
            this.btn_syllable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_syllable.TabIndex = 37;
            this.btn_syllable.TabStop = false;
            this.btn_syllable.Click += new System.EventHandler(this.btn_syllable_Click);
            this.btn_syllable.MouseLeave += new System.EventHandler(this.btn_syllable_MouseLeave);
            this.btn_syllable.MouseHover += new System.EventHandler(this.btn_syllable_MouseHover);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::WindowsFormsApplication1.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(523, 551);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(137, 49);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WindowsFormsApplication1.Properties.Resources.panel_study;
            this.ClientSize = new System.Drawing.Size(672, 622);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pic_study);
            this.Controls.Add(this.studystateBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form2";
            this.studystateBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btn_note)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_closeStudy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_study)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_strength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_syllable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel studystateBar;
        private System.Windows.Forms.PictureBox pic_study;
        private System.Windows.Forms.PictureBox btn_closeStudy;
        private System.Windows.Forms.PictureBox btn_note;
        private System.Windows.Forms.PictureBox btn_syllable;
        private System.Windows.Forms.PictureBox btn_strength;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}