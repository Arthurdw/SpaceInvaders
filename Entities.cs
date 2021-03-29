using System;
using System.Drawing;

namespace SpaceInvaders
{
    public static class Entities
    {
        public enum EntityType
        {
            Octopus
        }

        public class Entity
        {
            private const int Size = 50;

            protected int X;
            protected int Y;
            protected Bitmap Shape;

            public Entity(int x, int y, EntityType entityType)
            {
                this.X = x;
                this.Y = y;
                this.Shape = this.GetBitmap($"./assets/{Enum.GetName(typeof(EntityType), entityType)}.png");
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
                => graphics.DrawImage(this.Shape, this.X, this.Y);
        }
    }
}