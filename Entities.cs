using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
        public enum EntityType: int
        {
            Octopus = 10,
            Crab = 20,
            Squid = 30
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
            public int Worth;

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
                this.Worth = (int) entityType;
            }

            /// <summary>
            /// Get a bitmap image for an image path and change the size so it matches the entity size.
            /// </summary>
            /// <param name="img">The asset.</param>
            /// <returns>A fully functional bitmap, which is ready to get used.</returns>
            public Bitmap GetBitmap(Image img)
            {
                float multiplicationFactor = (float)Size / (img.Height > img.Width ? img.Height : img.Width);

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
                // TODO: Fix weird animation bug
                RectangleF[] shape;
                if (_iteration == AnimationSpeed * 2) _iteration = 0;

                if (ByPlayer) shape = new[] { new RectangleF(this.X, this.Y, (float)Size / 10, (float)Size / 2) };
                else
                {
                    float width = (float)Size / 10;
                    float height = (float)Size / 8;

                    float p = (float)AnimationSpeed / 4;
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

        public class Shield
        {
            public Rectangle Rect;
            public List<Rectangle> ShotsTaken = new List<Rectangle>();

            public Shield(int x, int y, int width, int height)
            {
                this.Rect = new Rectangle(x, y, width, height);
            }

            public void Draw(Graphics g)
            {
                g.FillRectangle(new SolidBrush(Config.Colors.Accent), Rect);

                // TODO: Fix these optimizations:
                // if (ShotsTaken.Count)
                // {
                //     List<Rectangle> mergedShots = new List<Rectangle>();
                //     List<Rectangle> removeBuffer = new List<Rectangle>();

                // foreach (Rectangle rectangle in ShotsTaken)
                // {
                //     Rectangle rectangleBuffer = rectangle;
                //     foreach (Rectangle comp in ShotsTaken.Where(c => rectangle != c))
                //     {
                //         if (rectangle.Top == comp.Top && rectangle.Bottom == comp.Bottom)
                //         {
                //             bool isRight = comp.Left <= rectangle.Right && comp.Right >= rectangle.Right;
                //             if (isRight || comp.Right >= rectangle.Left && comp.Left <= rectangle.Left)
                //             {
                //                 rectangleBuffer.X = isRight ? rectangle.X : comp.X;
                //                 rectangleBuffer.Width = isRight
                //                     ? comp.X + comp.Width - rectangle.X
                //                     : rectangle.X + rectangle.Width - comp.X;
                //                 removeBuffer.Add(comp);
                //             }
                //         } else if (rectangle.Left == comp.Left && rectangle.Right == comp.Right)
                //         {
                //             bool isTop = comp.Top <= rectangle.Top && comp.Bottom >= rectangle.Top;
                //
                //             if (isTop || comp.Bottom >= rectangle.Bottom && comp.Top <= rectangle.Bottom)
                //             {
                //                 rectangleBuffer.Y = isTop ? comp.Y : rectangle.Y;
                //                 rectangleBuffer.Height =
                //                     isTop
                //                         ? comp.Y + comp.Height - rectangle.Y
                //                         : rectangle.Y + rectangle.Height - comp.Y;
                //             }
                //             removeBuffer.Add(comp);
                //         }


                // if (rectangle.Bottom == comp.Bottom && rectangle.Top == comp.Top && rectangle.X <= comp.X && rectangle.Right >= comp.X) 
                //     rectangleBuffer.Width = comp.X + comp.Width - rectangle.X;
                // else if (rectangle.X == comp.X && rectangle.Right == comp.Right && rectangle.Top <= comp.Top &&
                //          rectangle.Bottom >= comp.Top)
                //     rectangleBuffer.Height = comp.Y + comp.Height - rectangle.Y;
                // else removeBuffer.Add(comp);
                // }

                // Console.WriteLine(rectangleBuffer.Width);
                //     mergedShots.Add(rectangleBuffer);
                // }

                //     foreach (Rectangle rectangle in removeBuffer)
                //         mergedShots.Remove(rectangle);
                //
                //     Console.WriteLine(mergedShots.Count);
                //
                //     ShotsTaken = mergedShots;
                // }

                // TODO: Fix these optimizations:
                // List<Rectangle> mergedShots = new List<Rectangle>();
                // List<Rectangle> removeBuffer = new List<Rectangle>();
                //
                // foreach (Rectangle rectangle in ShotsTaken)
                // {
                //     Rectangle rectangleBuffer = rectangle;
                //     foreach (var comp in ShotsTaken.Where(c => rectangle != c))
                //     {
                //         if (rectangle.Bottom == comp.Bottom && rectangle.X <= comp.X && rectangle.X + rectangle.Width >= comp.X)
                //         {
                //             rectangleBuffer.Width = comp.X + comp.Width - rectangle.X;
                //             removeBuffer.Add(comp);
                //         }
                //     }
                //     mergedShots.Add(rectangleBuffer);
                // }
                //
                // foreach (Rectangle rectangle in removeBuffer)
                //     mergedShots.Remove(rectangle);
                //
                // Console.WriteLine(mergedShots.Count);
                // ShotsTaken = mergedShots;

                Brush br = new SolidBrush(Config.Colors.PrimaryDarkest);
                foreach (Rectangle rectangle in ShotsTaken)
                    g.FillRectangle(br, rectangle);
            }
        }
    }
}