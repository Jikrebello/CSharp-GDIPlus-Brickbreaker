using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;

namespace Pong
{
    class BallAndPaddleSlow : PowerUp
    {
        readonly SoundPlayer powerUp = new SoundPlayer(@"D:\CSharp Projects\BrickBreaker\CSharp-GDIPlus-Brickbreaker\Media\Powerup.wav");

        public BallAndPaddleSlow() : base()
        {
            brush = new SolidBrush(Color.DarkGoldenrod);
        }

        public override void Collide(Paddle _player, List<Ball> _balls)
        {
            if (_player.Body.IntersectsWith(body))
            {
                // --- DOES THE ABILITY BEHAVIOUR ---
                foreach (Ball ball in _balls) // do it for all the balls
                {
                    Globals.IS_BALL_SLOW = true;
                    ball.SlowTime = DateTime.Now;
                    ball.BallStates = BallStates.Slow;
                    ball.Speed = Globals.BALL_SLOW;
                }

                Globals.IS_PADDLE_SLOW = true; // do it for the paddle
                _player.SlowTime = DateTime.Now;
                _player.PaddleStates = PaddleStates.Slow;
                _player.Speed = Globals.PADDLE_SLOW;

                // --- DOES THE ABILITY BEHAVIOUR ---

                powerUp.Play();
                MoveOffScreen();
            }
        }
    }
}
