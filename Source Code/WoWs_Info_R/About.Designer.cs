namespace WoWs_Info_R
{
    partial class About
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
            this.button_check_update = new System.Windows.Forms.Button();
            this.button_help = new System.Windows.Forms.Button();
            this.label_check_update = new System.Windows.Forms.Label();
            this.label_help = new System.Windows.Forms.Label();
            this.label_about = new System.Windows.Forms.Label();
            this.label_author = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_check_update
            // 
            this.button_check_update.Location = new System.Drawing.Point(298, 100);
            this.button_check_update.Name = "button_check_update";
            this.button_check_update.Size = new System.Drawing.Size(125, 23);
            this.button_check_update.TabIndex = 0;
            this.button_check_update.Text = "Check Updates...";
            this.button_check_update.UseVisualStyleBackColor = true;
            // 
            // button_help
            // 
            this.button_help.Location = new System.Drawing.Point(298, 178);
            this.button_help.Name = "button_help";
            this.button_help.Size = new System.Drawing.Size(125, 23);
            this.button_help.TabIndex = 0;
            this.button_help.Text = "Help (Github)";
            this.button_help.UseVisualStyleBackColor = true;
            this.button_help.Click += new System.EventHandler(this.button_help_Click);
            // 
            // label_check_update
            // 
            this.label_check_update.AutoSize = true;
            this.label_check_update.Location = new System.Drawing.Point(47, 105);
            this.label_check_update.Name = "label_check_update";
            this.label_check_update.Size = new System.Drawing.Size(245, 13);
            this.label_check_update.TabIndex = 1;
            this.label_check_update.Text = "Click the button to check updates for this program:";
            // 
            // label_help
            // 
            this.label_help.AutoSize = true;
            this.label_help.Location = new System.Drawing.Point(55, 182);
            this.label_help.Name = "label_help";
            this.label_help.Size = new System.Drawing.Size(236, 13);
            this.label_help.TabIndex = 1;
            this.label_help.Text = "Click the button to get the guide for this program:";
            // 
            // label_about
            // 
            this.label_about.AutoSize = true;
            this.label_about.Location = new System.Drawing.Point(164, 286);
            this.label_about.Name = "label_about";
            this.label_about.Size = new System.Drawing.Size(169, 13);
            this.label_about.TabIndex = 2;
            this.label_about.Text = "This program is under MIT license.";
            // 
            // label_author
            // 
            this.label_author.AutoSize = true;
            this.label_author.Location = new System.Drawing.Point(167, 325);
            this.label_author.Name = "label_author";
            this.label_author.Size = new System.Drawing.Size(165, 13);
            this.label_author.TabIndex = 3;
            this.label_author.Text = "Author: HenryQuan, MatchaCake";
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 393);
            this.Controls.Add(this.label_author);
            this.Controls.Add(this.label_about);
            this.Controls.Add(this.label_help);
            this.Controls.Add(this.label_check_update);
            this.Controls.Add(this.button_help);
            this.Controls.Add(this.button_check_update);
            this.Name = "About";
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_check_update;
        private System.Windows.Forms.Button button_help;
        private System.Windows.Forms.Label label_check_update;
        private System.Windows.Forms.Label label_help;
        private System.Windows.Forms.Label label_about;
        private System.Windows.Forms.Label label_author;
    }
}