using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public static class EscapeMenu
    {
        public static Game Game;
        public static Brush Brush = new SolidBrush(Config.Game.EscapeMenu.BackgroundColor);
        public static int HighlightedIndex;
        public static Action[] Actions;


        public static void Draw(Panel pnl, Graphics g)
        {
            g.FillRectangle(Brush, 0, 0, pnl.Width, pnl.Height);
            g.DrawString(Config.Game.EscapeMenu.TopMessage, new Font(Config.FontFamily, (float) pnl.Height / 15), new SolidBrush(Config.Colors.Accent), new RectangleF(0, 0, pnl.Width, (float) pnl.Height / 5), Config.StringFormat);

            // TODO: Make the actions actually usefull
            (string, Action)[] items = {
                (Config.Game.EscapeMenu.GoBackToMainScreenMessage, () => Console.WriteLine(@"Main screen")),
                (Config.Game.EscapeMenu.GoBackToGameMessage, () => Console.WriteLine(@"GoBackToGame")),
                (Config.Game.EscapeMenu.ExitGameMessage, () => Console.WriteLine(@"Exit")),
            };

            Actions = new Action[items.Length];

            Font fnt = new Font(Config.FontFamily, (float) (pnl.Height - pnl.Height / 15) / (items.Length + 2));
            // TODO: Get color from config.
            Brush br = new SolidBrush(Color.Red);
            float startAt = (float) (pnl.Height * 0.8);

            foreach ((int idx, (string msg, Action callback)) in items.Select((tuple, i) => (i, tuple)))
            {
                // TODO: Fix this
                float yPos = startAt * (idx + 1);
                g.DrawString(msg, fnt, br, new RectangleF(0, yPos, pnl.Width, pnl.Height - startAt - yPos), Config.StringFormat);
                Actions[idx] = callback;
            }
        }
    }
}
