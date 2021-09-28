using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Numerics;

namespace Pong
{
    class Ball
    {
        // --- PUBLIC FIELDS ---
        public int DIAMETER = 10;
        public double X;
        public double Y;
        public float Speed = 3.5f;
        public bool isActive;
        public bool isDeath;
        public Vector2 Vector = new Vector2();
        public Rectangle Body;
        public DateTime SmallTime;
        public DateTime BigTime;
        public DateTime FastTime;
        public DateTime SlowTime;
        public DateTime DeathTime;
        public BallStates BallStates;
        public SolidBrush Brush;
        public Color OriginalColor;

        // --- PRIVATE FIELDS ---
        const double maxAngle = 50;
        readonly SoundPlayer HitPlayer = new SoundPlayer(@"E:\C# Projects\General C#\Pong\Media\Hit.wav");
        readonly Random rand = new Random();
        Rectangle area;
        double ballIntersectX;
        double ballIntersectY;
        double ballAngle;


        // --- CONSTRUCTOR ---
        public Ball(Rectangle _area, Color _color, int _velocityX, int _velocityY)
        {
            area = _area;

            X = area.Width / 2 - DIAMETER;
            Y = area.Height / 2 - DIAMETER + 100;
            Body.X = (int)X;
            Body.Y = (int)Y;

            Vector.X = _velocityX;
            Vector.Y = _velocityY;

            Body = new Rectangle((int)X, (int)Y, DIAMETER, DIAMETER);

            OriginalColor = _color;
            Brush = new SolidBrush(OriginalColor);

            BallStates = BallStates.Neutral;
        }

        // --- MOVEMENT METHOD ---
        public void Update(Paddle _player, List<Ball> _balls)
        {
            if (isActive)
            {
                X += Vector.X;
                Y += Vector.Y;
                Body.X = (int)X;
                Body.Y = (int)Y;
                UpdateAbilities();
            }

            PositionBall(_player, _balls);
        }

        // --- START-UP METHODS ---
        public void PositionBall(Paddle _player, List<Ball> _balls)
        {
            if (X <= area.X) // left bound.
                Vector.X *= -1;

            else if (X >= area.Width - DIAMETER) // right bound.
                Vector.X *= -1;

            else if (Y <= area.Y) // top bound.
                Vector.Y *= -1;

            else if (Y >= area.Height - DIAMETER) // Bottom bound
            {
                if (_balls.Count > 1)
                {
                    _balls.Remove(this);
                }
                else
                {
                    Globals.IS_BALL_START = true; // this should only be reached when we're at the "last" ball.
                }
            }

            if (Globals.IS_BALL_START == true) // if this is true, then we launch the ball out just like we did in the beginning.
            {
                StartUpBall(_player, _balls);
                Globals.IS_BALL_START = false;
            }
        }

        void StartUpBall(Paddle _player, List<Ball> _balls)
        {
            _player.Lives--;
            _balls.Clear();
            Ball ball = new Ball(area, Color.Red, 1, 1);
            _balls.Add(ball);


            ball.X = area.Width / 2 - ball.DIAMETER;
            ball.Body.X = (int)X;

            ball.Y = area.Height / 2 - ball.DIAMETER + 100;
            ball.Body.Y = (int)Y;

            Vector2 _newVector = new Vector2(rand.Next(-2, 3), rand.Next(2, 4));
            ball.Vector = Vector2.Normalize(_newVector) * Speed;
            ball.isActive = true;
        }

        // --- COLLISION METHODS ---
        public void Bounce(Paddle _player)
        {
            double paddleCentreX = (_player.Width + DIAMETER) / 2;
            double paddleLeftCorner = _player.X + DIAMETER;

            // Ensure that the ball is colliding at the bottom of the ball and not the side.
            double ballCentreX = X + (DIAMETER / 2);

            // Get the length of the incoming vector speed that the ball is moving.
            Speed = (float)Math.Sqrt((Vector.X * Vector.X) + (Vector.Y * Vector.Y));

            // Determines what the balls X velocity is going to be based on where the ball lands on the rectangle that's width has been normalised between -1, 0 and 1.
            ballIntersectX = (ballCentreX - (paddleCentreX + paddleLeftCorner)) / paddleCentreX;

            // Determine the balls angle that its going to be reflected back at from the paddles X intersect.
            ballAngle = ballIntersectX * (maxAngle);

            // Determines what the balls Y velocity is going to be based on the balls worked out X velocity.
            ballIntersectY = Math.Tan(ballAngle) * ballIntersectX;

            // Lifts the ball above the paddle before applying velocities to ensure that its not in the middle.
            double respositionedY = (Y + DIAMETER) - _player.Y;
            Y -= Math.Abs(respositionedY);

            // Apply the new vector to the ball.
            Vector2 newVector = new Vector2((float)(Speed * ballIntersectX), (float)(-Speed * ballIntersectY));

            // Normalise the vector and multiply by the speed value.
            Vector = Vector2.Normalize(newVector) * Speed;
            HitPlayer.Play();
        }
        public void Bounce(Paddle _player, Block _block)
        {
            if (Body.IntersectsWith(_block.Body))
            {
                _block.Hit(_player, this);
                HitPlayer.Play();
            }
        }

