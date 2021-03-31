using System;
using System.Drawing;

namespace SpaceInvaders
{
    public static class Entities
    {
        public static int Size = 50;

        public enum EntityType
        {
            Crab, Octopus
        }

        public class Entity
        {
            protected int X;
            protected int Y;
            protected Bitmap Shape;
            protected Bitmap Shape2;

            private bool _isFirst;

            public Entity(int x, int y, EntityType entityType)
            {
                this.X = x;
                this.Y = y;
                string name = Enum.GetName(typeof(EntityType), entityType);
                this.Shape = this.GetBitmap($"./assets/{name}.png");
                this.Shape2 = this.GetBitmap($"./assets/{name}2.png");
                this._isFirst = true;
            }

            public Bitmap GetBitmap(string imgPath)
            {
                Image img = Image.FromFile(imgPath);
                float multiplicationFactor = (float)Size / (img.Height > img.Width ? img.Height : img.Width);

                Bitmap bm = new Bitmap(img, new Size(
                    (int)Math.Round(img.Width * multiplicationFactor),
                    (int)Math.Round(img.Height * multiplicationFactor)));

                return bm;
            }

            public void Draw(Graphics graphics)
            {
                this.Draw(graphics, this._isFirst);
                this._isFirst = !this._isFirst;
            }

            public void Draw(Graphics graphics, bool first)
                => graphics.DrawImage(first ? this.Shape : this.Shape2, this.X, this.Y);
        }
    }
}