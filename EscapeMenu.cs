using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public static class EscapeMenu
    {
        public static Game Game;
        public static Panel Pnl;
        public static int HighlightedIndex;

        public static (string, Action)[] Items = {
            (Config.Game.EscapeMenu.GoBackToMainScreenMessage, () =>
            {
                GameScreen.Reset(Pnl, Game);
                WelcomeScreen.ScreenPassed = false;
                Game.Callback = WelcomeScreen.Draw;
            }),
            (Config.Game.EscapeMenu.ResetGameMessage, () => GameScreen.Reset(Pnl, Game)),
            (Config.Game.EscapeMenu.GoBackToGameMessage, () =>
            {
                Game.Overlay = (_, __) => { };
                GameScreen.IsPaused = false;
                HighlightedIndex = 0;
            }),
            (Config.Game.EscapeMenu.ExitGameMessage, () => Game.Close()),
        };

        public static void Draw(Panel pnl, Graphics g)
        {
            g.FillRectangle(Config.Game.EscapeMenu.Brush, 0, 0, pnl.Width, pnl.Height);
            g.DrawString(Config.Game.EscapeMenu.TopMessage, new Font(Config.FontFamily, (float)pnl.Height / 15), new SolidBrush(Config.Colors.Accent), new RectangleF(0, 0, pnl.Width, (float)pnl.Height / 5), Config.StringFormat);
            Pnl = pnl;

            float size = ((float)(pnl.Height - (pnl.Height / 15)) / (Items.Length + 2)) / 4;
            Font fnt = new Font(Config.FontFamily, size);
            Brush br = new SolidBrush(Config.Colors.Primary);
            float startAt = (float)(pnl.Height * 0.25);

            foreach ((int idx, string msg) in Items.Select((tuple, i) => (i, tuple.Item1)))
            {
                float yPos = startAt + fnt.Size * (idx + 1);
                if (idx == HighlightedIndex)
                    g.DrawString(msg, fnt, new SolidBrush(Config.Colors.Back), new RectangleF(10, yPos + 5, pnl.Width - 10, yPos + fnt.Size), Config.StringFormat);

                g.DrawString(msg, fnt, br, new RectangleF(0, yPos, pnl.Width, yPos + fnt.Size), Config.StringFormat);
            }

            g.DrawString("Use the up/down or w/s\r\nkeys to select an item", new Font(Config.FontFamily, size / 2), br, new RectangleF(0, pnl.Height - size * 3 - (float)pnl.Height / 20, pnl.Width - (pnl.Width / 20), size * 3), new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Far
            });
        }
    }
}