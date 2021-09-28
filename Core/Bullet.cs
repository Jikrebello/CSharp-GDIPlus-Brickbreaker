using System.Drawing;

namespace Pong.Core
{
    class Bullet
    {
        // --- PUBLIC FIELDS ---
        public bool IsActive = false;
        public Rectangle Body;
        public double X;
        public double Y;

        // --- PRIVATE FIELDS ---
        const float Speed = 5;
        const int Width = 10;
        const int Height = 15;
        protected SolidBrush brush;

        // --- CONSTRUCTOR ---
        public Bullet()
        {
            Body = new Rectangle((int)X, (int)Y, Width, Height);
            brush = new SolidBrush(Color.Black);
            ResestPosition();

        }

        public void FireBullet(Paddle _player)
        {
            if (IsActive == false)
            {
                GetBulletStartPosition(_player);
                IsActive = true;
            }
        }

        void GetBulletStartPosition(Paddle _player)
        {
            X = _player.X + (_player.Width / 2);
            Body.X = _player.X + (_player.Width / 2);
            Y = _player.Y;
            Body.Y = _player.X + (_player.Width / 2);

        }

        public void Move()
        {
            if (IsActive)
            {
                if (Body.Y + Body.Height < 0) // gone out of bounds
                    ResestPosition();
                Y -= Speed; // just send it up towards the top of the screen
                Body.Y = (int)Y;
            }
        }

        public void ResestPosition()
        {
            IsActive = false;
            X = -100; Y = -200;// move off screen on both x and y
            Body.X = -100; Body.Y = -200;
        }


        public void Draw(Graphics _gfx)
        {
            _gfx.FillRectangle(brush, Body);
        }

    }
}
