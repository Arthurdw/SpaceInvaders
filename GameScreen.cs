using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
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

        private const int EntitiesPerRow = 11;
        private static int _entityAnimationIteration;
        private static bool _isGoingRight = true;
        public static int Speed => LivingEntities.Count;
        private static readonly Random Rn = new Random();
        public static int Score;
        public static int HighScore;
        public static bool HasSavedHighScore = false;

        public const int BaseDifficulty = 30;
        public static int Difficulty = BaseDifficulty;

        /// <summary>
        /// All the actions that should be taken for the next draw.
        /// </summary>
        public static List<Action<Panel, Graphics>> ActionBuffer = new List<Action<Panel, Graphics>>();

        /// <summary>
        /// A list of all the bullets that are currently on screen.
        /// </summary>
        public static List<Entities.Bullet> Bullets = new List<Entities.Bullet>();

        public static List<Entities.Entity> LivingEntities = new List<Entities.Entity>();
        public static List<Entities.Shield> Shields = new List<Entities.Shield>();
        private static readonly SoundPlayer Sp1 = new SoundPlayer("./assets/sound/move1.wav");
        private static readonly SoundPlayer Sp2 = new SoundPlayer("./assets/sound/move2.wav");
        private static readonly SoundPlayer Sp3 = new SoundPlayer("./assets/sound/move3.wav");
        private static readonly SoundPlayer Sp4 = new SoundPlayer("./assets/sound/move4.wav");
        private static readonly SoundPlayer SpShoot = new SoundPlayer("./assets/sound/shoot.wav");
        private static readonly SoundPlayer SpInvaderKilled = new SoundPlayer("./assets/sound/invaderkilled.wav");

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
            if (IsFirstInteraction) PerformStartup(pnl);

            CurrentYLocation = pnl.Height - 20 - Entities.Size;
            CurrentBarrelMiddle = CurrentXLocation + Entities.Size / 2 - (Entities.Size / 10) / 2;

            foreach (Entities.Shield shield in Shields)
                shield.Draw(g);

            if (IsPaused)
            {
                foreach (Entities.Bullet bullet in Bullets)
                    bullet.Draw(g);
            }
            else
            {
                if (WelcomeScreen.IsPlayingSong && WelcomeScreen.ScreenPassed)
                {
                    WelcomeScreen.SpSong.Stop();
                    WelcomeScreen.IsPlayingSong = false;
                }

                foreach (Action<Panel, Graphics> action in ActionBuffer)
                    action(pnl, g);

                ActionBuffer = new List<Action<Panel, Graphics>>();

                List<Entities.Bullet> removeBulletBuffer = new List<Entities.Bullet>();
                List<Entities.Entity> removeEntitiesBuffer = new List<Entities.Entity>();

                foreach (Entities.Bullet bullet in Bullets)
                {
                    if (bullet.Y <= 0 || bullet.Y >= pnl.Height) removeBulletBuffer.Add(bullet);
                    else
                    {
                        foreach (Entities.Entity entity in LivingEntities.Where(entity => bullet.ByPlayer && bullet.X >= entity.X && bullet.X <= entity.X + Entities.Size && entity.Y < bullet.Y && entity.Y + Entities.Size > bullet.Y))
                        {
                            removeEntitiesBuffer.Add(entity);
                            removeBulletBuffer.Add(bullet);
                        }

                        // Calculate bullet positions:
                        float bulletRight = bullet.X + (float)Entities.Size / 10 * (bullet.ByPlayer ? 1 : 2);
                        float bulletLeft = bullet.X - (float)Entities.Size / 10 * (bullet.ByPlayer ? 1 : 2);
                        float bulletBottom = bullet.Y + (bullet.ByPlayer ? (float)Entities.Size / 2 : (float)Entities.Size / 10 * 9);

                        if (!removeBulletBuffer.Contains(bullet))
                        {
                            foreach (Entities.Shield shield in Shields)
                            {
                                if (!(bulletRight >= shield.Rect.Left) || !(bulletLeft <= shield.Rect.Right) ||
                                    !(bulletBottom >= shield.Rect.Top) || bullet.Y > shield.Rect.Bottom) continue;
                                bool collision = true;
                                foreach (var _ in shield.ShotsTaken.Where(rectangle => bulletLeft >= rectangle.Left &&
                                    bulletRight <= rectangle.Right &&
                                    (bullet.ByPlayer ? bullet.Y >= rectangle.Top : bulletBottom <= rectangle.Bottom)))
                                    collision = false;

                                if (!collision) continue;
                                shield.ShotsTaken.Add(new Rectangle(
                                    bullet.X - (int)((bulletRight - bullet.X) * 2.5) / 2,
                                    bullet.Y + (bullet.ByPlayer ? -1 : 1) * (int)(bulletBottom - bullet.Y) - 6,
                                    (int)(bulletRight - bullet.X) * 3,
                                    (int)((bulletBottom - bullet.Y) * 1.25) + 12));
                                removeBulletBuffer.Add(bullet);
                                break;
                            }
                        }

                        if (removeBulletBuffer.Contains(bullet)) continue;

                        if (!bullet.ByPlayer)
                        {
                            if (bulletRight >= CurrentXLocation &&
                                bulletLeft <= CurrentXLocation + Entities.Size &&
                                bulletBottom >= CurrentYLocation + Entities.Size / 2 &&
                                bullet.Y <= CurrentYLocation + Entities.Size)
                            {
                                removeBulletBuffer.Add(bullet);
                                GameOverMenu.Enabled = true;
                            }
                        }
                        else
                        {
                            foreach (Entities.Bullet blt in Bullets.Where(blt => !blt.ByPlayer && !removeBulletBuffer.Contains(blt)).Where(blt => bullet.Y <= blt.Y + (float)Entities.Size / 10 * 9 &&
                                bullet.X >= blt.X - (float)Entities.Size / 10 * 2 && bullet.X <= blt.X + (float)Entities.Size / 10 * 2))
                            {
                                removeBulletBuffer.Add(blt);
                                removeBulletBuffer.Add(bullet);
                            }
                        }

                        bullet.PerformStep(g);
                    }
                }

                foreach (Entities.Bullet bullet in removeBulletBuffer)
                    Bullets.Remove(bullet);

                foreach (Entities.Entity entity in removeEntitiesBuffer)
                {
                    Score += entity.Worth;
                    LivingEntities.Remove(entity);
                    SpInvaderKilled.Play();
                }

                if (_entityAnimationIteration >= Speed * 2 && LivingEntities.Count != 0)
                {
                    // We need to get the bottom entities so we can let them shoot.
                    int maxRowCount = LivingEntities.Max(e => e.Row) + 1;
                    List<Entities.Entity>[] entitiesSortedByRow = new List<Entities.Entity>[maxRowCount];

                    foreach (Entities.Entity entity in LivingEntities)
                    {
                        if (entitiesSortedByRow[entity.Row] == null)
                            entitiesSortedByRow[entity.Row] = new List<Entities.Entity>();
                        entitiesSortedByRow[entity.Row].Add(entity);
                    }

                    List<Entities.Entity> bottomEntities = entitiesSortedByRow[maxRowCount - 1];
                    for (int i = maxRowCount - 2; i >= 0; i--)
                    {
                        if (bottomEntities.Count == EntitiesPerRow) break;
                        for (int j = 0; j < entitiesSortedByRow[i]?.Count; j++)
                        {
                            if (entitiesSortedByRow[i][j] == null) continue;
                            bool contains = bottomEntities.Any(e => e.X == entitiesSortedByRow[i][j].X);
                            if (!contains) bottomEntities.Add(entitiesSortedByRow[i][j]);
                        }
                    }

                    // Animation steps
                    bool lastDirection = _isGoingRight;

                    _isGoingRight = _isGoingRight
                        ? !(LivingEntities.Max(e => e.X) + Entities.Size / 12 >= pnl.Width - 20 - Entities.Size)
                        : LivingEntities.Min(e => e.X) - Entities.Size / 12 <= 10;

                    foreach (Entities.Entity entity in LivingEntities)
                    {
                        if (lastDirection != _isGoingRight) entity.Y += Entities.Size;
                        else entity.X += (_isGoingRight ? 1 : -1) * Entities.Size / 2;
                    }

                    foreach (Entities.Entity entity in bottomEntities.Where(entity => Rn.Next(0, Difficulty) == 0))
                        Bullets.Add(new Entities.Bullet(entity.X + Entities.Size / 2, entity.Y + Entities.Size, false));

                    _entityAnimationIteration = 0;
                    (LivingEntities.Count > 40 ? Sp1 : LivingEntities.Count > 30 ? Sp2 : LivingEntities.Count > 20 ? Sp3 : Sp4).Play();
                }
                _entityAnimationIteration++;
            }

            foreach (Entities.Entity entity in LivingEntities)
            {
                foreach (Entities.Shield shield in Shields)
                {
                    if (entity.Y + Entities.Size >= shield.Rect.Y && entity.X + Entities.Size >= shield.Rect.X &&
                        entity.X <= shield.Rect.Right && entity.Y <= shield.Rect.Bottom)
                    {
                        shield.ShotsTaken.Add(new Rectangle(entity.X, entity.Y, Entities.Size, Entities.Size));
                        break;
                    }
                }

                if (entity.Y + Entities.Size >= CurrentYLocation && entity.X + Entities.Size >= CurrentXLocation &&
                    entity.X <= CurrentXLocation + Entities.Size)
                    GameOverMenu.Enabled = true;
                entity.Draw(g, _entityAnimationIteration <= Speed);
            }

            DrawLaser(g);
            DrawGameOverlay(pnl, g);

            // Player won game
            if (LivingEntities.Count == 0)
            {
                PerformStartup(pnl);
                if (Difficulty != 0)
                    Difficulty--;
            }
        }

        private static void PerformStartup(Panel pnl)
        {
            EscapeMenu.HighlightedIndex = 0;
            CurrentXLocation = pnl.Width / 2 - Entities.Size / 2;
            Bullets = new List<Entities.Bullet>();
            ActionBuffer = new List<Action<Panel, Graphics>>();
            SpawnEntities(pnl);
            IsFirstInteraction = false;
        }

        private static void SpawnEntities(Panel pnl)
        {
            LivingEntities = new List<Entities.Entity>();
            Shields = new List<Entities.Shield>();

            _isGoingRight = true;
            _entityAnimationIteration = 0;
            (Entities.EntityType, int)[] entitiesConfig = {
                (Entities.EntityType.Squid, 1),
                (Entities.EntityType.Crab, 2),
                (Entities.EntityType.Octopus, 2)
            };

            const int startY = 50;
            const int startX = 10;
            int row = 0,
                jumpSizeX = ((pnl.Width - startX * 2) / 11 - Entities.Size) / 2,
                jumpSizeY = Entities.Size / 3,
                currentRow = row;

            foreach ((int idx, Entities.EntityType entity, int rows) in entitiesConfig.Select((tuple, i) => (i, tuple.Item1, tuple.Item2)))
            {
                int curX = startX;
                for (int i = 0; i < rows * EntitiesPerRow; i++)
                {
                    LivingEntities.Add(new Entities.Entity(curX + (Entities.Size + jumpSizeX) * i, startY +
                        (Entities.Size + jumpSizeY) * (idx + row), entity, currentRow));
                    if (i == EntitiesPerRow - 1 && rows > 1)
                    {
                        row++;
                        curX -= (Entities.Size + jumpSizeX) * (i + 1);
                        currentRow++;
                    }
                }

                currentRow++;
            }

            for (int i = 0; i < 4; i++)
                Shields.Add(new Entities.Shield(
                    (pnl.Width / 9) * (i + 1) + pnl.Width / 9 * i,
                    pnl.Height - pnl.Height / 8 - Entities.Size * 2,
                    pnl.Width / 9,
                    pnl.Width / 8));
        }

        /// <summary>
        /// Draws the laser, which the user controls.
        /// </summary>
        private static void DrawLaser(Graphics g)
        {
            g.FillRectangles(new SolidBrush(Config.Colors.Accent), new[]
            {
                new Rectangle(CurrentBarrelMiddle, CurrentYLocation + Entities.Size / 5, Entities.Size / 10, Entities.Size / 2),
                new Rectangle(CurrentXLocation, CurrentYLocation + Entities.Size / 2, Entities.Size, Entities.Size / 2)
            });
        }

        /// <summary>
        /// Let the laser shoot a bullet.
        /// </summary>
        public static void Shoot()
        {
            SpShoot.Play();
            Bullets.Add(new Entities.Bullet(CurrentBarrelMiddle, CurrentYLocation));
        }

        /// <summary>
        /// Reset the game.
        /// </summary>
        public static void Reset(Panel pnl, Game game)
        {
            IsPaused = false;
            game.Overlay = (_, __) => { };
            IsFirstInteraction = true;
            GameScreen.HasSavedHighScore = false;
            SpawnEntities(pnl);
        }

        private static void DrawGameOverlay(Panel pnl, Graphics g)
         => g.DrawString(string.Format(Config.Game.ScoreOverlay.ScoreMessage, Score, BaseDifficulty - Difficulty, HighScore, Config.CurrentUserName, DateTime.Now.ToLongTimeString()), Config.Font, new SolidBrush(Config.Colors.Primary), new RectangleF(0, 0, pnl.Width, Config.Font.Size * 2));
    }
}