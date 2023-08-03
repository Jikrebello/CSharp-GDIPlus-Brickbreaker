using System;
using System.Drawing;
using System.Media;

namespace Pong.Abilities
{
    class PaddleShoot : PowerUp
    {
        readonly SoundPlayer powerUp = new SoundPlayer(@"D:\CSharp Projects\BrickBreaker\CSharp-GDIPlus-Brickbreaker\Media\Powerup.wav");

        public PaddleShoot() : base()
        {
            brush = new SolidBrush(Color.Orange);
        }

        public override void Collide(Paddle _player)
        {
            if (_player.Body.IntersectsWith(body))
            {
                // --- DOES THE ABILITY BEHAVIOUR ---
                Globals.CAN_PADDLE_SHOOT = true;
                // --- DOES THE ABILITY BEHAVIOUR ---

                powerUp.Play();

                MoveOffScreen();
            }
        }
    }
}
