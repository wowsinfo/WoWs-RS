using System;
using System.Drawing;
using Console = Colorful.Console;

namespace WRInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteAscii("WRInfo 1.0", Color.FromArgb(33, 150, 243));
            Console.Read();
        }
    }
}
