using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RS
{
    public partial class RS : Form
    {
        public RS()
        {
            InitializeComponent();
        }

        private void githubMenu_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/HenryQuan/WoWs-RS");
        }
    }
}
