using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public static class GameOverMenu
    {
        public static bool Enabled = false;
        private const int GoToHomeScreenSpeed = 40;
        private static int _currentIteration;

        public static void Draw(Panel pnl, Graphics g)
        {
            g.FillRectangle(Config.Game.EscapeMenu.Brush, 0, 0, pnl.Width, pnl.Height);
            g.DrawString(Config.Game.GameOverMenu.GameOverMessage, new Font(Config.FontFamily, (float)pnl.Height / 10), new SolidBrush(Config.Colors.Accent), new RectangleF(0, 0, pnl.Width, pnl.Height), Config.StringFormat);

            if (_currentIteration <= GoToHomeScreenSpeed)
                g.DrawString(Config.Game.GameOverMenu.GoHomeMessage, Config.Font, new SolidBrush(Config.Colors.Primary), new RectangleF(0, (float)pnl.Height / 5, pnl.Width, pnl.Height - (float)pnl.Height / 5), Config.StringFormat);

            _currentIteration = _currentIteration == GoToHomeScreenSpeed * 2 ? 0 : ++_currentIteration;
        }
    }
}