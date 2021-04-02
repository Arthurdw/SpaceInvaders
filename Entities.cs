﻿using System;
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
            protected int X;
            /// <summary>
            /// The entity its current vertical location.
            /// </summary>
            protected int Y;
            /// <summary>
            /// The first frame for the animation.
            /// </summary>
            protected Bitmap Shape;
            /// <summary>
            /// The second frame for the animation.
            /// </summary>
            protected Bitmap Shape2;

            /// <summary>
            /// Helper variable for the frame animations, to check whether or not the first frame should currently be shown.
            /// </summary>
            private bool _isFirst;

            /// <summary>
            /// Creates a new entity.
            /// </summary>
            /// <param name="x">The horizontal start location for the entity</param>
            /// <param name="y">The vertical start location for the entity.</param>
            /// <param name="entityType">The type of entity.</param>
            public Entity(int x, int y, EntityType entityType)
            {
                this.X = x;
                this.Y = y;
                string entityName = Enum.GetName(typeof(EntityType), entityType);
                this.Shape = this.GetBitmap($"./assets/{entityName}.png");
                this.Shape2 = this.GetBitmap($"./assets/{entityName}2.png");
                this._isFirst = true;
            }

            /// <summary>
            /// Get a bitmap image for an image path and change the size so it matches the entity size.
            /// </summary>
            /// <param name="imgPath">The src location for the asset.</param>
            /// <returns>A fully functional bitmap, which is ready to get used.</returns>
            public Bitmap GetBitmap(string imgPath)
            {
                Image img = Image.FromFile(imgPath);
                float multiplicationFactor = (float) Size / (img.Height > img.Width ? img.Height : img.Width);

                Bitmap bm = new Bitmap(img, new Size(
                    (int)Math.Round(img.Width * multiplicationFactor),
                    (int)Math.Round(img.Height * multiplicationFactor)));

                return bm;
            }

            /// <summary>
            /// Draw the entity, this will make use of the inner helper methods to check on which frame the animation is.
            /// </summary>
            public void Draw(Graphics graphics)
            {
                this.Draw(graphics, this._isFirst);
                this._isFirst = !this._isFirst;
            }

            /// <summary>
            /// Draw the entity.
            /// This method requires you to specify if you want the first frame to be shown or the second.
            /// </summary>
            public void Draw(Graphics graphics, bool first)
                => graphics.DrawImage(first ? this.Shape : this.Shape2, this.X, this.Y);
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
            /// <summary>
            /// The horizontal location of the bullet.
            /// </summary>
            private readonly int _x;
            /// <summary>
            /// Whether or not the bullet was sent by the player or an enemy.
            /// </summary>
            private readonly bool _byPlayer;

            /// <summary>
            /// Spawn a new bullet.
            /// </summary>
            /// <param name="x">The horizontal start location for the bullet. (THIS IS PERMANENT)</param>
            /// <param name="y">The vertical start location for the bullet.</param>
            /// <param name="byPlayer">Whether or not the bullet was sent by the player or an enemy.</param>
            public Bullet(int x, int y, bool byPlayer = true)
            {
                this._x = x;
                this.Y = y;
                this._iteration = 0;
                this._byPlayer = byPlayer;
            }

            /// <summary>
            /// Draw the current bullet to the screen.
            /// </summary>
            public void Draw(Graphics g)
                => g.FillRectangle(new SolidBrush(Color.Red), this._x, this.Y, 2, 2);

            /// <summary>
            /// Perform a step with the bullet.
            /// </summary>
            public void PerformStep(Graphics g)
            {
                this.Y += _byPlayer ? -1 : 1;
                this.Draw(g);
            }
        }
    }
}