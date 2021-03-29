using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public partial class Game : Form
    {
        public Game()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.ForeColor = Config.Colors.Primary;
            this.BackColor = Config.Colors.Back;
        }

        private void DrawPanel(object sender, PaintEventArgs e)
        {
            this.DrawWelcomeScreen(e.Graphics);
        }

        private void DrawWelcomeScreen(Graphics g)
        {
            new Entities.Entity(10, 10, Entities.EntityType.Octopus).Draw(g);
            new Entities.Entity(120, 10, Entities.EntityType.Octopus).Draw(g);
        }
    }
}