using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Reflection;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public partial class Game : Form
    {
        private Action<Graphics> _callback;

        private int _welcomeEntitiesLeft;
        private bool _welcomeEntitiesFirst;
        private int _welcomeEntitiesIteration;

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
            this._callback = DrawWelcomeScreen;
            this._welcomeEntitiesLeft = - Entities.Size;
            this._welcomeEntitiesFirst = true;
            this._welcomeEntitiesIteration = 0;
            FrameHandler.Start();

            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, pnl, new object[] {true});
        }

        private void DrawPanel(object sender, PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            this._callback(e.Graphics);
        }

        private void DrawWelcomeScreen(Graphics g)
        {
            this._welcomeEntitiesLeft += Entities.Size / 8;
            if (this._welcomeEntitiesLeft >= pnl.Width / 10 - Entities.Size) 
                this._welcomeEntitiesLeft = -Entities.Size;

            for (int i = 0; i < 11; i++)
            {
                int x = (pnl.Width / 10 - Entities.Size) * i + Entities.Size * i;
                new Entities.Entity(this._welcomeEntitiesLeft + x, 10, Entities.EntityType.Crab).Draw(g, this._welcomeEntitiesFirst);
                new Entities.Entity(- this._welcomeEntitiesLeft + x - Entities.Size, pnl.Height - Entities.Size, Entities.EntityType.Crab).Draw(g, this._welcomeEntitiesFirst);
            }

            this._welcomeEntitiesFirst = !this._welcomeEntitiesFirst;
            string message = "SPACE\r\nINVADERS";
            StringFormat sf = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };

            Font font = new Font(Config.FontFamily, (float)pnl.Width / 12);
            Color clr = Config.Colors.Accent;

            g.DrawString(message, font,
                new SolidBrush(Color.FromArgb(64, clr)),
                new RectangleF(0 + 5, 0 + 5, pnl.Width, pnl.Height), sf);
        
            g.DrawString(message, font,
                new SolidBrush(clr),
                new RectangleF(0, 0, pnl.Width, pnl.Height), sf);

            if (this._welcomeEntitiesIteration >= 8)
                g.DrawString("Press enter...", Config.Font,
                    new SolidBrush(Config.Colors.Primary),
                    new RectangleF(0, (float)pnl.Height / 3, pnl.Width, pnl.Height), sf);

            if (this._welcomeEntitiesIteration >= 16) this._welcomeEntitiesIteration = 0;
            else this._welcomeEntitiesIteration++;
            
            pnl.BackColor = Config.Colors.PrimaryDarkest;
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
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.ToggleScreen(false);
                    break;
                case Keys.F11:
                    this.ToggleScreen(this.FormBorderStyle != FormBorderStyle.None);
                    break;
            }
        }

        private void ToggleScreen(bool toFull)
        {
            this.FormBorderStyle = toFull ? FormBorderStyle.None : FormBorderStyle.Sizable;
            this.WindowState = toFull ? FormWindowState.Maximized : FormWindowState.Normal;
        }

        private void FrameHandler_Tick(object sender, EventArgs e)
        {
            pnl.Refresh();
        }
    }
}
