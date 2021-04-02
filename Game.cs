using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Reflection;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public partial class Game : Form
    {
        private List<Keys> _currentPressed;
        private Action<Panel, Graphics> _callback;

        public Game()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.ToggleScreen(true);

            this.ForeColor = Config.Colors.Primary;
            this.BackColor = Config.Colors.Back;
            this._callback = WelcomeScreen.Draw;
            this._currentPressed = new List<Keys>();
            FrameHandler.Start();

            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, pnl, new object[] {true});
        }

        private void DrawPanel(object sender, PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            this.HandleKeyEvents();
            this._callback(pnl, e.Graphics);
        }

        private void HandleWindowContentLocation(object sender, EventArgs e)
        {
            // TODO: Fix the panel height issue
            pnl.Width = pnl.Height = this.Width < this.Height ? this.Width : this.Height;

            Entities.Size = pnl.Width / 12;
            
            pnl.Left = this.Width / 2 - pnl.Width / 2;
            pnl.Top = this.Height / 2 - pnl.Height / 2;

            pnl.Refresh();
        }

        private void HandleFormKeydown(object sender, KeyEventArgs e)
        {
            if (!this._currentPressed.Contains(e.KeyCode)) 
                this._currentPressed.Add(e.KeyCode);
        }

        private void HandleFormKeyup(object sender, KeyEventArgs e)
        {
            if (this._currentPressed.Contains(e.KeyCode))
                this._currentPressed.Remove(e.KeyCode);
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
                    this.ToggleScreen(false);
                    break;
                case Keys.F11:
                    this.ToggleScreen(this.FormBorderStyle != FormBorderStyle.None);
                    break;
                case Keys.Space:
                case Keys.Enter:
                    if (!WelcomeScreen.ScreenPassed)
                    {
                        WelcomeScreen.ScreenPassed = true;
                        this._callback = GameScreen.Draw;
                    }
                    else GameScreen.Shoot();
                    break;
                case Keys.Left:
                case Keys.A:
                    if (GameScreen.CurrentXLocation <= 10) GameScreen.CurrentXLocation = 10;
                    else GameScreen.CurrentXLocation -= 10;
                    break;
                case Keys.Right:
                case Keys.D:
                    if (GameScreen.CurrentXLocation >= pnl.Width - 10 - Entities.Size) GameScreen.CurrentXLocation = pnl.Width - 10 - Entities.Size;
                    else GameScreen.CurrentXLocation += 10;
                    break;
            }
        }
    }
}