        // --- ABILITY METHODS ---
        public void UpdateAbilities()
        {
            SmallCheck();
            BigCheck();
            FastCheck();
            SlowCheck();
            DeathBallCheck();
        }

        public void SmallCheck()
        {
            if (BallStates == BallStates.Small)
            {
                if (Globals.BALL_SHRINKING)
                {
                    int shrinkFactor = 1;
                    DIAMETER -= shrinkFactor;
                    Body.Width = DIAMETER;
                    Body.Height = DIAMETER;

                    if (Body.Width <= Globals.SMALL_BALL_DIAMETER)
                    {
                        Globals.BALL_SHRINKING = false;
                        Globals.IS_BALL_SMALL = true;
                    }
                }

                if (Globals.IS_BALL_SMALL)
                {
                    if (SmallTime.AddSeconds(5) < DateTime.Now)
                    {
                        Globals.IS_BALL_SMALL = false;
                        Globals.BALL_GROWING = true;
                    }
                }

                if (Globals.BALL_GROWING)
                {
                    int growFactor = 1;
                    DIAMETER += growFactor;
                    Body.Width = DIAMETER;
                    Body.Height = DIAMETER;

                    if (Body.Width >= Globals.BALL_NORMAL_DIAMETER)
                    {
                        Globals.BALL_GROWING = false;
                        BallStates = BallStates.Neutral;
                    }
                }
            }

        }

        void BigCheck()
        {
            if (BallStates == BallStates.Big)
            {
                if (Globals.BALL_GROWING)
                {
                    int growFactor = 1;
                    DIAMETER += growFactor;
                    Body.Width = DIAMETER;
                    Body.Height = DIAMETER;

                    if (Body.Width >= Globals.BIG_BALL_DIAMETER)
                    {
                        Globals.BALL_GROWING = false;
                        Globals.IS_BALL_BIG = true;
                    }
                }

                if (Globals.IS_BALL_BIG)
                {
                    if (BigTime.AddSeconds(5) < DateTime.Now)
                    {
                        Globals.IS_BALL_BIG = false;
                        Globals.BALL_SHRINKING = true;
                    }
                }

                if (Globals.BALL_SHRINKING)
                {
                    int shrinkFactor = 1;
                    DIAMETER -= shrinkFactor;
                    Body.Width = DIAMETER;
                    Body.Height = DIAMETER;

                    if (Body.Width <= Globals.BALL_NORMAL_DIAMETER)
                    {
                        Globals.BALL_SHRINKING = false;
                        BallStates = BallStates.Neutral;
                    }
                }

            }

        }

        void FastCheck()
        {
            if (BallStates == BallStates.Fast)
            {
                if (Globals.IS_BALL_FAST)
                {
                    if (FastTime.AddSeconds(5) < DateTime.Now)
                    {
                        Speed = Globals.BALL_NORMAL_SPEED;
                        Globals.IS_BALL_FAST = false;
                        BallStates = BallStates.Neutral;
                    }
                }
            }
        }

        void SlowCheck()
        {
            if (BallStates == BallStates.Slow)
            {
                if (Globals.IS_BALL_SLOW)
                {
                    if (SlowTime.AddSeconds(5) < DateTime.Now)
                    {
                        Speed = Globals.BALL_NORMAL_SPEED;
                        Globals.IS_BALL_SLOW = false;
                        BallStates = BallStates.Neutral;
                    }
                }
            }
        }

        void DeathBallCheck()
        {
            if (BallStates == BallStates.Death)
            {
                if (isDeath)
                {
                    Color _color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256)); // --- DISCO MODE ---
                    Brush.Color = _color;

                    if (DeathTime.AddSeconds(5) < DateTime.Now)
                    {
                        Brush.Color = OriginalColor;
                        isDeath = false;
                        BallStates = BallStates.Neutral;
                    }
                }
            }
        }

        // --- GRAPHICS METHOD ---
        public void Draw(Graphics _gfx)
        {
            try
            {
                _gfx.FillRectangle(Brush, Body);
            }
            catch (OverflowException OverEX)
            {
                string oops = OverEX.Message;
            }
        }
    }
}
