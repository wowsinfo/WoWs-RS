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
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void button_help_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/HenryQuan/WoWs_Info_R");
        }
    }
}
