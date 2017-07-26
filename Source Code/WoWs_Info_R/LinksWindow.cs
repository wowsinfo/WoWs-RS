using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WoWs_Info_R
{
    public partial class LinksWindow : Form
    {
        public LinksWindow()
        {
            InitializeComponent();
        }


        private void link_wows_info_ios_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://itunes.apple.com/app/id1202750166");
        }

        private void link_wows_official_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://worldofwarships.asia");
        }
        private void link_wows_wiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://wiki.wargaming.net/en/World_of_Warships");
        }
        private void link_wows_number_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://wows-numbers.com");
        }
        private void link_sea_group_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://sea-group.org");
        }
        private void link_aslain_modpack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://aslain.com/index.php?/topic/2020-0640-aslains-wows-modpack-installer-wpicture-preview");
        }

        private void LinksWindow_Load(object sender, EventArgs e)
        {
            
        }
    }
}
