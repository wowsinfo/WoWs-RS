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
