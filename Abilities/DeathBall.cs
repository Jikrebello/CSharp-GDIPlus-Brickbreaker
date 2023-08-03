using System.Drawing;
using System.Media;
using System.Collections.Generic;
using System;

namespace Pong.Abilities
{
    class DeathBall : PowerUp
    {
        readonly SoundPlayer powerUp = new SoundPlayer(@"D:\CSharp Projects\BrickBreaker\CSharp-GDIPlus-Brickbreaker\Media\Powerup.wav");

        public DeathBall() : base()
        {
            brush = new SolidBrush(Color.Black);
        }


        public override void Collide(Paddle _player, List<Ball> _balls)
        {
            if (_player.Body.IntersectsWith(body))
            {
                // --- DOES THE ABILITY BEHAVIOUR ---
                foreach (Ball ball in _balls)
                {
                    ball.DeathTime = DateTime.Now;
                    ball.BallStates = BallStates.Death;
                    ball.isDeath = true;
                    //Globals.IS_BALL_DEATH = true;
                }
                // --- DOES THE ABILITY BEHAVIOUR ---

                powerUp.Play();

                MoveOffScreen();
            }
        }
    }
}
