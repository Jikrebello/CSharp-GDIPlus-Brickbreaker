using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;

namespace Pong
{
    class SmallerBall : PowerUp
    {
        readonly SoundPlayer powerUp = new SoundPlayer(@"E:\C# Projects\General C#\Pong\Media\Shrinking.wav");

        public SmallerBall() : base()
        {
            brush = new SolidBrush(Color.Yellow);
        }

        public override void Collide(Paddle _player, List<Ball> _balls)
        {
            if (_player.Body.IntersectsWith(body))
            {
                // --- DOES THE ABILITY BEHAVIOUR ---
                foreach (Ball ball in _balls)
                {
                    ball.SmallTime = DateTime.Now;
                    ball.BallStates = BallStates.Small;
                    Globals.BALL_SHRINKING = true;
                }
                // --- DOES THE ABILITY BEHAVIOUR ---

                powerUp.Play();

                MoveOffScreen();
            }
        }
    }
}
