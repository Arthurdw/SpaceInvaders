using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace SpaceInvaders
{
    /// <summary>
    /// This contains the general game configuration.
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// The font that should be used throughout the game.
        /// </summary>
        public static Font Font;
        /// <summary>
        /// The font family of the font. (<see cref="Font"/>)
        /// </summary>
        public static FontFamily FontFamily;

        static Config()
        {
            // Get our custom font and assign the two font variables.
            PrivateFontCollection pfc = new PrivateFontCollection();
            int fontLength = Properties.Resources.MachineStd.Length;
            System.IntPtr data = Marshal.AllocCoTaskMem(fontLength);
            Marshal.Copy(Properties.Resources.MachineStd, 0, data, fontLength);
            pfc.AddMemoryFont(data, fontLength);

            FontFamily = pfc.Families[0];
            Font = new Font(FontFamily, 24);
        }

        /// <summary>
        /// The general color scheme for the game.
        /// </summary>
        public static class Colors
        {
            public static Color Back = Color.FromArgb(41, 50, 65);
            public static Color Accent = Color.FromArgb(239, 108, 77);
            public static Color Primary = Color.FromArgb(224, 251, 252);
            public static Color PrimaryDark = Color.FromArgb(152, 193, 217);
            public static Color PrimaryDarkest = Color.FromArgb(61, 90, 128);
        }
    }
}