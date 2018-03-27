using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WRInfo
{
    class Info
    {
        private string name;
        private string server;

        public string Name { get => name; }
        public string Server { get => server; }

        public Info(string name, string server)
        {
            this.name = name;
            this.server = server;
        }

    }
}
