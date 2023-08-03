using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;

namespace Pong.Abilities
{
    class MultiBall : PowerUp
    {
        readonly SoundPlayer powerUp = new SoundPlayer(@"D:\CSharp Projects\BrickBreaker\CSharp-GDIPlus-Brickbreaker\Media\Powerup.wav");

        public MultiBall() : base()
        {
            brush = new SolidBrush(Color.HotPink);
        }

        public override void Collide(Paddle _player, Rectangle _area, List<Ball> _balls)
        {
            if (_player.Body.IntersectsWith(body))
            {
                // --- DOES THE ABILITY BEHAVIOUR ---
                Ball ballLeft = new Ball(_area, Color.Red, -2, -1); // Make the ball X velocity move to the left.
                Ball ballRight = new Ball(_area, Color.Red, 2, -1); // Make the ball Y velocity move to the right.

                // Spawn ball on the left hand corner of the paddle.
                ballLeft.X = _player.X;
                ballLeft.Body.X = _player.Body.X;
                ballLeft.Y = _player.Y - Math.Abs(10);
                ballLeft.Body.Y = _player.Body.Y;
                ballLeft.isActive = true;

                // Spawn ball on the right hand corner of the paddle.
                ballRight.X = _player.X + _player.Width;
                ballRight.Body.X = _player.X + _player.Width;
                ballRight.Y = _player.Y - 5;
                ballRight.Body.Y = _player.Y;
                ballRight.isActive = true;


                _balls.Add(ballLeft);
                _balls.Add(ballRight);
                // --- DOES THE ABILITY BEHAVIOUR ---

                powerUp.Play();

                MoveOffScreen();
            }
        }
    }
}
