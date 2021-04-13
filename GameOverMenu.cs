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
            } else if (_startSongIn <= DateTime.Now && !WelcomeScreen.IsPlayingSong)
            {
                WelcomeScreen.SpSong.PlayLooping();
                WelcomeScreen.IsPlayingSong = true;
            }
            g.FillRectangle(Config.Game.EscapeMenu.Brush, 0, 0, pnl.Width, pnl.Height);
            g.DrawString(Config.Game.GameOverMenu.GameOverMessage, new Font(Config.FontFamily, (float)pnl.Height / 10), new SolidBrush(Config.Colors.Accent), new RectangleF(0, 0, pnl.Width, pnl.Height), Config.StringFormat);
            g.DrawString(string.Format(Config.Game.GameOverMenu.ScoreMessage, GameScreen.Score, GameScreen.BaseDifficulty - GameScreen.Difficulty, GameScreen.HighScore), Config.Font, new SolidBrush(Config.Colors.Primary), new RectangleF(0, (float)pnl.Height / 6, pnl.Width, pnl.Height - (float)pnl.Height / 6), Config.StringFormat);

            if (_currentIteration <= GoToHomeScreenSpeed)
                g.DrawString(Config.Game.GameOverMenu.GoHomeMessage, Config.Font, new SolidBrush(Config.Colors.Primary), new RectangleF(0, (float)pnl.Height / 4, pnl.Width, pnl.Height - (float)pnl.Height / 4), Config.StringFormat);

            _currentIteration = _currentIteration == GoToHomeScreenSpeed * 2 ? 0 : ++_currentIteration;
        }
    }
}