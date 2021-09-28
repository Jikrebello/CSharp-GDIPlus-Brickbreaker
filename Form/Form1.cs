using System.Drawing;
using System.Windows.Forms;

namespace Pong
{
    public partial class Form1 : Form
    {
        Engine Pong;
        Rectangle rect;
        public Form1()
        {
            InitializeComponent();

            rect = new Rectangle(20, 0, 525, 525);

            Pong = new Engine(rect);
            Pong.Start();
        }

        internal void Loop()
        {
            Pong.Update();
            label1.Text = "Player Score: " + Pong.Player.Score.ToString();
            label2.Text = "Player Lives: " + Pong.Player.Lives.ToString();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;

            Pong.Player.Draw(gfx);

            foreach (PowerUp powerUp in Pong.ListOfPowerUps)
            {
                powerUp.Draw(gfx);
            }

            foreach (Block block in Pong.Blocks)
            {
                block.Draw(gfx);
            }

            foreach (Ball ball in Pong.Balls)
            {
                ball.Draw(gfx);
            }

            Pong.Player.Bullet.Draw(gfx);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Pong.KeyPressed(e, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Pong.KeyPressed(e, false);
        }
    }
}
