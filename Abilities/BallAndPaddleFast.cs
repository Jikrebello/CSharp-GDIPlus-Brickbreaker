using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;

namespace Pong
{
    class BallAndPaddleFast : PowerUp
    {
        readonly SoundPlayer powerUp = new SoundPlayer(@"E:\C# Projects\General C#\Pong\Media\Powerup.wav");
        public BallAndPaddleFast() : base()
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
                    Globals.IS_BALL_FAST = true;
                    ball.FastTime = DateTime.Now;
                    ball.BallStates = BallStates.Fast;
                    ball.Speed = Globals.BALL_FAST;
                }

                Globals.IS_PADDLE_FAST = true; // do it for the paddle
                _player.FastTime = DateTime.Now;
                _player.PaddleStates = PaddleStates.Fast;
                _player.Speed = Globals.PADDLE_FAST;
                // --- DOES THE ABILITY BEHAVIOUR ---

                powerUp.Play();
                MoveOffScreen();
            }
        }

    }
}
