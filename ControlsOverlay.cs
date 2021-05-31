// MIT License

// Copyright (c) 2021 Arthurdw

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

// ---------------------------------------------------------------------------- //
//                                    ABOUT                                     //
//                                                                              //
//  This project was created for my final exams in GO-AO informatics (6INFO).   //
//  The task was to create a project which is a playable game in the .NET       //
//  framework, which utilizes the MySQL.Data dll.                               //
//                                                                              //
//  The project was finished on the 31'st of may 2021.                          //
//                                                                              //
// ---------------------------------------------------------------------------- //

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