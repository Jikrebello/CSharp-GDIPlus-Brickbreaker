using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;

namespace Pong
{
    class BiggerBall : PowerUp
    {
        readonly SoundPlayer powerUp = new SoundPlayer(@"D:\CSharp Projects\BrickBreaker\CSharp-GDIPlus-Brickbreaker\Media\Growing.wav");

        public BiggerBall() : base()
        {
            brush = new SolidBrush(Color.Cyan);
        }

        public override void Collide(Paddle _player, List<Ball> _balls)
        {
            if (_player.Body.IntersectsWith(body))
            {
                // --- DOES THE ABILITY BEHAVIOUR ---
                foreach (Ball ball in _balls)
                {
                    ball.BigTime = DateTime.Now;
                    ball.BallStates = BallStates.Big;
                    Globals.BALL_GROWING = true;
                }
                // --- DOES THE ABILITY BEHAVIOUR ---

                powerUp.Play();

                MoveOffScreen();
            }
        }
    }
}
