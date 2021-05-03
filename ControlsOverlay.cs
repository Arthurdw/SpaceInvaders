using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public static class ControlsOverlay
    {
        public static void Draw(Panel pnl, Graphics g)
        {
            Font font = new Font(Config.FontFamily, (float)pnl.Width / 12);
            Color clr = Config.Colors.Accent;

            g.FillRectangle(new SolidBrush(Color.FromArgb(224, Config.Colors.PrimaryDarkest)), new Rectangle(0, 0, pnl.Width, pnl.Height));

            g.DrawString("CONTROLS", font,
                new SolidBrush(Color.FromArgb(64, clr)),
                new RectangleF(0 + 5, 0 + 5, pnl.Width, (float)(pnl.Height * 0.8)), Config.StringFormat);

            g.DrawString("CONTROLS", font,
                new SolidBrush(clr),
                new RectangleF(0, 0, pnl.Width, (float)(pnl.Height * 0.8)), Config.StringFormat);

            string[] controls = { "press space/enter to shoot", "press a/◀ to move left", "press d/▶ to move to the right" };
            Brush br = new SolidBrush(Config.Colors.Primary);
            for (int i = 0; i < controls.Length; i++)
                g.DrawString(controls[i], Config.Font, br, new RectangleF(0, 0, pnl.Width, pnl.Height + i *
                    (float)(Entities.Size * 1.5)), Config.StringFormat);
        }
    }
}