using System.Drawing;
using System.Media;
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

        public static bool IsPlayingSong;

        public static int EntitiesIterationSpeed = 8;
        public static readonly SoundPlayer SpSong = new SoundPlayer("./assets/sound/song.wav");

        static WelcomeScreen()
        {
            EntitiesFirst = true;
            ScreenPassed = false;
            IsPlayingSong = false;
        }

        /// <summary>
        /// Paints the WelcomeScreen to the user.
        /// </summary>
        public static void Draw(Panel pnl, Graphics g)
        {
            if (!IsPlayingSong && !ScreenPassed)
            {
                SpSong.PlayLooping();
                IsPlayingSong = true;
            }
            if (EntitiesIteration == EntitiesIterationSpeed)
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
                new Entities.Entity(EntitiesLeft + x, 10, Config.Game.WelcomeScreen.TopEntityType).Draw(g, EntitiesFirst);
                new Entities.Entity(-EntitiesLeft + x - Entities.Size, pnl.Height - Entities.Size, Config.Game.WelcomeScreen.BottomEntityType).Draw(g, EntitiesFirst);
            }

            Font font = new Font(Config.FontFamily, (float)pnl.Width / 12);
            Color clr = Config.Colors.Accent;

            g.DrawString(Config.Game.WelcomeScreen.GameTitleMessage, font,
                new SolidBrush(Color.FromArgb(64, clr)),
                new RectangleF(0 + 5, 0 + 5, pnl.Width, pnl.Height), Config.StringFormat);

            g.DrawString(Config.Game.WelcomeScreen.GameTitleMessage, font,
                new SolidBrush(clr),
                new RectangleF(0, 0, pnl.Width, pnl.Height), Config.StringFormat);

            if (PressEnterToPlayIteration >= 15)
                g.DrawString(Config.Game.WelcomeScreen.ContinueToGameMessage, Config.Font,
                    new SolidBrush(Config.Colors.Primary),
                    new RectangleF(0, (float)pnl.Height / 3, pnl.Width, pnl.Height), Config.StringFormat);

            if (PressEnterToPlayIteration >= 30) PressEnterToPlayIteration = 0;
            else PressEnterToPlayIteration++;

            EntitiesIteration++;

            pnl.BackColor = Config.Colors.PrimaryDarkest;
        }
    }
}