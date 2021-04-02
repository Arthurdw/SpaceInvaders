using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    /// <summary>
    /// This is the screen which gets shown when the client opens the game.
    /// </summary>
    public static class WelcomeScreen
    {
        /// <summary>
        /// The current amount of pixels the moving entities are currently at. (horizontally)
        /// </summary>
        public static int EntitiesLeft;
        /// <summary>
        /// Whether or not the entities animation is currently the first frame.
        /// </summary>
        public static bool EntitiesFirst;
        /// <summary>
        /// The current iteration (amount of times the screen has updated).
        /// </summary>
        public static int EntitiesIteration;
        /// <summary>
        /// A separate iteration count for the `Press enter/space to play` message. Same principle as the <see cref="EntitiesIteration"/>
        /// </summary>
        public static int PressEnterToPlayIteration;
        /// <summary>
        /// Whether or not the welcome screen has passed.
        /// </summary>
        public static bool ScreenPassed;

        static WelcomeScreen()
        {
            EntitiesFirst = true;
            ScreenPassed = false;
        }

        /// <summary>
        /// Paints the WelcomeScreen to the user.
        /// </summary>
        public static void Draw(Panel pnl, Graphics g)
        {
            if (EntitiesIteration == 8)
            {
                EntitiesLeft += Entities.Size / 8;
                if (EntitiesLeft >= pnl.Width / 10 - Entities.Size)
                    EntitiesLeft = -Entities.Size;

                EntitiesFirst = !EntitiesFirst;
                EntitiesIteration = 0;
            }
            
            for (int i = 0; i < 11; i++)
            {
                int x = (pnl.Width / 10 - Entities.Size) * i + Entities.Size * i;
                new Entities.Entity(EntitiesLeft + x, 10, Entities.EntityType.Octopus).Draw(g, EntitiesFirst);
                new Entities.Entity(-EntitiesLeft + x - Entities.Size, pnl.Height - Entities.Size, Entities.EntityType.Crab).Draw(g, EntitiesFirst);
            }
            
            string message = "SPACE\r\nINVADERS";
            StringFormat sf = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };

            Font font = new Font(Config.FontFamily, (float)pnl.Width / 12);
            Color clr = Config.Colors.Accent;

            g.DrawString(message, font,
                new SolidBrush(Color.FromArgb(64, clr)),
                new RectangleF(0 + 5, 0 + 5, pnl.Width, pnl.Height), sf);

            g.DrawString(message, font,
                new SolidBrush(clr),
                new RectangleF(0, 0, pnl.Width, pnl.Height), sf);

            if (PressEnterToPlayIteration >= 15)
                g.DrawString("Press space/enter...", Config.Font,
                    new SolidBrush(Config.Colors.Primary),
                    new RectangleF(0, (float)pnl.Height / 3, pnl.Width, pnl.Height), sf);

            if (PressEnterToPlayIteration >= 30) PressEnterToPlayIteration = 0;
            else PressEnterToPlayIteration++;

            EntitiesIteration++;

            pnl.BackColor = Config.Colors.PrimaryDarkest;
        }
    }
}
