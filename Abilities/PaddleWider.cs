using System;
using System.Drawing;
using System.Media;

namespace Pong
{
    class PaddleWider : PowerUp
    {
        readonly SoundPlayer powerUp = new SoundPlayer(@"D:\CSharp Projects\BrickBreaker\CSharp-GDIPlus-Brickbreaker\Media\Growing.wav");
        public PaddleWider() : base()
        {
            brush = new SolidBrush(Color.DarkViolet);
        }

        public override void Collide(Paddle _player)
        {
            if (_player.Body.IntersectsWith(body))
            {
                _player.WideTime = DateTime.Now;
                _player.PaddleStates = PaddleStates.Wide;
                Globals.PADDLE_GROWING = true;

                powerUp.Play();

                MoveOffScreen();
            }
        }
    }
}
