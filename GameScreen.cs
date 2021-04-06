using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        /// Whether or not the game is currently paused.
        /// </summary>
        public static bool IsPaused = false;
        /// <summary>
        /// The general  brush for the laser.
        /// </summary>
        public static Brush Br = new SolidBrush(Config.Colors.Accent);

        private const int EntitiesPerRow = 11;
        private static int _entityAnimationIteration;
        private static bool _isGoingRight = true;
        public static int Speed = 15;

        /// <summary>
        /// All the actions that should be taken for the next draw.
        /// </summary>
        public static List<Action<Panel, Graphics>> ActionBuffer = new List<Action<Panel, Graphics>>();
        /// <summary>
        /// A list of all the bullets that are currently on screen.
        /// </summary>
        public static List<Entities.Bullet> Bullets = new List<Entities.Bullet>();

        public static List<Entities.Entity> LivingEntities = new List<Entities.Entity>();

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
            if (IsFirstInteraction) PerformStartup(pnl, g);

            CurrentYLocation = pnl.Height - 20 - Entities.Size;
            CurrentBarrelMiddle = CurrentXLocation + Entities.Size / 2 - (Entities.Size / 10) / 2;

            DrawLaser(g);

            foreach (Entities.Entity entity in LivingEntities)
                entity.Draw(g, _entityAnimationIteration <= Speed);

            if (IsPaused)
            {
                foreach (Entities.Bullet bullet in Bullets)
                    bullet.Draw(g);
            }
            else
            {
                foreach (Action<Panel, Graphics> action in ActionBuffer)
                    action(pnl, g);

                ActionBuffer = new List<Action<Panel, Graphics>>();

                List<Entities.Bullet> removeBulletBuffer = new List<Entities.Bullet>();
                List<Entities.Entity> removeEntitiesBuffer = new List<Entities.Entity>();

                foreach (Entities.Bullet bullet in Bullets)
                {
                    if (bullet.Y <= 0) removeBulletBuffer.Add(bullet);
                    else
                    {
                        foreach (Entities.Entity entity in LivingEntities)
                        {
                            if (bullet.X >= entity.X && bullet.X <= entity.X + Entities.Size &&
                                entity.Y < bullet.Y && entity.Y + Entities.Size > bullet.Y)
                            {
                                removeEntitiesBuffer.Add(entity);
                                removeBulletBuffer.Add(bullet);
                            }
                        }
                        bullet.PerformStep(g);
                    }
                }

                foreach (Entities.Bullet bullet in removeBulletBuffer)
                    Bullets.Remove(bullet);

                foreach (Entities.Entity entity in removeEntitiesBuffer)
                    LivingEntities.Remove(entity);

                if (_entityAnimationIteration >= Speed * 2 && LivingEntities.Count != 0)
                {
                    bool lastDirection = _isGoingRight;

                    _isGoingRight = _isGoingRight 
                        ? !(LivingEntities.Max(e => e.X) + Entities.Size / 12 >= pnl.Width - 20 - Entities.Size) 
                        : LivingEntities.Min(e => e.X) - Entities.Size / 12 <= 10;

                    foreach (Entities.Entity entity in LivingEntities)
                    {
                        if (lastDirection != _isGoingRight) entity.Y += Entities.Size;
                        else entity.X += (_isGoingRight ? 1 : -1) * Entities.Size / 2;
                    }

                    _entityAnimationIteration = 0;
                }
                _entityAnimationIteration++;
            }
        }

        private static void PerformStartup(Panel pnl, Graphics g)
        {
            EscapeMenu.HighlightedIndex = 0;
            Bullets = new List<Entities.Bullet>();
            ActionBuffer = new List<Action<Panel, Graphics>>();
            CurrentXLocation = pnl.Width / 2 - Entities.Size / 2;
            SpawnEntities(pnl);
            IsFirstInteraction = false;
        }

        private static void SpawnEntities(Panel pnl)
        {
            LivingEntities = new List<Entities.Entity>();
            _isGoingRight = true;
            _entityAnimationIteration = 0;
            (Entities.EntityType, int)[] entitiesConfig = new[]
            {
                (Entities.EntityType.Squid, 1),
                (Entities.EntityType.Crab, 2),
                (Entities.EntityType.Octopus, 2)
            };

            int startY = 50;
            int startX = 10;
            int row = 0;
            int jumpSizeX = ((pnl.Width - startX * 2) / 11 - Entities.Size) / 2;
            int jumpSizeY = Entities.Size / 3;

            foreach ((int idx, Entities.EntityType entity, int rows) in entitiesConfig.Select((tuple, i) => (i, tuple.Item1, tuple.Item2)))
            {
                int curX = startX;
                for (int i = 0; i < rows * EntitiesPerRow; i++)
                {
                    LivingEntities.Add(new Entities.Entity(curX + (Entities.Size + jumpSizeX) * i, startY +
                        (Entities.Size + jumpSizeY) * (idx + row), entity));
                    if (i == EntitiesPerRow - 1 && rows > 1)
                    {
                        row++;
                        curX -= (Entities.Size + jumpSizeX) * (i + 1);
                    }
                }
                
            }
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

        /// <summary>
        /// Reset the game.
        /// </summary>
        public static void Reset(Panel pnl, Game game)
        {
            IsPaused = false;
            game.Overlay = (_, __) => { };
            IsFirstInteraction = true;
            SpawnEntities(pnl);
        }

    }
}
