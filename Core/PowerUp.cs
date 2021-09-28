using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace Pong
{
    abstract class PowerUp
    {
        // --- PUBLIC FIELDS ---
        public bool IsActive = false;

        // --- PRIVATE FIELDS ---
        const float speed = 2;
        const int DIAMETER = 10;
        protected SolidBrush brush;
        protected double X = -120;
        protected double Y = - 120;
        Vector2 Vector = new Vector2();
        protected Rectangle body;

        // --- CONSTRUCTOR ---
        public PowerUp()
        {
            body = new Rectangle((int)X, (int)Y, DIAMETER, DIAMETER);
        }

        /// <summary>
        /// Used in the Wider and Thinner paddle powerUp.
        /// </summary>
        /// <param name="_player"></param>
        public virtual void Collide(Paddle _player) { }
        /// <summary>
        /// Used by pretty much every other powerUp that influences the ball
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_balls"></param>
        public virtual void Collide(Paddle _player, List<Ball> _balls) { }
        /// <summary>
        /// Used for the multi-ball powerUp.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_area"></param>
        /// <param name="_balls"></param>
        public virtual void Collide(Paddle _player, Rectangle _area, List<Ball> _balls) { }

        public void Move()
        {
            if (IsActive == true)
            {
                Y += Vector.Y;
                body.Y = (int)Y;
            }
        }

        public void PositionPowerUp(Block _block)
        {
            X = _block.X + (_block.Width / 2) - DIAMETER;
            body.X = (int)X;

            Y = _block.Y + (_block.Width / 2) - DIAMETER;
            body.Y = (int)Y;

            Vector2 newVector = new Vector2(0, speed);
            Vector = newVector;
        }

        protected void MoveOffScreen()
        {
            X = -250;
            Y = -250;
            body.X = -250;
            body.Y = -250;
            IsActive = false;
        }

        public void Draw(Graphics _gfx)
        {
            _gfx.FillRectangle(brush, body);
        }
    }
}
