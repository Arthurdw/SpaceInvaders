using System;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public static class GameScreen
    {
        public static int CurrentLocation;
        public static bool IsFirstInteraction = true;

        public static void Draw(Panel pnl, Graphics g)
        {
            Console.WriteLine(CurrentLocation);
            if (IsFirstInteraction)
            {
                CurrentLocation = pnl.Width / 2 - Entities.Size / 2;
                IsFirstInteraction = false;
            }

            g.FillRectangle(new SolidBrush(Config.Colors.Accent), CurrentLocation, 10, Entities.Size, 30);
        }
    }
}
