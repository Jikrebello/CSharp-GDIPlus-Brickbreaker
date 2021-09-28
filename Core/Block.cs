using System;
using System.Collections.Generic;
using System.Drawing;

namespace Pong
{
    class Block
    {
        // --- Properties ---
        public int Width { get { return width; } }
        public int Height { get { return height; } }

        // --- Public Fields ---
        public int X;
        public int Y;
        public Rectangle Body;
        public PowerUp powerUp;
        public int HitsLeft;

        // --- Private Fields ---
        readonly SolidBrush brush;
        readonly int width;
        readonly int height;
        Random rand = new Random();

        // --- Constructor ---
        /// <summary>
        /// Creates a Block.
        /// </summary>
        /// <param name="_area"></param>
        public Block(int _hitsLeft)
        {
            width = 40;
            height = 20;
            X = 0;
            Y = 0;

            brush = new SolidBrush(Color.Black);
            HitsLeft = _hitsLeft;
            Body = new Rectangle(X, Y, Width, Height);
            ChangeColor(HitsLeft);
        }

        public void PositionBlocks(Rectangle _blockArea, List<Block> _blocks, List<PowerUp> _powerUps)
        {
            int xCoord = _blockArea.X;
            int yCoord = _blockArea.Y;
            int powerUpCounter = 0;
            int blockTracker = 0;

            foreach (Block block in _blocks)
            {
                // Place the block X
                block.X = xCoord + 5;
                block.Body.X = xCoord + 5;

                // Place the block Y
                block.Y = yCoord + 2;
                block.Body.Y = yCoord + 2;

                int blockPlacer = rand.Next(0, 2);

                if (blockPlacer == 1)
                {
                    if (powerUpCounter >= _powerUps.Count)
                    {
                        blockTracker++;
                    }
                    else
                    {
                        _powerUps[powerUpCounter].PositionPowerUp(block); // hide it behind the current block.
                        block.powerUp = _powerUps[blockTracker]; // make a new association with this block and the powerUp.
                        powerUpCounter++;
                        blockTracker++;
                    }
                }

                if (_blockArea.Width - xCoord < block.Width + 55)
                {
                    xCoord = _blockArea.X + 20;
                    yCoord = yCoord + block.Height + 20;
                }
                else
                    xCoord += block.Width + 20;
            }
        }

        /// <summary>
        /// Moves the block out of the screen to be used later when all the blocks are gone.
        /// </summary>
        public void MoveBlock()
        {
            X = -250;
            Y = -250;
            Body.X = -250;
            Body.Y = -250;
        }

        /// <summary>
        /// Reflects the ball back at the same angle that it came at.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_ball"></param>
        public void Hit(Paddle _player, Ball _ball)
        {
            if (_ball.isDeath) // death ball powerUp.
            {
                HitsLeft = 0;
                _player.Score += 50;
                _ball.isDeath = false;
                _ball.Brush.Color = _ball.OriginalColor;
            }


            HitsLeft--;

            if (HitsLeft <= 0)
            {
                MoveBlock(); // move block out of the way.
                
                if (powerUp != null) // if the block has an associated powerUp on it.
                    powerUp.IsActive = true;

                _player.Score += 75; // add score to the relevant player.
                _ball.Vector.X *= -1;
                _ball.Vector.Y *= -1;
            }
            else if (HitsLeft == 1)
            {
                ChangeColor(1);
                _player.Score += 25;
                _ball.Vector.X *= -1;
                _ball.Vector.Y *= -1;
            }
            else if (HitsLeft == 2)
            {
                ChangeColor(2);
                _ball.Vector.X *= -1;
                _ball.Vector.Y *= -1;
            }
        }

        public void ChangeColor(int _hitsLeft)
        {
            Color oneHitColor = Color.GreenYellow;
            Color twoHitColor = Color.LightGreen;
            Color overTwoHitColor = Color.Green;

            switch (_hitsLeft)
            {
                default:
                    brush.Color = overTwoHitColor;
                    break;
                case 1:
                    brush.Color = oneHitColor;
                    break;
                case 2:
                    brush.Color = twoHitColor;
                    break;
            }
        }

        public void Draw(Graphics _gfx)
        {
            _gfx.FillRectangle(brush, Body);
        }
    }
}
