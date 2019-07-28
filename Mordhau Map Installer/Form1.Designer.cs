namespace Mordhau_Map_Installer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setMordhauDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.basicHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AvailableMapsBox = new System.Windows.Forms.CheckedListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.InstallButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.InstalledMapsBox = new System.Windows.Forms.CheckedListBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoneToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.MapDescriptionLabel = new System.Windows.Forms.LinkLabel();
            this.MapSuggestedPlayersLabel = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.MapSizeLabel = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.MapReleaseDateLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.MapVersionLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.MapAuthorsLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.MapNameLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.versionlabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelspacer = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.thumbnailBox = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thumbnailBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(719, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setMordhauDirectoryToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // setMordhauDirectoryToolStripMenuItem
            // 
            this.setMordhauDirectoryToolStripMenuItem.Name = "setMordhauDirectoryToolStripMenuItem";
            this.setMordhauDirectoryToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.setMordhauDirectoryToolStripMenuItem.Text = "Set Mordhau Directory";
            this.setMordhauDirectoryToolStripMenuItem.Click += new System.EventHandler(this.setMordhauDirectoryToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.basicHelpToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // basicHelpToolStripMenuItem
            // 
            this.basicHelpToolStripMenuItem.Name = "basicHelpToolStripMenuItem";
            this.basicHelpToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.basicHelpToolStripMenuItem.Text = "Basic Help";
            this.basicHelpToolStripMenuItem.Click += new System.EventHandler(this.basicHelpToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.AvailableMapsBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.InstallButton);
            this.panel1.Location = new System.Drawing.Point(11, 100);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(346, 273);
            this.panel1.TabIndex = 1;
            // 
            // AvailableMapsBox
            // 
            this.AvailableMapsBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AvailableMapsBox.BackColor = System.Drawing.SystemColors.Window;
            this.AvailableMapsBox.ContextMenuStrip = this.contextMenuStrip1;
            this.AvailableMapsBox.FormattingEnabled = true;
            this.AvailableMapsBox.Location = new System.Drawing.Point(15, 27);
            this.AvailableMapsBox.Name = "AvailableMapsBox";
            this.AvailableMapsBox.Size = new System.Drawing.Size(312, 199);
            this.AvailableMapsBox.TabIndex = 3;
            this.AvailableMapsBox.ThreeDCheckBoxes = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.selectNoneToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(138, 48);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            // 
            // selectNoneToolStripMenuItem
            // 
            this.selectNoneToolStripMenuItem.Name = "selectNoneToolStripMenuItem";
            this.selectNoneToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.selectNoneToolStripMenuItem.Text = "Select None";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Available Maps";
            // 
            // InstallButton
            // 
            this.InstallButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InstallButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.InstallButton.Enabled = false;
            this.InstallButton.Location = new System.Drawing.Point(15, 239);
            this.InstallButton.Name = "InstallButton";
            this.InstallButton.Size = new System.Drawing.Size(312, 21);
            this.InstallButton.TabIndex = 2;
            this.InstallButton.Text = "Install";
            this.InstallButton.UseVisualStyleBackColor = false;
            this.InstallButton.Click += new System.EventHandler(this.InstallButton_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.InstalledMapsBox);
            this.panel2.Controls.Add(this.RemoveButton);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(363, 100);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(346, 273);
            this.panel2.TabIndex = 3;
            // 
            // InstalledMapsBox
            // 
            this.InstalledMapsBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InstalledMapsBox.ContextMenuStrip = this.contextMenuStrip2;
            this.InstalledMapsBox.FormattingEnabled = true;
            this.InstalledMapsBox.Location = new System.Drawing.Point(17, 27);
            this.InstalledMapsBox.Name = "InstalledMapsBox";
            this.InstalledMapsBox.Size = new System.Drawing.Size(312, 199);
            this.InstalledMapsBox.TabIndex = 4;
            this.InstalledMapsBox.ThreeDCheckBoxes = true;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem1,
            this.selectNoneToolStripMenuItem1});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(138, 48);
            // 
            // selectAllToolStripMenuItem1
            // 
            this.selectAllToolStripMenuItem1.Name = "selectAllToolStripMenuItem1";
            this.selectAllToolStripMenuItem1.Size = new System.Drawing.Size(137, 22);
            this.selectAllToolStripMenuItem1.Text = "Select All";
            // 
            // selectNoneToolStripMenuItem1
            // 
            this.selectNoneToolStripMenuItem1.Name = "selectNoneToolStripMenuItem1";
            this.selectNoneToolStripMenuItem1.Size = new System.Drawing.Size(137, 22);
            this.selectNoneToolStripMenuItem1.Text = "Select None";
            // 
            // RemoveButton
            // 
            this.RemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.RemoveButton.Enabled = false;
            this.RemoveButton.Location = new System.Drawing.Point(17, 239);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(311, 21);
            this.RemoveButton.TabIndex = 4;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = false;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Installed Maps";
            // 
            // panel5
            // 
            this.panel5.AutoScroll = true;
            this.panel5.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Controls.Add(this.MapDescriptionLabel);
            this.panel5.Controls.Add(this.MapSuggestedPlayersLabel);
            this.panel5.Controls.Add(this.label10);
            this.panel5.Controls.Add(this.MapSizeLabel);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Controls.Add(this.MapReleaseDateLabel);
            this.panel5.Controls.Add(this.label8);
            this.panel5.Controls.Add(this.MapVersionLabel);
            this.panel5.Controls.Add(this.label7);
            this.panel5.Controls.Add(this.MapAuthorsLabel);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.MapNameLabel);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Location = new System.Drawing.Point(11, 379);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(698, 127);
            this.panel5.TabIndex = 4;
            // 
            // MapDescriptionLabel
            // 
            this.MapDescriptionLabel.AutoSize = true;
            this.MapDescriptionLabel.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.MapDescriptionLabel.Location = new System.Drawing.Point(100, 15);
            this.MapDescriptionLabel.Name = "MapDescriptionLabel";
            this.MapDescriptionLabel.Size = new System.Drawing.Size(0, 13);
            this.MapDescriptionLabel.TabIndex = 15;
            // 
            // MapSuggestedPlayersLabel
            // 
            this.MapSuggestedPlayersLabel.AutoSize = true;
            this.MapSuggestedPlayersLabel.Location = new System.Drawing.Point(100, 90);
            this.MapSuggestedPlayersLabel.Name = "MapSuggestedPlayersLabel";
            this.MapSuggestedPlayersLabel.Size = new System.Drawing.Size(0, 13);
            this.MapSuggestedPlayersLabel.TabIndex = 14;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 90);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(95, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "Suggested Players";
            // 
            // MapSizeLabel
            // 
            this.MapSizeLabel.AutoSize = true;
            this.MapSizeLabel.Location = new System.Drawing.Point(100, 75);
            this.MapSizeLabel.Name = "MapSizeLabel";
            this.MapSizeLabel.Size = new System.Drawing.Size(0, 13);
            this.MapSizeLabel.TabIndex = 12;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 75);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "File Size";
            // 
            // MapReleaseDateLabel
            // 
            this.MapReleaseDateLabel.AutoSize = true;
            this.MapReleaseDateLabel.Location = new System.Drawing.Point(100, 60);
            this.MapReleaseDateLabel.Name = "MapReleaseDateLabel";
            this.MapReleaseDateLabel.Size = new System.Drawing.Size(0, 13);
            this.MapReleaseDateLabel.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Release Date";
            // 
            // MapVersionLabel
            // 
            this.MapVersionLabel.AutoSize = true;
            this.MapVersionLabel.Location = new System.Drawing.Point(100, 45);
            this.MapVersionLabel.Name = "MapVersionLabel";
            this.MapVersionLabel.Size = new System.Drawing.Size(0, 13);
            this.MapVersionLabel.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Version";
            // 
            // MapAuthorsLabel
            // 
            this.MapAuthorsLabel.AutoSize = true;
            this.MapAuthorsLabel.Location = new System.Drawing.Point(100, 30);
            this.MapAuthorsLabel.Name = "MapAuthorsLabel";
            this.MapAuthorsLabel.Size = new System.Drawing.Size(0, 13);
            this.MapAuthorsLabel.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Author(s)";
            // 
            // MapNameLabel
            // 
            this.MapNameLabel.AutoSize = true;
            this.MapNameLabel.Location = new System.Drawing.Point(100, 0);
            this.MapNameLabel.Name = "MapNameLabel";
            this.MapNameLabel.Size = new System.Drawing.Size(0, 13);
            this.MapNameLabel.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Description";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Name";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.versionlabel,
            this.labelspacer,
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 661);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusStrip1.Size = new System.Drawing.Size(719, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // versionlabel
            // 
            this.versionlabel.Name = "versionlabel";
            this.versionlabel.Size = new System.Drawing.Size(118, 17);
            this.versionlabel.Text = "toolStripStatusLabel1";
            // 
            // labelspacer
            // 
            this.labelspacer.Name = "labelspacer";
            this.labelspacer.Size = new System.Drawing.Size(586, 17);
            this.labelspacer.Spring = true;
            this.labelspacer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(-1, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(65, 64);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Impact", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(124, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(484, 39);
            this.label3.TabIndex = 7;
            this.label3.Text = "Mordhau Community Maps Installer";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Location = new System.Drawing.Point(12, 28);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(697, 63);
            this.panel3.TabIndex = 8;
            // 
            // thumbnailBox
            // 
            this.thumbnailBox.ErrorImage = ((System.Drawing.Image)(resources.GetObject("thumbnailBox.ErrorImage")));
            this.thumbnailBox.Image = ((System.Drawing.Image)(resources.GetObject("thumbnailBox.Image")));
            this.thumbnailBox.InitialImage = ((System.Drawing.Image)(resources.GetObject("thumbnailBox.InitialImage")));
            this.thumbnailBox.Location = new System.Drawing.Point(11, 512);
            this.thumbnailBox.Name = "thumbnailBox";
            this.thumbnailBox.Size = new System.Drawing.Size(698, 146);
            this.thumbnailBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.thumbnailBox.TabIndex = 9;
            this.thumbnailBox.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(719, 683);
            this.Controls.Add(this.thumbnailBox);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Mordhau Community Maps Installer";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.contextMenuStrip2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thumbnailBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setMordhauDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button InstallButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label MapSuggestedPlayersLabel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label MapSizeLabel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label MapReleaseDateLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label MapVersionLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label MapAuthorsLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label MapNameLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckedListBox AvailableMapsBox;
        private System.Windows.Forms.CheckedListBox InstalledMapsBox;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel labelspacer;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.LinkLabel MapDescriptionLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectNoneToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem selectNoneToolStripMenuItem1;
        private System.Windows.Forms.ToolStripStatusLabel versionlabel;
        private System.Windows.Forms.ToolStripMenuItem basicHelpToolStripMenuItem;
        private System.Windows.Forms.PictureBox thumbnailBox;
    }
}

