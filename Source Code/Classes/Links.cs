using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Classes
{
    public class Links : LinkLabel
    {
        public string _Url { get; set; }
        public Links(string text,string name, string URL, int x, int y)
        {
            _Url = URL;
            AutoSize = true;
            Location = new System.Drawing.Point(x, y);
            Name = name;
            Size = new System.Drawing.Size(117, 13);
            TabIndex = 1;
            TabStop = true;
            Text = text;
            LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(link_Clicked);
        }

        private void link_Clicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(_Url);
        }
    }
}
