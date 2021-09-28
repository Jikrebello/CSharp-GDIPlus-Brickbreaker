using Pong.Core;
using System;
using System.Drawing;

namespace Pong
{
    class Paddle
    {
        // --- PROPERTIES ---
        public int X { get; private set; }
        public int Y { get; }
        public int Height { get { return height; } }

        // --- PUBLIC FIELDS ---
        public int Width;
        public bool Left;
        public bool Right;
        public int Speed = 5;
        public int Score;
        public int Lives;
        public Rectangle Body;
        public PaddleStates PaddleStates;
        public DateTime ThinTime;
        public DateTime WideTime;
        public DateTime FastTime;
        public DateTime SlowTime;
        public Bullet Bullet;
        public bool Fire;

        // --- PRIVATE FIELDS ---
        const int OFFSET_Y = 10;
        readonly SolidBrush brush;
        readonly int height;
        private Rectangle area;
        int bulletTotal = 1;
        readonly Random rand = new Random();
        readonly Color originalColor;


        // --- CONSTRUCTOR ---
        public Paddle(Rectangle _area, Color _color, Bullet _bullet)
        {
            area = _area;
            Width = Globals.PADDLE_NORMAL_WIDTH;
            height = 20;
            X = area.Width / 2 - Width;
            Y = area.Height - Height - OFFSET_Y;

            Body = new Rectangle(X, Y, Width, Height);
            brush = new SolidBrush(_color);
            originalColor = brush.Color;

            Left = false;
            Right = false;
            Lives = 3;
            PaddleStates = PaddleStates.Neutral;
            Bullet = _bullet;

        }


        // --- MOVEMENT METHODS --- 
        public void Update()
        {
            MoveLeft();
            MoveRight();
            UpdateAbilities();
        }
        void MoveLeft()
        {
            if (Left)
            {
                if (X <= area.X)        // Reached the left hand side boundary.
                    X = area.X;         // Keep it on the boundary and no further.
                else
                {
                    X -= Speed;         // Do the translation left (-).
                    Body.X -= Speed;    // Make the drawn rectangle follow;
                }
            }
        }
        void MoveRight()
        {
            if (Right)
            {
                if (X >= area.Width - Width)    // Reached the right hand side boundary.
                    X = area.Width - Width;     // Keep it on the boundary and no further.
                else
                {
                    X += Speed;                 // Do the translation right (+).
                    Body.X += Speed;            // Make the drawn rectangle follow;
                }
            }
        }


        // --- ABILITY METHODS ---
        public void UpdateAbilities()
        {
            ThinCheck();
            WideCheck();
            FastCheck();
            SlowCheck();
            ShootCheck();
        }
        void ThinCheck()
        {
            if (PaddleStates == PaddleStates.Thin)
            {
                if (Globals.PADDLE_SHRINKING)
                {
                    int shrinkFactor = 1;
                    Width -= shrinkFactor;
                    Body.Width = Width;

                    if (Body.Width <= Globals.PADDLE_THIN_WIDTH)
                    {
                        Globals.PADDLE_SHRINKING = false;
                        Globals.IS_PADDLE_THIN = true;
                    }
                }

                if (Globals.IS_PADDLE_THIN)
                {
                    if (ThinTime.AddSeconds(5) < DateTime.Now)
                    {
                        Globals.IS_PADDLE_THIN = false;
                        Globals.PADDLE_GROWING = true;
                    }
                }

                if (Globals.PADDLE_GROWING)
                {
                    int growFactor = 1;
                    Width += growFactor;
                    Body.Width = Width;

                    if (Body.Width >= Globals.PADDLE_NORMAL_WIDTH)
                    {
                        Globals.PADDLE_GROWING = false;
                        PaddleStates = PaddleStates.Neutral;
                    }
                }
            }
        }
        void WideCheck()
        {
            if (PaddleStates == PaddleStates.Wide)
            {
                if (Globals.PADDLE_GROWING)
                {
                    int growFactor = 1;
                    Width += growFactor;
                    Body.Width = Width;

                    if (Body.Width >= Globals.PADDLE_WIDE_WIDTH)
                    {
                        Globals.PADDLE_GROWING = false;
                        Globals.IS_PADDLE_WIDE = true;
                    }
                }

                if (Globals.IS_PADDLE_WIDE)
                {
                    if (WideTime.AddSeconds(5) < DateTime.Now)
                    {
                        Globals.IS_PADDLE_WIDE = false;
                        Globals.PADDLE_SHRINKING = true;
                    }
                }

                if (Globals.PADDLE_SHRINKING)
                {
                    int shrinkFactor = 1;
                    Width -= shrinkFactor;
                    Body.Width = Width;

                    if (Body.Width <= Globals.PADDLE_NORMAL_WIDTH)
                    {
                        Globals.PADDLE_SHRINKING = false;
                        PaddleStates = PaddleStates.Neutral;
                    }
                }
            }
        }
        void FastCheck()
        {
            if (PaddleStates == PaddleStates.Fast)
            {
                if (Globals.IS_PADDLE_FAST)
                {
                    if (FastTime.AddSeconds(5) < DateTime.Now)
                    {
                        Speed = Globals.PADDLE_NORMAL_SPEED;
                        Globals.IS_PADDLE_FAST = false;
                        PaddleStates = PaddleStates.Neutral;
                    }
                }
            }
        }
        void SlowCheck()
        {
            if (PaddleStates == PaddleStates.Slow)
            {
                if (Globals.IS_PADDLE_SLOW)
                {
                    if (SlowTime.AddSeconds(5) < DateTime.Now)
                    {
                        Speed = Globals.PADDLE_NORMAL_SPEED;
                        Globals.IS_PADDLE_SLOW = false;
                        PaddleStates = PaddleStates.Neutral;
                    }
                }
            }
        }
        void ShootCheck()
        {
            if (Globals.CAN_PADDLE_SHOOT)
            {
                // --- 
                Color _color = Color.FromArgb(rand.Next(256),rand.Next(256),rand.Next(256)); // --- DISCO MODE ---
                brush.Color = _color;

                if (bulletTotal > 0)
                {
                    if (Fire)
                    {
                        Bullet.FireBullet(this);
                        bulletTotal--;
                    }
                }
                else
                {
                    Globals.CAN_PADDLE_SHOOT = false;
                    brush.Color = originalColor;
                    bulletTotal = 1;
                }
            }
        }

        // --- DISPLAY METHODS ---
        public void Draw(Graphics _gfx)
        {
            _gfx.FillRectangle(brush, Body);
        }
    }
}
