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
using System.Media;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public static class GameOverMenu
    {
        public static bool Enabled = false;
        private const int GoToHomeScreenSpeed = 40;
        private static int _currentIteration;
        private static bool _isFirst = true;
        private static DateTime _startSongIn = DateTime.MaxValue;
        private static readonly SoundPlayer SpPlayerDeath = new SoundPlayer("./assets/sound/explosion.wav");

        public static void Draw(Panel pnl, Graphics g)
        {
            if (_isFirst)
            {
                SpPlayerDeath.Play();
                _isFirst = false;
                _startSongIn = DateTime.Now.AddSeconds(2);
            }
            else if (_startSongIn <= DateTime.Now && !WelcomeScreen.IsPlayingSong)
            {
                WelcomeScreen.SpSong.PlayLooping();
                WelcomeScreen.IsPlayingSong = true;
            }
            g.FillRectangle(Config.Game.EscapeMenu.Brush, 0, 0, pnl.Width, pnl.Height);
            g.DrawString(Config.Game.GameOverMenu.GameOverMessage, new Font(Config.FontFamily, (float)pnl.Height / 10), new SolidBrush(Config.Colors.Accent), new RectangleF(0, 0, pnl.Width, pnl.Height), Config.StringFormat);
            g.DrawString(string.Format(Config.Game.GameOverMenu.ScoreMessage, GameScreen.Score, GameScreen.BaseDifficulty - GameScreen.Difficulty, GameScreen.HighScore), Config.Font, new SolidBrush(Config.Colors.Primary), new RectangleF(0, (float)pnl.Height / 6, pnl.Width, pnl.Height - (float)pnl.Height / 6), Config.StringFormat);

            if (_currentIteration <= GoToHomeScreenSpeed)
                g.DrawString(Config.Game.GameOverMenu.GoHomeMessage + "\r\n" +
                             (GameScreen.HasSavedHighScore ? Config.Game.GameOverMenu.ScoreSavedMessage : Config.Game.GameOverMenu.SaveScoreMessage), Config.Font, new SolidBrush(Config.Colors.Primary), new RectangleF(0, (float)pnl.Height / 4, pnl.Width, pnl.Height - (float)pnl.Height / 8), Config.StringFormat);

            _currentIteration = _currentIteration == GoToHomeScreenSpeed * 2 ? 0 : ++_currentIteration;
        }
    }
}