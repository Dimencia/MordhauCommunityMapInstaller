namespace Mordhau_Map_Installer
{
    partial class UpdateAvailable
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
            this.button1 = new System.Windows.Forms.Button();
            this.dontShowAgainBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.githubLink = new System.Windows.Forms.LinkLabel();
            this.currentVersionLabel = new System.Windows.Forms.Label();
            this.newVersionLabel = new System.Windows.Forms.Label();
            this.summaryLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(217, 219);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dontShowAgainBox
            // 
            this.dontShowAgainBox.AutoSize = true;
            this.dontShowAgainBox.Location = new System.Drawing.Point(12, 223);
            this.dontShowAgainBox.Name = "dontShowAgainBox";
            this.dontShowAgainBox.Size = new System.Drawing.Size(140, 17);
            this.dontShowAgainBox.TabIndex = 1;
            this.dontShowAgainBox.Text = "Don\'t check for updates";
            this.dontShowAgainBox.UseVisualStyleBackColor = true;
            this.dontShowAgainBox.CheckedChanged += new System.EventHandler(this.dontShowAgainBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(91, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(322, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "A newer version of MCMI has been detected";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Your Version: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(67, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "New Version:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(67, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Summary:";
            // 
            // githubLink
            // 
            this.githubLink.AutoSize = true;
            this.githubLink.LinkArea = new System.Windows.Forms.LinkArea(6, 4);
            this.githubLink.Location = new System.Drawing.Point(70, 177);
            this.githubLink.Name = "githubLink";
            this.githubLink.Size = new System.Drawing.Size(385, 17);
            this.githubLink.TabIndex = 6;
            this.githubLink.TabStop = true;
            this.githubLink.Text = "Click here to visit the GitHub page where you can download the new version";
            this.githubLink.UseCompatibleTextRendering = true;
            // 
            // currentVersionLabel
            // 
            this.currentVersionLabel.AutoSize = true;
            this.currentVersionLabel.Location = new System.Drawing.Point(146, 59);
            this.currentVersionLabel.Name = "currentVersionLabel";
            this.currentVersionLabel.Size = new System.Drawing.Size(0, 13);
            this.currentVersionLabel.TabIndex = 7;
            // 
            // newVersionLabel
            // 
            this.newVersionLabel.AutoSize = true;
            this.newVersionLabel.Location = new System.Drawing.Point(146, 85);
            this.newVersionLabel.Name = "newVersionLabel";
            this.newVersionLabel.Size = new System.Drawing.Size(0, 13);
            this.newVersionLabel.TabIndex = 8;
            // 
            // summaryLabel
            // 
            this.summaryLabel.Location = new System.Drawing.Point(146, 112);
            this.summaryLabel.Name = "summaryLabel";
            this.summaryLabel.Size = new System.Drawing.Size(249, 52);
            this.summaryLabel.TabIndex = 9;
            // 
            // UpdateAvailable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(503, 254);
            this.ControlBox = false;
            this.Controls.Add(this.summaryLabel);
            this.Controls.Add(this.newVersionLabel);
            this.Controls.Add(this.currentVersionLabel);
            this.Controls.Add(this.githubLink);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dontShowAgainBox);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "UpdateAvailable";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Update";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox dontShowAgainBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel githubLink;
        private System.Windows.Forms.Label currentVersionLabel;
        private System.Windows.Forms.Label newVersionLabel;
        private System.Windows.Forms.Label summaryLabel;
    }
}