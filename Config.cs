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
        public static int Id = 0;
        public static string CurrentUserName = "<username>";
        public static bool ResponsivenessEnabled = true;
        public static bool ShouldDie = false;
        public static bool IsAdmin = false;

        /// <summary>
        /// The font that should be used throughout the game.
        /// </summary>
        public static Font Font => new Font(FontFamily, (float)Entities.Size / 2);

        /// <summary>
        /// The font family of the font. (<see cref="Font"/>)
        /// </summary>
        public static FontFamily FontFamily;

        /// <summary>
        /// General project its string format. (centered)
        /// </summary>
        public static StringFormat StringFormat = new StringFormat
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Center
        };

        static Config()
        {
            // Get our custom font and assign the two font variables.
            PrivateFontCollection pfc = new PrivateFontCollection();
            int fontLength = Properties.Resources.MachineStd.Length;
            System.IntPtr data = Marshal.AllocCoTaskMem(fontLength);
            Marshal.Copy(Properties.Resources.MachineStd, 0, data, fontLength);
            pfc.AddMemoryFont(data, fontLength);

            FontFamily = pfc.Families[0];
        }

        public static class Themes
        {
            public static class Default
            {
                public static Color Back = Color.FromArgb(41, 50, 65);
                public static Color Accent = Color.FromArgb(239, 108, 77);
                public static Color Primary = Color.FromArgb(224, 251, 252);
                public static Color PrimaryDark = Color.FromArgb(152, 193, 217);
                public static Color PrimaryDarkest = Color.FromArgb(61, 90, 128);
            }

            public static class Retro
            {
                public static Color Back = Color.FromArgb(0, 0, 0);
                public static Color Accent = Color.FromArgb(16, 255, 35);
                public static Color Primary = Color.FromArgb(255, 255, 255);
                public static Color PrimaryDark = Color.FromArgb(128, 255, 255, 255);
                public static Color PrimaryDarkest = Color.FromArgb(0, 0, 0);
            }

            public static class Stylish
            {
                public static Color Back = Color.FromArgb(0, 0, 0);
                public static Color Accent = Color.FromArgb(252, 163, 17);
                public static Color Primary = Color.FromArgb(255, 255, 255);
                public static Color PrimaryDark = Color.FromArgb(229, 229, 229);
                public static Color PrimaryDarkest = Color.FromArgb(20, 33, 61);
            }
        }

        /// <summary>
        /// The general color scheme for the game.
        /// </summary>
        public static class Colors
        {
            public static Color Back = Themes.Default.Back;
            public static Color Accent = Themes.Default.Accent;
            public static Color Primary = Themes.Default.Primary;
            public static Color PrimaryDark = Themes.Default.PrimaryDark;
            public static Color PrimaryDarkest = Themes.Default.PrimaryDarkest;
        }

        public static class Game
        {
            public static class WelcomeScreen
            {
                public static string GameTitleMessage = "SPACE\r\nINVADERS";

                public static string ContinueToGameMessage = "Press space/enter...\r\n" +
                                                             "Press ? to see the controls\r\n" +
                                                             "Press c to update the settings\r\n" +
                                                             "Press s to see all the statistics";

                public static Entities.EntityType TopEntityType = Entities.EntityType.Octopus;
                public static Entities.EntityType BottomEntityType = Entities.EntityType.Crab;
            }

            public static class EscapeMenu
            {
                public static string TopMessage = "Paused - Menu";
                public static Color BackgroundColor = Color.FromArgb(50, Colors.Back);
                public static Brush Brush = new SolidBrush(BackgroundColor);

                public static string GoBackToMainScreenMessage = "Main screen";
                public static string ResetGameMessage = "reset game";
                public static string GoBackToGameMessage = "BACK";
                public static string ExitGameMessage = "Exit";
            }

            public static class GameOverMenu
            {
                public static string GameOverMessage = "GAME OVER";
                public static string GoHomeMessage = "Press space/enter to go back home";
                public static string SaveScoreMessage = "Press s to save your score";
                public static string ScoreSavedMessage = "Score has been saved";
                public static string ScoreMessage = "Score: {0} | Difficulty: {1}";
            }

            public static class ScoreOverlay
            {
                public static string ScoreMessage = "Score: {0} | Difficulty: {1} | High Score: {2} | {3} | {4}";
            }
        }

        public static class Credentials
        {
            public static string DbUsername = "06InfoArthur";
            public static string DbDatabase = "06InfoArthur";
            public static string DbPassword = "theWhiteArthur@6@21";
            public static string DbServer = "arthur.go-ao.be";
        }

        public static class LoginRegister
        {
            public static string LoginButtonText = "Sign In";
            public static string RegisterButtonText = "Register";

            public static string LoginLabelText = "Don't have an account? Create one!";
            public static string RegisterLabelText = "Already have an account? Sign in!";
        }
    }
}