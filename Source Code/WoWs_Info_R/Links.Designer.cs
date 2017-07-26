namespace WoWs_Info_R
{
    partial class LinksWindow
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
            this.label_intro = new System.Windows.Forms.Label();
            this.link_wows_info_ios = new Classes.Links("WoWs Info (iOS version)", "link_wows_info_ios", "https://itunes.apple.com/app/id1202750166", 197, 99);
            this.link_wows_official = new Classes.Links("WoWs Official Website", "link_wows_official", "https://worldofwarships.asia", 197, 139);
            this.link_wows_wiki = new Classes.Links("WoWs Wiki", "link_wows_wiki", "https://itunes.apple.com/app/id1202750166", 197, 179);
            this.link_wows_number = new Classes.Links("WoWs Number", "link_wows_number", "https://itunes.apple.com/app/id1202750166", 197, 219);
            this.link_sea_group = new Classes.Links("Sea Group", "link_sea_group", "https://itunes.apple.com/app/id1202750166", 197, 259);
            this.link_aslian_modpack = new Classes.Links("ASLAIN Modpack", "link_aslian_modpack", "https://itunes.apple.com/app/id1202750166", 197, 299);
            this.SuspendLayout();
            // 
            // label_intro
            // 
            this.label_intro.AutoSize = true;
            this.label_intro.Location = new System.Drawing.Point(43, 45);
            this.label_intro.Name = "label_intro";
            this.label_intro.Size = new System.Drawing.Size(247, 13);
            this.label_intro.TabIndex = 0;
            this.label_intro.Text = "Check out the links that are relavent to the WoWs:";
            // 
            // LinksWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 408);
            this.Controls.Add(this.link_aslian_modpack);
            this.Controls.Add(this.link_sea_group);
            this.Controls.Add(this.link_wows_number);
            this.Controls.Add(this.link_wows_wiki);
            this.Controls.Add(this.link_wows_official);
            this.Controls.Add(this.link_wows_info_ios);
            this.Controls.Add(this.label_intro);
            this.Name = "LinksWindow";
            this.Text = "Links";
            this.Load += new System.EventHandler(this.LinksWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_intro;
        private Classes.Links link_wows_info_ios;
        private Classes.Links link_wows_official;
        private Classes.Links link_wows_wiki;
        private Classes.Links link_wows_number;
        private Classes.Links link_sea_group;
        private Classes.Links link_aslian_modpack;
    }
}