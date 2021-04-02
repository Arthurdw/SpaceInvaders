using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    /// <summary>
    /// Handles the whole screen which gets displayed when a user plays the game.
    /// </summary>
    public static class GameScreen
    {
        /// <summary>
        /// The current Y location for the laser.
        /// </summary>
        public static int CurrentYLocation;
        /// <summary>
        /// The current X location for the laser.
        /// </summary>
        public static int CurrentXLocation;
        /// <summary>
        /// The current X location for the barrel middle.
        /// </summary>
        public static int CurrentBarrelMiddle;
        /// <summary>
        /// Whether or not the client has already moved the laser.
        /// This when its the first interaction the laser location gets calculated.
        /// </summary>
        public static bool IsFirstInteraction = true;
        /// <summary>
        /// The general  brush for the laser.
        /// </summary>
        public static Brush Br = new SolidBrush(Config.Colors.Accent);

        /// <summary>
        /// All the actions that should be taken for the next draw.
        /// </summary>
        public static List<Action<Panel, Graphics>> ActionBuffer = new List<Action<Panel, Graphics>>();
        /// <summary>
        /// A list of all the bullets that are currently on screen.
        /// </summary>
        public static List<Entities.Bullet> Bullets = new List<Entities.Bullet>();

        /// <summary>
        /// Draws the GameScreen.
        ///
        /// SIDE EFFECTS:
        ///     1. Checks whether or not its the first interaction, if it is it will calculate the current x location for the laser.
        ///     2. Executes all actions which are in the action buffer.
        ///     3. Updates all bullets, if a bullet is out of bounds it will remove this bullet from the Bullets collection.
        /// </summary>
        public static void Draw(Panel pnl, Graphics g)
        {
            if (IsFirstInteraction)
            {
                CurrentXLocation = pnl.Width / 2 - Entities.Size / 2;
                IsFirstInteraction = false;
            }

            CurrentYLocation = pnl.Height - 20 - Entities.Size;
            CurrentBarrelMiddle = CurrentXLocation + Entities.Size / 2 - (Entities.Size / 10) / 2;

            DrawLaser(g);

            foreach (Action<Panel, Graphics> action in ActionBuffer)
                action(pnl, g);

            ActionBuffer = new List<Action<Panel, Graphics>>();

            List<Entities.Bullet> removeBuffer = new List<Entities.Bullet>();

            foreach (Entities.Bullet bullet in Bullets)
            {
                if (bullet.Y <= 0) removeBuffer.Add(bullet);
                else bullet.PerformStep(g);
            }

            foreach (Entities.Bullet bullet in removeBuffer)
                Bullets.Remove(bullet);
        }

        /// <summary>
        /// Draws the laser, which the user controls.
        /// </summary>
        private static void DrawLaser(Graphics g)
        {
            g.FillRectangles(Br, new []
            {
                new Rectangle(CurrentBarrelMiddle, CurrentYLocation + Entities.Size / 5, Entities.Size / 10, Entities.Size / 2),
                new Rectangle(CurrentXLocation, CurrentYLocation + Entities.Size / 2, Entities.Size, Entities.Size / 2)
            });
        }

        /// <summary>
        /// Let the laser shoot a bullet.
        /// </summary>
        public static void Shoot()
            => Bullets.Add(new Entities.Bullet(CurrentBarrelMiddle, CurrentYLocation));

    }
}
