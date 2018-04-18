using System.Drawing;

namespace WRInfo
{
    class Colour
    {
        public static Color WBlue = Color.FromArgb(33, 150, 243);
        public static Color WPurple = Color.FromArgb(103, 58, 183);
        public static Color WRed = Color.FromArgb(244, 67, 54);
        public static Color WYellow = Color.FromArgb(255, 235, 59);
        public static Color WGreen = Color.FromArgb(76, 175, 80);
        public static Color WOrange = Color.FromArgb(255, 152, 0);

        // Personal Rating colours
        // TODO: Change their value later DONT FORGET
        public static Color Bad = WRed;
        public static Color BelowAverage = Color.Orange;
        public static Color Average = WYellow;
        public static Color Good = Color.LightGreen;
        public static Color VeryGood = WGreen;
        public static Color Great = Color.Cyan;
        public static Color Unicum = Color.Magenta;
        public static Color SuperUnicum = Color.Purple;
    }
}
