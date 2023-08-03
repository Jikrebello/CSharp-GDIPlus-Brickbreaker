using Pong.Abilities;
using Pong.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;

namespace Pong
{
    class Engine
    {
        // --- PROPERTIES ---
        internal Block Block { get => block; set => block = value; }
        internal List<Block> Blocks { get => blocks; set => blocks = value; }
        internal List<PowerUp> ListOfPowerUps { get => listOfPowerUps; set => listOfPowerUps = value; }

        // --- PUBLIC FIELDS ---
        public Paddle Player;
        public List<Ball> Balls = new List<Ball>();

        // --- PRIVATE FIELDS ---
        readonly int numberOfBlocks = 32;
        readonly Random rand = new Random();
        readonly MediaPlayer musicPlayer = new MediaPlayer();
        private Rectangle area;
        private Rectangle blockArea;
        private Ball ball;
        private Block block;
        private List<Block> blocks = new List<Block>();
        private PowerUp powerUp;
        private List<PowerUp> listOfPowerUps = new List<PowerUp>();
        private Bullet bullet;

        // --- CONSTRUCTOR ---
        public Engine(Rectangle _area)
        {
            area = _area;
        }

        public void Start()
        {
            musicPlayer.Open(new Uri(@"D:\CSharp Projects\BrickBreaker\CSharp-GDIPlus-Brickbreaker\Media\Chiptronical.wav"));
            musicPlayer.Play();
            musicPlayer.Volume = 0.1f;

            // Create a bullet for the paddle
            bullet = new Bullet();

            // Create a player.
            Player = new Paddle(area, System.Drawing.Color.Blue, bullet);

            // Create a ball and add the ball to the list of balls (will be overridden anyway with the balls PositionBall() & StartUpBall() methods but is needed to ensure the list is the correct size.
            ball = new Ball(area, System.Drawing.Color.Red, 1, 1);
            Balls.Add(ball);

            int numberOfPowerUps = 10; // Make it +1 more than it needs to be for the outer random outer exclusive

            // Generate a bunch of blocks and hide powerUps behind them (default 24) and put them into a list.
            for (int i = 0; i < numberOfBlocks; i++)
            {

                if (rand.Next(0, numberOfPowerUps) == 1)                    // --- POWERUP: DEATHBALL ---
                {
                    powerUp = new DeathBall();
                    Block = new Block(rand.Next(1, 4));
                    Blocks.Add(Block);
                    ListOfPowerUps.Add(powerUp);
                }

                else if (rand.Next(0, numberOfPowerUps) == 2)             // --- POWERUP: MULTIBALL ---
                {
                    powerUp = new MultiBall();
                    Block = new Block(rand.Next(1, 4));
                    Blocks.Add(Block);
                    ListOfPowerUps.Add(powerUp);
                }

                else if (rand.Next(0, numberOfPowerUps) == 3)             // --- POWERUP: PADDLETHINNER ---
                {
                    powerUp = new PaddleThinner();
                    Block = new Block(rand.Next(1, 4));
                    Blocks.Add(Block);
                    ListOfPowerUps.Add(powerUp);
                }

                else if (rand.Next(0, numberOfPowerUps) == 4)             // --- POWERUP: PADDLEWIDER ---
                {
                    powerUp = new PaddleWider();
                    Block = new Block(rand.Next(1, 4));
                    Blocks.Add(Block);
                    ListOfPowerUps.Add(powerUp);
                }

                else if (rand.Next(0, numberOfPowerUps) == 5)             // --- POWERUP: BIGGERBALL ---
                {
                    powerUp = new BiggerBall();
                    Block = new Block(rand.Next(1, 4));
                    Blocks.Add(Block);
                    ListOfPowerUps.Add(powerUp);
                }

                else if (rand.Next(0, numberOfPowerUps) == 6)             // --- POWERUP: SMALLERBALL ---
                {
                    powerUp = new SmallerBall();
                    Block = new Block(rand.Next(1, 4));
                    Blocks.Add(Block);
                    ListOfPowerUps.Add(powerUp);
                }

                else if (rand.Next(0, numberOfPowerUps) == 7)             // --- POWERUP: PADDLESHOOT ---
                {
                    powerUp = new PaddleShoot();
                    Block = new Block(rand.Next(1, 4));
                    Blocks.Add(Block);
                    ListOfPowerUps.Add(powerUp);
                }

                else if (rand.Next(0, numberOfPowerUps) == 8)             // --- POWERUP: BALLANDPADDLEFAST ---
                {
                    powerUp = new BallAndPaddleFast();
                    Block = new Block(rand.Next(1, 4));
                    Blocks.Add(Block);
                    ListOfPowerUps.Add(powerUp);
                }

                else if (rand.Next(0, numberOfPowerUps) == 9)             // --- POWERUP: BALLANDPADDLESLOW ---
                {
                    powerUp = new BallAndPaddleSlow();
                    Block = new Block(rand.Next(1, 4));
                    Blocks.Add(Block);
                    ListOfPowerUps.Add(powerUp);
                }

                else                                                        // --- NO POWER UP ---
                {
                    Block = new Block(rand.Next(1, 4));
                    Blocks.Add(Block);
                }

                // polymorphism is cool
            }

            // Set up the area in which the blocks will spawn.
            blockArea = new Rectangle(area.X, area.Y + 100, area.Width - 18, area.Height / 4);

            // Position the blocks in the blockArea.
            Block.PositionBlocks(blockArea, Blocks, ListOfPowerUps);

            // Position the ball below the blockArea.
            Globals.IS_BALL_START = true;
            ball.PositionBall(Player, Balls);

        }

        public void Update()
        {
            // Moves the Player Paddle.
            Player.Update();

            // Moves the ball.
            foreach (Ball ball in Balls.ToList())
                ball.Update(Player, Balls);

            // Moves the powerUp when active.
            foreach (PowerUp powerUp in ListOfPowerUps)
                powerUp.Move();

            // Moves the bullet when fired.
            Player.Bullet.Move();

            Collisions();
        }

        void Collisions()
        {
            foreach (Block block in Blocks)
            {
                foreach (Ball ball in Balls)
                {
                    ball.Bounce(Player, block);
                }

                if (Player.Bullet.Body.IntersectsWith(block.Body))
                {
                    block.HitsLeft--;
                    block.ChangeColor(block.HitsLeft);
                    if (block.HitsLeft <= 0)
                    {
                        block.MoveBlock();
                        if (block.powerUp != null) // if the block has an associated powerUp on it.
                            block.powerUp.IsActive = true;
                    }

                    Player.Bullet.ResestPosition();
                }
            }

            foreach (PowerUp powerUp in ListOfPowerUps)
            {
                powerUp.Collide(Player);
                powerUp.Collide(Player, Balls);
                powerUp.Collide(Player, area, Balls);

            }

            foreach (Ball ball in Balls)
            {
                if (Player.Body.IntersectsWith(ball.Body))
                {
                    ball.Bounce(Player);
                }
            }
        }

        public void KeyPressed(KeyEventArgs _e, bool _isPressed)
        {
            if (_e.KeyCode == Keys.Right || _e.KeyCode == Keys.D)
                Player.Right = _isPressed;
            else if (_e.KeyCode == Keys.Left || _e.KeyCode == Keys.A)
                Player.Left = _isPressed;

            if (_e.KeyCode == Keys.Space || _e.KeyCode == Keys.Up)
                Player.Fire = _isPressed;
        }

    }
}
