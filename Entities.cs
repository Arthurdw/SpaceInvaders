using System;
using System.Drawing;

namespace SpaceInvaders
{
    public static class Entities
    {
        /// <summary>
        /// The general size (w/h) for entities.
        /// </summary>
        public static int Size = 50;

        /// <summary>
        /// All enemy entities types.
        /// </summary>
        public enum EntityType
        {
            Crab, Octopus, Squid
        }

        /// <summary>
        /// Represents an enemy entity.
        /// </summary>
        public class Entity
        {
            /// <summary>
            /// The entity its current horizontal location.
            /// </summary>
            public int X;
            /// <summary>
            /// The entity its current vertical location.
            /// </summary>
            public int Y;

            public int Row;
            /// <summary>
            /// The first frame for the animation.
            /// </summary>
            protected Image Shape;
            /// <summary>
            /// The second frame for the animation.
            /// </summary>
            protected Image Shape2;

            /// <summary>
            /// Creates a new entity.
            /// </summary>
            /// <param name="x">The horizontal start location for the entity</param>
            /// <param name="y">The vertical start location for the entity.</param>
            /// <param name="entityType">The type of entity.</param>
            public Entity(int x, int y, EntityType entityType, int row = 0)
            {
                this.X = x;
                this.Y = y;
                string entityName = Enum.GetName(typeof(EntityType), entityType);
                this.Shape = Image.FromFile($"./assets/{entityName}.png");
                this.Shape2 = Image.FromFile($"./assets/{entityName}2.png");
                this.Row = row;
            }

            /// <summary>
            /// Get a bitmap image for an image path and change the size so it matches the entity size.
            /// </summary>
            /// <param name="img">The asset.</param>
            /// <returns>A fully functional bitmap, which is ready to get used.</returns>
            public Bitmap GetBitmap(Image img)
            {
                float multiplicationFactor = (float) Size / (img.Height > img.Width ? img.Height : img.Width);

                Bitmap bm = new Bitmap(img, new Size(
                    (int)Math.Round(img.Width * multiplicationFactor),
                    (int)Math.Round(img.Height * multiplicationFactor)));

                return bm;
            }

            /// <summary>
            /// Draw the entity.
            /// This method requires you to specify if you want the first frame to be shown or the second.
            /// </summary>
            public void Draw(Graphics graphics, bool first)
                => graphics.DrawImage(this.GetBitmap(first ? this.Shape : this.Shape2), this.X, this.Y);
        }

        /// <summary>
        /// Represents a game bullet, which is shot from an entity or from a laser.
        /// </summary>
        public class Bullet
        {
            /// <summary>
            /// The current vertical location of the bullet
            /// </summary>
            public int Y { get; private set; }

            /// <summary>
            /// The current frame iteration for the bullet animations.
            /// </summary>
            private int _iteration;

            private int _subIteration;
            /// <summary>
            /// The horizontal location of the bullet.
            /// </summary>
            public int X;
            /// <summary>
            /// Whether or not the bullet was sent by the player or an enemy.
            /// </summary>
            public readonly bool ByPlayer;
            /// <summary>
            /// The size of the steps that the bullet takes, increase this to improve the speed of the bullets.
            /// </summary>
            public static int StepSize = 20;

            public static int AnimationSpeed = 10;

            /// <summary>
            /// Spawn a new bullet.
            /// </summary>
            /// <param name="x">The horizontal start location for the bullet. (THIS IS PERMANENT)</param>
            /// <param name="y">The vertical start location for the bullet.</param>
            /// <param name="byPlayer">Whether or not the bullet was sent by the player or an enemy.</param>
            public Bullet(int x, int y, bool byPlayer = true)
            {
                this.X = x;
                this.Y = y;
                this._iteration = 0;
                this.ByPlayer = byPlayer;
                this._subIteration = 0;
            }

            /// <summary>
            /// Draw the current bullet to the screen.
            /// </summary>
            public void Draw(Graphics g)
            {
                // TODO: Fix weird animation bugg
                RectangleF[] shape; 
                if (_iteration == AnimationSpeed * 2) _iteration = 0;

                if (ByPlayer) shape = new[] {new RectangleF(this.X, this.Y, (float) Size / 10, (float) Size / 2)};
                else {
                    float width = (float)Size / 10;
                    float height = (float)Size / 8;

                    float p = (float) AnimationSpeed / 4;
                    int animationStartValue = _iteration < p * 2
                        ? _iteration < p ? 0 : 1
                        : _iteration > p * 3 ? 2 : 3;

                    shape = new RectangleF[7];

                    for (int i = animationStartValue; i < animationStartValue + 7; i++)
                    {
                        int mod = i % 4;
                        shape[i - animationStartValue] = new RectangleF(this.X + (mod == 0 ? -width : ((mod == 1 || mod == 3) ? 0 : width)), 
                            this.Y + height * (i - animationStartValue), width, height);
                    }

                    _subIteration++;
                    
                    if (_subIteration == 3)
                    {
                        if (!GameScreen.IsPaused) _iteration++;
                        _subIteration = 0;
                    }
                }

                g.FillRectangles(new SolidBrush(Config.Colors.PrimaryDark), shape);
            }

            /// <summary>
            /// Perform a step with the bullet.
            /// </summary>
            public void PerformStep(Graphics g)
            {
                this.Y += ByPlayer ? -StepSize : StepSize / 10;
                this.Draw(g);
            }
        }
    }
}