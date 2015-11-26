namespace camera
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button_TakePhoto = new System.Windows.Forms.Button();
            this.textbox_picfoldername = new System.Windows.Forms.TextBox();
            this.textBox_picfolder = new System.Windows.Forms.TextBox();
            this.textbox_picmidname = new System.Windows.Forms.TextBox();
            this.textBox_picmidtext = new System.Windows.Forms.TextBox();
            this.button_apply = new System.Windows.Forms.Button();
            this.textBox_picharddisk = new System.Windows.Forms.TextBox();
            this.textBox_harddiskname = new System.Windows.Forms.TextBox();
            this.textBox_picfolder2 = new System.Windows.Forms.TextBox();
            this.textbox_picfoldername2 = new System.Windows.Forms.TextBox();
            this.textBox_picfolder3 = new System.Windows.Forms.TextBox();
            this.textbox_picfoldername3 = new System.Windows.Forms.TextBox();
            this.button_Download = new System.Windows.Forms.Button();
            this.button_closesession = new System.Windows.Forms.Button();
            this.button_Init = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.liveViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ViewBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_TakePhoto
            // 
            this.button_TakePhoto.Location = new System.Drawing.Point(14, 69);
            this.button_TakePhoto.Name = "button_TakePhoto";
            this.button_TakePhoto.Size = new System.Drawing.Size(100, 23);
            this.button_TakePhoto.TabIndex = 1;
            this.button_TakePhoto.Text = "TakePhoto";
            this.button_TakePhoto.UseVisualStyleBackColor = true;
            this.button_TakePhoto.Click += new System.EventHandler(this.button_TakePhoto_Click);
            // 
            // textbox_picfoldername
            // 
            this.textbox_picfoldername.Location = new System.Drawing.Point(120, 625);
            this.textbox_picfoldername.Name = "textbox_picfoldername";
            this.textbox_picfoldername.ReadOnly = true;
            this.textbox_picfoldername.Size = new System.Drawing.Size(100, 21);
            this.textbox_picfoldername.TabIndex = 2;
            this.textbox_picfoldername.Text = "Folder";
            // 
            // textBox_picfolder
            // 
            this.textBox_picfolder.Location = new System.Drawing.Point(120, 652);
            this.textBox_picfolder.Name = "textBox_picfolder";
            this.textBox_picfolder.Size = new System.Drawing.Size(100, 21);
            this.textBox_picfolder.TabIndex = 3;
            // 
            // textbox_picmidname
            // 
            this.textbox_picmidname.Location = new System.Drawing.Point(120, 681);
            this.textbox_picmidname.Name = "textbox_picmidname";
            this.textbox_picmidname.ReadOnly = true;
            this.textbox_picmidname.Size = new System.Drawing.Size(100, 21);
            this.textbox_picmidname.TabIndex = 4;
            this.textbox_picmidname.Text = "MidText";
            // 
            // textBox_picmidtext
            // 
            this.textBox_picmidtext.Location = new System.Drawing.Point(120, 708);
            this.textBox_picmidtext.Name = "textBox_picmidtext";
            this.textBox_picmidtext.Size = new System.Drawing.Size(100, 21);
            this.textBox_picmidtext.TabIndex = 5;
            // 
            // button_apply
            // 
            this.button_apply.Location = new System.Drawing.Point(251, 706);
            this.button_apply.Name = "button_apply";
            this.button_apply.Size = new System.Drawing.Size(75, 23);
            this.button_apply.TabIndex = 6;
            this.button_apply.Text = "Apply";
            this.button_apply.UseVisualStyleBackColor = true;
            this.button_apply.Click += new System.EventHandler(this.button_apply_Click);
            // 
            // textBox_picharddisk
            // 
            this.textBox_picharddisk.Location = new System.Drawing.Point(14, 652);
            this.textBox_picharddisk.Name = "textBox_picharddisk";
            this.textBox_picharddisk.Size = new System.Drawing.Size(100, 21);
            this.textBox_picharddisk.TabIndex = 8;
            // 
            // textBox_harddiskname
            // 
            this.textBox_harddiskname.Location = new System.Drawing.Point(14, 625);
            this.textBox_harddiskname.Name = "textBox_harddiskname";
            this.textBox_harddiskname.ReadOnly = true;
            this.textBox_harddiskname.Size = new System.Drawing.Size(100, 21);
            this.textBox_harddiskname.TabIndex = 7;
            this.textBox_harddiskname.Text = "HardDisk";
            // 
            // textBox_picfolder2
            // 
            this.textBox_picfolder2.Location = new System.Drawing.Point(226, 652);
            this.textBox_picfolder2.Name = "textBox_picfolder2";
            this.textBox_picfolder2.Size = new System.Drawing.Size(100, 21);
            this.textBox_picfolder2.TabIndex = 10;
            // 
            // textbox_picfoldername2
            // 
            this.textbox_picfoldername2.Location = new System.Drawing.Point(226, 625);
            this.textbox_picfoldername2.Name = "textbox_picfoldername2";
            this.textbox_picfoldername2.ReadOnly = true;
            this.textbox_picfoldername2.Size = new System.Drawing.Size(100, 21);
            this.textbox_picfoldername2.TabIndex = 9;
            this.textbox_picfoldername2.Text = "/Folder";
            // 
            // textBox_picfolder3
            // 
            this.textBox_picfolder3.Location = new System.Drawing.Point(14, 708);
            this.textBox_picfolder3.Name = "textBox_picfolder3";
            this.textBox_picfolder3.Size = new System.Drawing.Size(100, 21);
            this.textBox_picfolder3.TabIndex = 12;
            // 
            // textbox_picfoldername3
            // 
            this.textbox_picfoldername3.Location = new System.Drawing.Point(14, 681);
            this.textbox_picfoldername3.Name = "textbox_picfoldername3";
            this.textbox_picfoldername3.ReadOnly = true;
            this.textbox_picfoldername3.Size = new System.Drawing.Size(100, 21);
            this.textbox_picfoldername3.TabIndex = 11;
            this.textbox_picfoldername3.Text = "//Folder";
            // 
            // button_Download
            // 
            this.button_Download.Location = new System.Drawing.Point(131, 69);
            this.button_Download.Name = "button_Download";
            this.button_Download.Size = new System.Drawing.Size(100, 23);
            this.button_Download.TabIndex = 13;
            this.button_Download.Text = "Download";
            this.button_Download.UseVisualStyleBackColor = true;
            this.button_Download.Click += new System.EventHandler(this.button_Download_Click);
            // 
            // button_closesession
            // 
            this.button_closesession.Location = new System.Drawing.Point(131, 28);
            this.button_closesession.Name = "button_closesession";
            this.button_closesession.Size = new System.Drawing.Size(100, 23);
            this.button_closesession.TabIndex = 14;
            this.button_closesession.Text = "CloseSession";
            this.button_closesession.UseVisualStyleBackColor = true;
            this.button_closesession.Click += new System.EventHandler(this.button_closesession_Click);
            // 
            // button_Init
            // 
            this.button_Init.Location = new System.Drawing.Point(12, 28);
            this.button_Init.Name = "button_Init";
            this.button_Init.Size = new System.Drawing.Size(100, 23);
            this.button_Init.TabIndex = 15;
            this.button_Init.Text = "InitCamera";
            this.button_Init.UseVisualStyleBackColor = true;
            this.button_Init.Click += new System.EventHandler(this.button_Init_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.cameraToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1406, 25);
            this.menuStrip1.TabIndex = 17;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.configToolStripMenuItem.Text = "Config";
            this.configToolStripMenuItem.Click += new System.EventHandler(this.configToolStripMenuItem_Click);
            // 
            // cameraToolStripMenuItem
            // 
            this.cameraToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.liveViewToolStripMenuItem});
            this.cameraToolStripMenuItem.Name = "cameraToolStripMenuItem";
            this.cameraToolStripMenuItem.Size = new System.Drawing.Size(65, 21);
            this.cameraToolStripMenuItem.Text = "Camera";
            // 
            // liveViewToolStripMenuItem
            // 
            this.liveViewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewAllToolStripMenuItem});
            this.liveViewToolStripMenuItem.Name = "liveViewToolStripMenuItem";
            this.liveViewToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.liveViewToolStripMenuItem.Text = "Live View";
            // 
            // viewAllToolStripMenuItem
            // 
            this.viewAllToolStripMenuItem.Name = "viewAllToolStripMenuItem";
            this.viewAllToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.viewAllToolStripMenuItem.Text = "ViewAll";
            this.viewAllToolStripMenuItem.Click += new System.EventHandler(this.viewAllToolStripMenuItem_Click);
            // 
            // ViewBox1
            // 
            this.ViewBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ViewBox1.Location = new System.Drawing.Point(344, 33);
            this.ViewBox1.Name = "ViewBox1";
            this.ViewBox1.Size = new System.Drawing.Size(1050, 700);
            this.ViewBox1.TabIndex = 18;
            this.ViewBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1406, 745);
            this.Controls.Add(this.ViewBox1);
            this.Controls.Add(this.button_Init);
            this.Controls.Add(this.button_closesession);
            this.Controls.Add(this.button_Download);
            this.Controls.Add(this.textBox_picfolder3);
            this.Controls.Add(this.textbox_picfoldername3);
            this.Controls.Add(this.textBox_picfolder2);
            this.Controls.Add(this.textbox_picfoldername2);
            this.Controls.Add(this.textBox_picharddisk);
            this.Controls.Add(this.textBox_harddiskname);
            this.Controls.Add(this.button_apply);
            this.Controls.Add(this.textBox_picmidtext);
            this.Controls.Add(this.textbox_picmidname);
            this.Controls.Add(this.textBox_picfolder);
            this.Controls.Add(this.textbox_picfoldername);
            this.Controls.Add(this.button_TakePhoto);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "CamerControl";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ViewBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_TakePhoto;
        private System.Windows.Forms.TextBox textbox_picfoldername;
        private System.Windows.Forms.TextBox textBox_picfolder;
        private System.Windows.Forms.TextBox textbox_picmidname;
        private System.Windows.Forms.TextBox textBox_picmidtext;
        private System.Windows.Forms.Button button_apply;
        private System.Windows.Forms.TextBox textBox_picharddisk;
        private System.Windows.Forms.TextBox textBox_harddiskname;
        private System.Windows.Forms.TextBox textBox_picfolder2;
        private System.Windows.Forms.TextBox textbox_picfoldername2;
        private System.Windows.Forms.TextBox textBox_picfolder3;
        private System.Windows.Forms.TextBox textbox_picfoldername3;
        private System.Windows.Forms.Button button_Download;
        private System.Windows.Forms.Button button_closesession;
        private System.Windows.Forms.Button button_Init;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        public System.Windows.Forms.PictureBox ViewBox1;
        private System.Windows.Forms.ToolStripMenuItem cameraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem liveViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewAllToolStripMenuItem;

    }
}

