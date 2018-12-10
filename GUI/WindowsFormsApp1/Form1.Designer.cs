namespace WindowsFormsApp1
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.loadbutton = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.connectbutton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.botIP_label = new System.Windows.Forms.Label();
            this.commandbox = new System.Windows.Forms.TextBox();
            this.sendcommandbutton = new System.Windows.Forms.Button();
            this.passwordbox = new System.Windows.Forms.TextBox();
            this.password_label = new System.Windows.Forms.Label();
            this.devicelist = new System.Windows.Forms.ComboBox();
            this.devicename_label = new System.Windows.Forms.Label();
            this.logbox = new System.Windows.Forms.RichTextBox();
            this.loglabel = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.filelabel = new System.Windows.Forms.Label();
            this.filepathbox = new System.Windows.Forms.TextBox();
            this.filebutton = new System.Windows.Forms.Button();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.stream_label = new System.Windows.Forms.Label();
            this.file_label = new System.Windows.Forms.Label();
            this.version_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(12, 221);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Play";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // loadbutton
            // 
            this.loadbutton.Enabled = false;
            this.loadbutton.Location = new System.Drawing.Point(345, 221);
            this.loadbutton.Name = "loadbutton";
            this.loadbutton.Size = new System.Drawing.Size(87, 23);
            this.loadbutton.TabIndex = 10;
            this.loadbutton.Text = "Load";
            this.loadbutton.UseVisualStyleBackColor = true;
            this.loadbutton.Click += new System.EventHandler(this.loadbutton_Click);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(105, 221);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(87, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Stop";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // connectbutton
            // 
            this.connectbutton.Location = new System.Drawing.Point(12, 62);
            this.connectbutton.Name = "connectbutton";
            this.connectbutton.Size = new System.Drawing.Size(180, 23);
            this.connectbutton.TabIndex = 1;
            this.connectbutton.Text = "Connect";
            this.connectbutton.UseVisualStyleBackColor = true;
            this.connectbutton.Click += new System.EventHandler(this.connectbutton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(77, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(115, 19);
            this.textBox1.TabIndex = 8;
            this.textBox1.Text = "127.0.0.1:8888";
            // 
            // botIP_label
            // 
            this.botIP_label.AutoSize = true;
            this.botIP_label.Location = new System.Drawing.Point(12, 15);
            this.botIP_label.Name = "botIP_label";
            this.botIP_label.Size = new System.Drawing.Size(37, 12);
            this.botIP_label.TabIndex = 9;
            this.botIP_label.Text = "Bot IP";
            // 
            // commandbox
            // 
            this.commandbox.Enabled = false;
            this.commandbox.Location = new System.Drawing.Point(12, 120);
            this.commandbox.Name = "commandbox";
            this.commandbox.Size = new System.Drawing.Size(180, 19);
            this.commandbox.TabIndex = 10;
            this.commandbox.Text = "@@ help";
            this.commandbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.commandbox_KeyPress);
            // 
            // sendcommandbutton
            // 
            this.sendcommandbutton.Enabled = false;
            this.sendcommandbutton.Location = new System.Drawing.Point(12, 91);
            this.sendcommandbutton.Name = "sendcommandbutton";
            this.sendcommandbutton.Size = new System.Drawing.Size(180, 23);
            this.sendcommandbutton.TabIndex = 11;
            this.sendcommandbutton.Text = "Send command";
            this.sendcommandbutton.UseVisualStyleBackColor = true;
            this.sendcommandbutton.Click += new System.EventHandler(this.sendcommandbutton_Click);
            // 
            // passwordbox
            // 
            this.passwordbox.Location = new System.Drawing.Point(77, 37);
            this.passwordbox.Name = "passwordbox";
            this.passwordbox.PasswordChar = '*';
            this.passwordbox.Size = new System.Drawing.Size(115, 19);
            this.passwordbox.TabIndex = 12;
            // 
            // password_label
            // 
            this.password_label.AutoSize = true;
            this.password_label.Location = new System.Drawing.Point(12, 40);
            this.password_label.Name = "password_label";
            this.password_label.Size = new System.Drawing.Size(54, 12);
            this.password_label.TabIndex = 13;
            this.password_label.Text = "Password";
            // 
            // devicelist
            // 
            this.devicelist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.devicelist.FormattingEnabled = true;
            this.devicelist.Location = new System.Drawing.Point(12, 195);
            this.devicelist.Name = "devicelist";
            this.devicelist.Size = new System.Drawing.Size(180, 20);
            this.devicelist.TabIndex = 14;
            // 
            // devicename_label
            // 
            this.devicename_label.AutoSize = true;
            this.devicename_label.Location = new System.Drawing.Point(12, 180);
            this.devicename_label.Name = "devicename_label";
            this.devicename_label.Size = new System.Drawing.Size(73, 12);
            this.devicename_label.TabIndex = 15;
            this.devicename_label.Text = "Device name:";
            // 
            // logbox
            // 
            this.logbox.Location = new System.Drawing.Point(198, 26);
            this.logbox.Name = "logbox";
            this.logbox.ReadOnly = true;
            this.logbox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.logbox.Size = new System.Drawing.Size(234, 113);
            this.logbox.TabIndex = 16;
            this.logbox.Text = "";
            this.logbox.TextChanged += new System.EventHandler(this.logbox_TextChanged);
            // 
            // loglabel
            // 
            this.loglabel.AutoSize = true;
            this.loglabel.Location = new System.Drawing.Point(198, 11);
            this.loglabel.Name = "loglabel";
            this.loglabel.Size = new System.Drawing.Size(83, 12);
            this.loglabel.TabIndex = 17;
            this.loglabel.Text = "Connection log:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Music file (*.mp3)|*.mp3|All files (*.*)|*.*";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // filelabel
            // 
            this.filelabel.AutoSize = true;
            this.filelabel.Location = new System.Drawing.Point(252, 180);
            this.filelabel.Name = "filelabel";
            this.filelabel.Size = new System.Drawing.Size(111, 12);
            this.filelabel.TabIndex = 19;
            this.filelabel.Text = "File name (full path):";
            // 
            // filepathbox
            // 
            this.filepathbox.Location = new System.Drawing.Point(252, 196);
            this.filepathbox.Name = "filepathbox";
            this.filepathbox.Size = new System.Drawing.Size(180, 19);
            this.filepathbox.TabIndex = 20;
            // 
            // filebutton
            // 
            this.filebutton.Location = new System.Drawing.Point(252, 221);
            this.filebutton.Name = "filebutton";
            this.filebutton.Size = new System.Drawing.Size(87, 23);
            this.filebutton.TabIndex = 21;
            this.filebutton.Text = "Select file";
            this.filebutton.UseVisualStyleBackColor = true;
            this.filebutton.Click += new System.EventHandler(this.filebutton_Click);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(321, 161);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(42, 16);
            this.radioButton3.TabIndex = 6;
            this.radioButton3.Text = "File";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.Visible = false;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(91, 161);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(59, 16);
            this.radioButton1.TabIndex = 3;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Stream";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.Visible = false;
            // 
            // stream_label
            // 
            this.stream_label.AutoSize = true;
            this.stream_label.Location = new System.Drawing.Point(14, 164);
            this.stream_label.Name = "stream_label";
            this.stream_label.Size = new System.Drawing.Size(41, 12);
            this.stream_label.TabIndex = 22;
            this.stream_label.Text = "Stream";
            // 
            // file_label
            // 
            this.file_label.AutoSize = true;
            this.file_label.Location = new System.Drawing.Point(252, 164);
            this.file_label.Name = "file_label";
            this.file_label.Size = new System.Drawing.Size(24, 12);
            this.file_label.TabIndex = 23;
            this.file_label.Text = "File";
            // 
            // version_label
            // 
            this.version_label.AutoSize = true;
            this.version_label.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.version_label.Location = new System.Drawing.Point(422, 247);
            this.version_label.Name = "version_label";
            this.version_label.Size = new System.Drawing.Size(19, 12);
            this.version_label.TabIndex = 24;
            this.version_label.Text = "1.0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 261);
            this.Controls.Add(this.version_label);
            this.Controls.Add(this.file_label);
            this.Controls.Add(this.stream_label);
            this.Controls.Add(this.filebutton);
            this.Controls.Add(this.filepathbox);
            this.Controls.Add(this.filelabel);
            this.Controls.Add(this.loglabel);
            this.Controls.Add(this.logbox);
            this.Controls.Add(this.devicename_label);
            this.Controls.Add(this.devicelist);
            this.Controls.Add(this.password_label);
            this.Controls.Add(this.passwordbox);
            this.Controls.Add(this.sendcommandbutton);
            this.Controls.Add(this.commandbox);
            this.Controls.Add(this.botIP_label);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.connectbutton);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.loadbutton);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "タイガボート（ディスボート）";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button loadbutton;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button connectbutton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label botIP_label;
        private System.Windows.Forms.TextBox commandbox;
        private System.Windows.Forms.Button sendcommandbutton;
        private System.Windows.Forms.TextBox passwordbox;
        private System.Windows.Forms.Label password_label;
        private System.Windows.Forms.ComboBox devicelist;
        private System.Windows.Forms.Label devicename_label;
        private System.Windows.Forms.RichTextBox logbox;
        private System.Windows.Forms.Label loglabel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label filelabel;
        private System.Windows.Forms.TextBox filepathbox;
        private System.Windows.Forms.Button filebutton;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label stream_label;
        private System.Windows.Forms.Label file_label;
        private System.Windows.Forms.Label version_label;
    }
}

