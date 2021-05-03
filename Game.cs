using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Reflection;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SpaceInvaders
{
    public partial class Game : Form
    {
        private List<Keys> _currentPressed;
        public Action<Panel, Graphics> Callback;
        public Action<Panel, Graphics> Overlay;
        private DateTime _lastShotFired;
        private DateTime _overlayCooldown;
        private readonly int _id;
        private readonly string _user;
        private readonly MySqlHandler _mySqlHandler;
        private bool _hasOpenedConfigScreen = false;

        public Game(int id, string user, MySqlHandler handler)
        {
            InitializeComponent();

            this._id = id;
            this._user = user;
            this._mySqlHandler = handler;
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.ToggleScreen(true);
            
            this.Callback = WelcomeScreen.Draw;
            this.Overlay = null;
            this._overlayCooldown = DateTime.MinValue;
            this._currentPressed = new List<Keys>();
            this._lastShotFired = DateTime.MinValue;
            FrameHandler.Start();
            this.Text = $@"Space Invaders - {this._user}";
            Config.CurrentUserName = this._user;
            Config.Id = this._id;

            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, pnl, new object[] { true });
        }

        private void DrawPanel(object sender, PaintEventArgs e)
        {
            pnl.BackColor = Config.Colors.PrimaryDarkest;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            this.ForeColor = Config.Colors.Primary;
            this.BackColor = Config.Colors.Back;
            this.HandleKeyEvents();
            this.Callback(pnl, e.Graphics);
            if (Overlay == null && GameOverMenu.Enabled)
            {
                GameScreen.IsPaused = true;
                Overlay = GameOverMenu.Draw;
            }
            Overlay?.Invoke(pnl, e.Graphics);
        }

        private void HandleWindowContentLocation(object sender, EventArgs e)
        {
            if (!Config.ResponsivenessEnabled) return;
            // TODO: Fix the panel height issue
            pnl.Width = pnl.Height = this.Width < this.Height ? this.Width : this.Height;

            Entities.Size = pnl.Width / 20;

            pnl.Left = this.Width / 2 - pnl.Width / 2;
            pnl.Top = this.Height / 2 - pnl.Height / 2;

            pnl.Refresh();
        }

        private void HandleFormKeydown(object sender, KeyEventArgs e)
        {
            // Handle some keys in here, to prevent repetitions
            switch (e.KeyCode)
            {
                case Keys.C:
                    if (!this._hasOpenedConfigScreen)
                    {
                        ConfigScreen cfg = new ConfigScreen(this._mySqlHandler);
                        cfg.Closed += (_, __) =>
                        {
                            this._hasOpenedConfigScreen = false;
                            if (Config.ShouldDie) this.Close();
                        };
                        this._hasOpenedConfigScreen = true;
                        cfg.Show();
                    }
                    break;
                case Keys.OemQuestion:
                    if (!WelcomeScreen.ScreenPassed)
                        this.Overlay = ControlsOverlay.Draw;
                    break;

                case Keys.F11:
                case Keys.Up:
                case Keys.Down:
                case Keys.W:
                case Keys.S:
                    this.HandleKeyEvent(e.KeyCode);
                    break;

                case Keys.Enter:
                    if (!GameScreen.IsPaused || GameOverMenu.Enabled) goto case Keys.Space;

                    EscapeMenu.Game = this;
                    EscapeMenu.Items[EscapeMenu.HighlightedIndex].Item2();
                    break;

                case Keys.Space:
                    if (!WelcomeScreen.ScreenPassed) goto default;
                    else if (!GameScreen.IsPaused)
                    {
                        DateTime dt = DateTime.Now;
                        if (!(this._currentPressed.Contains(Keys.Enter) || this._currentPressed.Contains(Keys.Space)) && (dt - this._lastShotFired).Milliseconds >= 50)
                        {
                            this._lastShotFired = dt;
                            GameScreen.Shoot();
                            this._currentPressed.Add(e.KeyCode);
                        }
                    }
                    else if (GameOverMenu.Enabled)
                    {
                        GameOverMenu.Enabled = false;
                        GameScreen.Reset(pnl, this);
                        WelcomeScreen.ScreenPassed = false;
                        this.Callback = WelcomeScreen.Draw;
                        this.Overlay = null;

                        GameScreen.Difficulty = GameScreen.BaseDifficulty;
                        if (GameScreen.Score > GameScreen.HighScore) GameScreen.HighScore = GameScreen.Score;
                        GameScreen.Score = 0;
                    }
                    break;

                default:
                    if (!this._currentPressed.Contains(e.KeyCode))
                        this._currentPressed.Add(e.KeyCode);
                    break;
            }
        }

        private void HandleFormKeyup(object sender, KeyEventArgs e)
        {
            if (this._currentPressed.Contains(e.KeyCode))
                this._currentPressed.Remove(e.KeyCode);
            else if (e.KeyCode == Keys.OemQuestion) this.Overlay = null;
        }

        private void ToggleScreen(bool toFull)
        {
            this.FormBorderStyle = toFull ? FormBorderStyle.None : FormBorderStyle.Sizable;
            this.WindowState = toFull ? FormWindowState.Maximized : FormWindowState.Normal;
        }

        private void FrameHandler_Tick(object sender, EventArgs e)
            => pnl.Refresh();

        private void HandleKeyEvents()
        {
            foreach (Keys key in this._currentPressed)
                this.HandleKeyEvent(key);
        }

        private void HandleKeyEvent(Keys key)
        {
            switch (key)
            {
                case Keys.Escape:
                {
                    if (!WelcomeScreen.ScreenPassed)
                    {
                        this.ToggleScreen(false);
                        break;
                    }

                    DateTime dt = DateTime.Now;

                    if ((dt - this._overlayCooldown).Milliseconds >= 100 && !GameOverMenu.Enabled)
                    {
                        // this.ToggleScreen(false);
                        GameScreen.IsPaused = !GameScreen.IsPaused;

                        // Can't use ternary here because of C# types... sad
                        if (GameScreen.IsPaused) this.Overlay = EscapeMenu.Draw;
                        else this.Overlay = null;

                        this._overlayCooldown = dt;
                    }
                }
                break;

                case Keys.F11:
                    this.ToggleScreen(this.FormBorderStyle != FormBorderStyle.None);
                    break;

                case Keys.Space:
                case Keys.Enter:
                    if (!WelcomeScreen.ScreenPassed)
                    {
                        WelcomeScreen.ScreenPassed = true;
                        this.Callback = GameScreen.Draw;
                        this.Overlay = null;
                    }
                    break;

                case Keys.Left:
                case Keys.A:
                    if (!GameScreen.IsPaused)
                    {
                        if (GameScreen.CurrentXLocation <= 10) GameScreen.CurrentXLocation = 10;
                        else GameScreen.CurrentXLocation -= 10;
                    }
                    break;

                case Keys.Right:
                case Keys.D:
                    if (!GameScreen.IsPaused)
                    {
                        if (GameScreen.CurrentXLocation >= pnl.Width - 10 - Entities.Size)
                            GameScreen.CurrentXLocation = pnl.Width - 10 - Entities.Size;
                        else GameScreen.CurrentXLocation += 10;
                    }
                    break;

                case Keys.S:
                    if (GameOverMenu.Enabled)
                    {
                        GameScreen.HasSavedHighScore = true;
                        MySqlCommand cmd = this._mySqlHandler.Prepare(
                            "INSERT INTO EX2_space_invaders_scores (owner, score) VALUES (@owner, @score);",
                            ("@owner", this._id), ("@score", GameScreen.Score));
                        this._mySqlHandler.Execute(cmd);
                    }
                    else goto case Keys.Down;
                    break;
                case Keys.Down:
                    if (EscapeMenu.HighlightedIndex == EscapeMenu.Items.Length - 1) break;
                    EscapeMenu.HighlightedIndex++;
                    break;

                case Keys.W:
                case Keys.Up:
                    if (EscapeMenu.HighlightedIndex == 0) break;
                    EscapeMenu.HighlightedIndex--;
                    break;
            }
        }
    }
}