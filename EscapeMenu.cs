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

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public static class EscapeMenu
    {
        public static Game Game;
        public static Panel Pnl;
        public static int HighlightedIndex;

        public static (string, Action)[] Items = {
            (Config.Game.EscapeMenu.GoBackToMainScreenMessage, () =>
            {
                GameScreen.Reset(Pnl, Game);
                WelcomeScreen.ScreenPassed = false;
                Game.Callback = WelcomeScreen.Draw;
            }),
            (Config.Game.EscapeMenu.ResetGameMessage, () => GameScreen.Reset(Pnl, Game)),
            (Config.Game.EscapeMenu.GoBackToGameMessage, () =>
            {
                Game.Overlay = (_, __) => { };
                GameScreen.IsPaused = false;
                HighlightedIndex = 0;
            }),
            (Config.Game.EscapeMenu.ExitGameMessage, () => Game.Close()),
        };

        public static void Draw(Panel pnl, Graphics g)
        {
            if (!WelcomeScreen.IsPlayingSong)
            {
                WelcomeScreen.SpSong.PlayLooping();
                WelcomeScreen.IsPlayingSong = true;
            }
            g.FillRectangle(Config.Game.EscapeMenu.Brush, 0, 0, pnl.Width, pnl.Height);
            g.DrawString(Config.Game.EscapeMenu.TopMessage, new Font(Config.FontFamily, (float)pnl.Height / 15), new SolidBrush(Config.Colors.Accent), new RectangleF(0, 0, pnl.Width, (float)pnl.Height / 5), Config.StringFormat);
            Pnl = pnl;

            float size = ((float)(pnl.Height - (pnl.Height / 15)) / (Items.Length + 2)) / 4;
            Font fnt = new Font(Config.FontFamily, size);
            Brush br = new SolidBrush(Config.Colors.Primary);
            float startAt = (float)(pnl.Height * 0.25);

            foreach ((int idx, string msg) in Items.Select((tuple, i) => (i, tuple.Item1)))
            {
                float yPos = startAt + fnt.Size * (idx + 1);
                if (idx == HighlightedIndex)
                    g.DrawString(msg, fnt, new SolidBrush(Color.FromArgb(32, Config.Colors.Primary)), new RectangleF(10, yPos + 5, pnl.Width - 10, yPos + fnt.Size), Config.StringFormat);

                g.DrawString(msg, fnt, br, new RectangleF(0, yPos, pnl.Width, yPos + fnt.Size), Config.StringFormat);
            }

            g.DrawString("Use the up/down or w/s\r\nkeys to select an item", new Font(Config.FontFamily, size / 2), br, new RectangleF(0, pnl.Height - size * 3 - (float)pnl.Height / 20, pnl.Width - (pnl.Width / 20), size * 3), new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Far
            });
        }
    }
}