﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Blood_of_Christ
{
    internal class Player : GameObject
    {
        // Field
        private Rectangle prevPos;  // record's the players position for collision purposes
        private int playerSize;     // record's the players default (vampire) size for transformation purposes
        private float xVelocity;    // player's current horizontal speed
        private float yVelocity;    // player's current vertical speed
        private float gravity;      // how fast the player will begin to fall.
        private int health;         // total player health. Starts at 100.
        private int resetX;         // X coordinate for the current level start
        private int resetY;         // Y coordinate for the current level start
        private bool isDead;        // if the player is currently dead (health is less than 0)
        private bool isBat;         // if the player is in a bat form
        private double batTime;      // timer for how long bat can stay a bat
        private int timeElapsed;    // time for how long the player has been a bat

        // Property
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        public double BatTime
        {
            get { return batTime; }
        }
        public int X
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public int Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public Rectangle PrevPos
        {
            get { return prevPos; }
            set { prevPos = value; }
        }

        public float XVelocity
        {
            get { return xVelocity; }
            set { xVelocity = value; }
        }

        public float YVelocity
        {
            get { return yVelocity; }
            set { yVelocity = value; }
        }

        public int ResetX
        {
            get { return resetX; }
            set { resetX = value; }
        }

        public int ResetY
        {
            get { return resetY; }
            set { resetY = value; }
        }

        // Constructor
        public Player(Texture2D texture, Rectangle position)
            : base(texture, position)
        {
            yVelocity = 0f;
            gravity = .61f;
            health = 100;
            isDead = false;
            isBat = false;
            batTime = 7;
            playerSize = position.Width;
        }

        // Methods
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(
                base.asset,
                position,
                Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kbstate = Keyboard.GetState();
           // If player is dead, calls Reset
            if (isDead)
            {
                Reset(ResetX, ResetY);
            }

            //if player is vampire
            if (!isBat)
            {
                position.Width = playerSize;
                position.Height = playerSize;

                if (batTime < 7)
                {
                    batTime += gameTime.ElapsedGameTime.TotalSeconds;
                }
                // changes the player position by the Y velocity
                position.Y += (int)yVelocity;
                // transforming to bat
                if (kbstate.IsKeyDown(Keys.E))
                {
                    position.Width /= 2;    // bat width is half the size of human
                    position.Height /= 2;   // bat form hitbox is a perfect square
                    isBat = true;
                }
            }

            // bat form has additional vertical movement
            if (isBat)
            {
                batTime -= gameTime.ElapsedGameTime.TotalSeconds;
                if (batTime <= 0)
                {
                    isBat = false;
                }

                if (kbstate.IsKeyDown(Keys.Up))
                {
                    position.Y -= 5;
                }
                if (kbstate.IsKeyDown(Keys.Down))
                {
                    position.Y += 5;
                }

                if (kbstate.IsKeyDown(Keys.W))
                {
                    position.Width *= 2;
                    position.Height *= 2;
                    isBat = false;
                }

                yVelocity = 1;
            }
            // basic player movement
            if (kbstate.IsKeyDown(Keys.Left))
            {
                position.X -= 5;
            }
            if (kbstate.IsKeyDown(Keys.Right))
            {
                position.X += 5;
            }
            
            // adds gravity to the y velocity
            yVelocity += gravity;
        }

        public void Physics(Rectangle platform, GraphicsDeviceManager _graphics)
        {
            while ((prevPos.X + prevPos.Width <= platform.X &&                                 // If player was left from the wall
                    position.Intersects(platform)) ||                                          // and now intersects the wall
                    position.X + position.Width >= _graphics.GraphicsDevice.Viewport.Width)    // Or, when player is getting out of screen
            {
                position.X--;
                if (xVelocity > 0)
                {
                    xVelocity = 0;
                }
            }

            while ((prevPos.X >= platform.X + platform.Width &&    // If player was right from the wall
                    position.Intersects(platform)) ||              // and now intersects the wall
                    position.X <= 0)                               // Or, when player is getting out of screen
            {
                position.X++;
                if (xVelocity < 0)
                {
                    xVelocity = 0;
                }
            }

            while ((prevPos.Y + prevPos.Height <= platform.Y &&                                  // If player was up from the wall
                    position.Intersects(platform)) ||                                            // and now intersects the wall
                    position.Y + position.Height >= _graphics.GraphicsDevice.Viewport.Height)    // Or, when player is getting out of screen
            {
                position.Y--;
                if (yVelocity > 0)
                {
                    yVelocity = 0;
                }

                // The player can jump only if they are on the ground as a vampire
                KeyboardState kbstate = Keyboard.GetState();
                if (kbstate.IsKeyDown(Keys.Space) &&
                    yVelocity == 0)
                {
                    yVelocity = -15;
                }
            }

            while ((prevPos.Y >= platform.Y + platform.Height &&    // If player was down from the wall
                    position.Intersects(platform)) ||               // and now intersects the wall
                    position.Y <= 0)                                // Or, when player is getting out of screen
            {
                position.Y++;
                if (yVelocity < 0)
                {
                    yVelocity = 0;
                }
            }
        }

        /// <summary>
        /// Written by Sean
        /// Allows the player to take a given amount of damage
        /// Checks
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                isDead = true;
            }
        }

        /// <summary>
        /// Written by Sean
        /// Resets the players health to 100 and the isDead bool to false
        /// Then returns the player to the level's start
        /// </summary>
        /// <param name="x"> x coordinate for the player's starting point </param>
        /// <param name="y"> y coordinate for the player's starting point </param>
        public void Reset(float x, float y)
        {
            health = 100;
            position.X = (int)x;
            position.Y = (int)y;
            isDead = false;
            isBat = false;
            batTime = 7;
        }
    }
}

