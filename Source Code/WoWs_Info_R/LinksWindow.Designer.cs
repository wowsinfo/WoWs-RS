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
            this.link_wows_info_ios = new System.Windows.Forms.LinkLabel();
            this.link_wows_official = new System.Windows.Forms.LinkLabel();
            this.link_wows_wiki = new System.Windows.Forms.LinkLabel();
            this.link_wows_number = new System.Windows.Forms.LinkLabel();
            this.link_sea_group = new System.Windows.Forms.LinkLabel();
            this.link_aslian_modpack = new System.Windows.Forms.LinkLabel();
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
            // link_wows_info_ios
            // 
            this.link_wows_info_ios.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.link_wows_info_ios.AutoSize = true;
            this.link_wows_info_ios.Location = new System.Drawing.Point(197, 99);
            this.link_wows_info_ios.Name = "link_wows_info_ios";
            this.link_wows_info_ios.Size = new System.Drawing.Size(124, 13);
            this.link_wows_info_ios.TabIndex = 1;
            this.link_wows_info_ios.TabStop = true;
            this.link_wows_info_ios.Text = "WoWs Info (iOS version)";
            this.link_wows_info_ios.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_wows_info_ios_LinkClicked);
            // 
            // link_wows_official
            // 
            this.link_wows_official.AutoSize = true;
            this.link_wows_official.Location = new System.Drawing.Point(197, 139);
            this.link_wows_official.Name = "link_wows_official";
            this.link_wows_official.Size = new System.Drawing.Size(117, 13);
            this.link_wows_official.TabIndex = 1;
            this.link_wows_official.TabStop = true;
            this.link_wows_official.Text = "WoWs Official Website";
            this.link_wows_official.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_wows_official_LinkClicked);
            // 
            // link_wows_wiki
            // 
            this.link_wows_wiki.AutoSize = true;
            this.link_wows_wiki.Location = new System.Drawing.Point(197, 180);
            this.link_wows_wiki.Name = "link_wows_wiki";
            this.link_wows_wiki.Size = new System.Drawing.Size(64, 13);
            this.link_wows_wiki.TabIndex = 1;
            this.link_wows_wiki.TabStop = true;
            this.link_wows_wiki.Text = "WoWs Wiki";
            this.link_wows_wiki.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_wows_wiki_LinkClicked);
            // 
            // link_wows_number
            // 
            this.link_wows_number.AutoSize = true;
            this.link_wows_number.Location = new System.Drawing.Point(197, 222);
            this.link_wows_number.Name = "link_wows_number";
            this.link_wows_number.Size = new System.Drawing.Size(80, 13);
            this.link_wows_number.TabIndex = 1;
            this.link_wows_number.TabStop = true;
            this.link_wows_number.Text = "WoWs Number";
            this.link_wows_number.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_wows_number_LinkClicked);
            // 
            // link_sea_group
            // 
            this.link_sea_group.AutoSize = true;
            this.link_sea_group.Location = new System.Drawing.Point(197, 265);
            this.link_sea_group.Name = "link_sea_group";
            this.link_sea_group.Size = new System.Drawing.Size(58, 13);
            this.link_sea_group.TabIndex = 1;
            this.link_sea_group.TabStop = true;
            this.link_sea_group.Text = "Sea Group";
            this.link_sea_group.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_sea_group_LinkClicked);
            // 
            // link_aslian_modpack
            // 
            this.link_aslian_modpack.AutoSize = true;
            this.link_aslian_modpack.Location = new System.Drawing.Point(197, 306);
            this.link_aslian_modpack.Name = "link_aslian_modpack";
            this.link_aslian_modpack.Size = new System.Drawing.Size(93, 13);
            this.link_aslian_modpack.TabIndex = 1;
            this.link_aslian_modpack.TabStop = true;
            this.link_aslian_modpack.Text = "ASLAIN Modpack";
            this.link_aslian_modpack.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_aslain_modpack_LinkClicked);
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
        private System.Windows.Forms.LinkLabel link_wows_info_ios;
        private System.Windows.Forms.LinkLabel link_wows_official;
        private System.Windows.Forms.LinkLabel link_wows_wiki;
        private System.Windows.Forms.LinkLabel link_wows_number;
        private System.Windows.Forms.LinkLabel link_sea_group;
        private System.Windows.Forms.LinkLabel link_aslian_modpack;
    }
}