using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public static class GameOverMenu
    {
        public static bool Enabled = false;

        public static void Draw(Panel pnl, Graphics g)
        {
            g.FillRectangle(Config.Game.EscapeMenu.Brush, 0, 0, pnl.Width, pnl.Height);
            g.DrawString("GAME OVER", new Font(Config.FontFamily, (float)pnl.Height / 10), new SolidBrush(Config.Colors.Accent), new RectangleF(0, 0, pnl.Width, pnl.Height), Config.StringFormat);
            // TODO: IMPLEMENT LITTLE MENU
        }
    }
}