using System;
using System.Drawing;
using System.Media;

namespace Pong
{
    class PaddleThinner : PowerUp
    {
        readonly SoundPlayer powerUp = new SoundPlayer(@"E:\C# Projects\General C#\Pong\Media\Shrinking.wav");
        public PaddleThinner() : base()
        {
            brush = new SolidBrush(Color.DarkViolet);
        }

        public override void Collide(Paddle _player)
        {
            if (_player.Body.IntersectsWith(body))
            {
                // --- DOES THE ABILITY BEHAVIOUR ---
                _player.ThinTime = DateTime.Now;
                _player.PaddleStates = PaddleStates.Thin;
                Globals.PADDLE_SHRINKING = true;
                // --- DOES THE ABILITY BEHAVIOUR ---

                powerUp.Play();

                MoveOffScreen();
            }
        }

    }
}
