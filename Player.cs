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
    public class Player : GameObject
    {
        // animation enumerator
        private enum animState
        {
            standingRight,
            standingLeft,
            walkingRight,
            walkingLeft,
            jumpingRight,
            jumpingLeft,
            fallingLeft,
            fallingRight,
        }

        // Field
        private Rectangle prevPos;  // record's the players position for collision purposes
        private int playerSize;     // record's the players default (vampire) size for transformation purposes
        private float xVelocity;    // player's current horizontal speed
        private float yVelocity;    // player's current vertical speed
        private float gravity;      // how fast the player will begin to fall.
        private float health;         // total player health. Starts at 100.
        private int resetX;         // X coordinate for the current level start
        private int resetY;         // Y coordinate for the current level start
        private bool isBat;         // if the player is in a bat form
        private double batTime;     // timer for how long bat can stay a bat
        private double hitTime;     // When player is hit, they will be invulnerable until this value == 0
        private bool godMode;       // Whether or not the player takes damage

        // animation variables
        private animState anim;
        private int spriteWidth;
        private int currentFrame;
        private double fps;
        private double secondsPerFrame;
        private double timeCounter;

        // keyboard state
        private KeyboardState kbState;
        private KeyboardState prevKbState;

        // Property
        public float Health
        {
            get { return health; }
            set
            {
                if (!godMode)
                {
                    health = value;
                }
            }
        }
        public bool IsDead
        {
            get
            {
                if (health <= 0) return true;
                else return false;
            }
        }
        public double BatTime
        {
            get { return batTime; }
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

        public double HitTime
        {
            get { return hitTime; }
        }

        public bool GodMode
        {
            get { return godMode; }
            set { godMode = value; }
        }

        // Constructor
        public Player(Texture2D texture, Rectangle position)
            : base(texture, position)
        {
            yVelocity = 0f;
            gravity = .61f;
            health = 100;
            isBat = false;
            batTime = 3;
            playerSize = position.Width;
            hitTime = .5;

            // animation data
            anim = animState.standingRight;
            spriteWidth = texture.Width / 8;
            fps = 12;
            secondsPerFrame = 1.0 / fps;
            timeCounter = 0;
            currentFrame = 1;
        }

        // Methods
        public override void Update(GameTime gameTime)
        {
            hitTime -= gameTime.ElapsedGameTime.TotalSeconds;
            kbState = Keyboard.GetState();
            // player can only take damage when hitTime = 0.
            if (hitTime < 0)
            {
                hitTime = 0;                
            }

            //if player is vampire
            if (!isBat)
            {
                UpdateAnimation(gameTime);
                position.Width = playerSize;
                position.Height = playerSize;

                // Bat Meter recharges when not in bat form
                if (batTime < 3)
                {
                    batTime += gameTime.ElapsedGameTime.TotalSeconds/2;
                }

                // switch statement for anim states and player movement
                switch (anim)
                {
                    // standing
                    case animState.standingRight:
                        {
                            if (kbState.IsKeyDown(Keys.A))
                            {
                                anim = animState.walkingLeft;
                            }
                            if (kbState.IsKeyDown(Keys.D))
                            {
                                anim = animState.walkingRight;
                            }
                            if (kbState.IsKeyDown(Keys.W))
                            {
                                anim = animState.fallingRight;
                            }
                            break;
                        }
                    case animState.standingLeft:
                        {
                            if (kbState.IsKeyDown(Keys.A))
                            {
                                anim = animState.walkingLeft;
                            }
                            if (kbState.IsKeyDown(Keys.D))
                            {
                                anim = animState.walkingRight;
                            }
                            if (kbState.IsKeyDown(Keys.W))
                            {
                                anim = animState.fallingLeft;
                            }
                            break;
                        }

                    // walking
                    case animState.walkingLeft:
                        {
                            if (kbState.IsKeyUp(Keys.A))
                            {
                                anim = animState.standingLeft;
                            }

                            if (kbState.IsKeyDown(Keys.D))
                            {
                                anim = animState.walkingRight;
                            }

                            if (kbState.IsKeyDown(Keys.W))
                            {
                                anim = animState.jumpingLeft;
                            }
                            position.X -= 5;
                            break;
                        }
                    case animState.walkingRight:
                        {
                            if (!kbState.IsKeyDown(Keys.D))
                            {
                                anim = animState.standingRight;
                            }
                            if (kbState.IsKeyDown(Keys.A))
                            {
                                anim = animState.walkingLeft;
                            }

                            if (kbState.IsKeyDown(Keys.W))
                            {
                                anim = animState.jumpingRight;
                            }
                            position.X += 5;
                            break;
                        }

                    // jumping (moving left/right while in the air)
                    case animState.jumpingLeft:
                        {

                            if (kbState.IsKeyUp(Keys.A))
                            {
                                anim = animState.fallingLeft;
                            }
                            if (kbState.IsKeyDown(Keys.D))
                            {
                                anim = animState.jumpingRight;
                            }
                            if (yVelocity == 0)
                            {
                                anim = animState.standingLeft;
                            }
                            position.X -= 5;
                            break;

                        }
                    case animState.jumpingRight:
                        {
                            if (kbState.IsKeyUp(Keys.D))
                            {
                                anim = animState.fallingRight;
                            }
                            if (kbState.IsKeyDown(Keys.A))
                            {
                                anim = animState.jumpingLeft;
                            }
                            if (yVelocity == 0)
                            {
                                anim = animState.standingRight;
                            }
                            position.X += 5;
                            break;                            
                        }

                    // falling (midjump but no horizontal movement)
                    case animState.fallingRight:
                        {
                            if (kbState.IsKeyDown(Keys.D))
                            {
                                anim = animState.jumpingRight;
                            }
                            if (kbState.IsKeyDown(Keys.A))
                            {
                                anim = animState.jumpingLeft;
                            }
                            if (yVelocity == 0)
                            {
                                anim = animState.standingRight;
                            }
                            break;
                        }
                    case animState.fallingLeft:
                        {
                            if (kbState.IsKeyDown(Keys.D))
                            {
                                anim = animState.jumpingRight;
                            }
                            if (kbState.IsKeyDown(Keys.A))
                            {
                                anim = animState.jumpingLeft;
                            }
                            if (yVelocity == 0)
                            {
                                anim = animState.standingLeft;
                            }
                            break;
                        }
                }
                position.Y += (int)yVelocity;
            }

            // bat form has additional vertical movement
            if (isBat)
            {
                batTime -= gameTime.ElapsedGameTime.TotalSeconds;
                // transforms back into a player when timer runs out
                if (batTime <= 0)
                {
                    isBat = false;
                }

                if (kbState.IsKeyDown(Keys.W))
                {
                    position.Y -= 5;
                }
                if (kbState.IsKeyDown(Keys.S))
                {
                    position.Y += 5;
                }
                if (kbState.IsKeyDown(Keys.A))
                {
                    position.X -= 5;
                }
                if (kbState.IsKeyDown(Keys.D))
                {
                    position.X += 5;
                }

                // transforming out of bat causes player to be bounced into the air slightly
                yVelocity = -5;
            }

            // adds gravity to the y velocity
            yVelocity += gravity;

            // transforming into bat
            if (kbState.IsKeyDown(Keys.E) && prevKbState.IsKeyUp(Keys.E))
            {
                if (isBat)
                {
                    position.Width *= 2;
                    position.Height *= 2;
                    isBat = false;
                }
                else if (!isBat)
                {
                    position.Width /= 2;    // bat width is half the size of human
                    position.Height /= 2;   // bat form hitbox is a perfect square
                    isBat = true;
                }
            }

            // prev kb state
            prevKbState = kbState;
        }

        /// <summary>
        ///  Draws the player differently depending on what anim state they're in
        /// </summary>
        /// <param name="sb"> spritebatch </param>
        public override void Draw(SpriteBatch sb)
        {
            // drawing as bat
            if (isBat)
            {
                DrawAsBat(SpriteEffects.None, sb);
            }

            // drawing as vampire
            else
            {
                switch (anim)
                {
                    // drawing player jumping
                    case animState.jumpingLeft:
                        {
                            DrawPlayerJumping(SpriteEffects.FlipHorizontally, sb);
                            break;
                        }
                    case animState.jumpingRight:
                        {
                            DrawPlayerJumping(SpriteEffects.None, sb);
                            break;
                        }

                    // drawing player walking
                    case animState.walkingLeft:
                        {
                            DrawPlayerWalking(SpriteEffects.FlipHorizontally, sb);
                            break;
                        }
                    case animState.walkingRight:
                        {
                            DrawPlayerWalking(SpriteEffects.None, sb);
                            break;
                        }

                    // drawing player standing
                    case animState.standingLeft:
                        {
                            DrawPlayerStanding(SpriteEffects.FlipHorizontally, sb);
                            break;
                        }

                    case animState.standingRight:
                        {
                            DrawPlayerStanding(SpriteEffects.None, sb);
                            break;
                        }

                    // drawing player falling
                    case animState.fallingRight:
                        {
                            DrawPlayerJumping(SpriteEffects.None, sb);
                            break;
                        }
                    case animState.fallingLeft:
                        {
                            DrawPlayerJumping(SpriteEffects.FlipHorizontally, sb);
                            break;
                        }
                }
            }
        }

        #region Player Physics
        /// <summary>
        /// Player physics to move on ground and collide to wall
        /// </summary>
        /// <param name="platform">Platform or door as obstacles</param>
        public void Physics(Rectangle platform)
        {
            while ((prevPos.X + prevPos.Width <= platform.X &&    // If player was left from the wall
                    position.Intersects(platform)))               // and now intersects the wall
            {
                position.X--;
                if (xVelocity > 0)
                {
                    xVelocity = 0;
                }
            }

            while ((prevPos.X >= platform.X + platform.Width &&    // If player was right from the wall
                    position.Intersects(platform)))                // and now intersects the wall
            {
                position.X++;
                if (xVelocity < 0)
                {
                    xVelocity = 0;
                }
            }

            while ((prevPos.Y + prevPos.Height <= platform.Y &&    // If player was up from the wall
                    position.Intersects(platform)))                // and now intersects the wall
            {
                position.Y--;
                if (yVelocity > 0)
                {
                    yVelocity = 0;
                }

                // The player can jump only if they are on the ground as a vampire
                KeyboardState kbstate = Keyboard.GetState();
                if (kbstate.IsKeyDown(Keys.W) &&
                    yVelocity == 0)
                {
                    yVelocity = -16;
                }
            }

            while ((prevPos.Y >= platform.Y + platform.Height &&    // If player was down from the wall
                    position.Intersects(platform)))                 // and now intersects the wall
            {
                position.Y++;
                if (yVelocity < 0)
                {
                    yVelocity = 1;
                }
            }
        }
        #endregion

        /// <summary>
        /// Player loses health when colliding with certain gameObjects
        /// Checks
        /// </summary>
        /// <param name="other"> Whatever gameobject the player is colliding with </param>
        public void TakeDamage(GameObject other)
        {
            if (position.Intersects(other.Position))
            {
                if (!godMode && hitTime == 0)
                {
                    if (other is Fireballs)
                    {
                        health -= 20;
                    }
                    if (other is Priest)
                    {
                        health -= 40;
                    }    
                    hitTime = 1;
                }
            }
        }

        /// <summary>
        /// Resets the players health to 100
        /// Then returns the player to the level's start
        /// </summary>
        /// <param name="x"> x coordinate for the player's starting point </param>
        /// <param name="y"> y coordinate for the player's starting point </param>
        public void Reset(float x, float y)
        {
            health = 100;
            position.X = (int)x;
            position.Y = (int)y;
            isBat = false;
            batTime = 3;
        }

        /// <summary>
        /// Animates the player by changing the current frame over time
        /// </summary>
        /// <param name="gameTime"> elapsed time </param>
        private void UpdateAnimation(GameTime gameTime)
        {
            //Counts time before switching to next frame
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            //checks that enough time has passed
            if (timeCounter >= secondsPerFrame) 
            {
                // changes the current frame as a loop
                currentFrame++;
                if (currentFrame >= 8)
                {
                    currentFrame = 1;
                }

                // resetting timer
                timeCounter -= secondsPerFrame;
            }
        }

        /// <summary>
        /// Draws player with an animated walk cylce
        /// </summary>
        /// <param name="flip"> whether it will face left or not </param>
        /// <param name="sb"> drawn with spritebatch </param>
        private void DrawPlayerWalking(SpriteEffects flip, SpriteBatch sb)
        {
            sb.Draw(
                texture,
                new Vector2(position.X-25, position.Y-50),
                new Rectangle(
                    currentFrame * spriteWidth,
                    0,
                    spriteWidth,
                    100),
                Color.White,
                0.0f,
                Vector2.Zero,
                1.0f,
                flip,
                0.0f);
        }

        /// <summary>
        /// Draws the player standing
        /// </summary>
        /// <param name="flip"> whether it will face left or not </param>
        /// <param name="sb"> drawn with spritebatch </param>
        private void DrawPlayerStanding(SpriteEffects flip, SpriteBatch sb) 
        {
            sb.Draw(
                texture,
                new Vector2(position.X - 25, position.Y - 50),
                new Rectangle(
                    0,
                    100,
                    spriteWidth,
                    100),
                Color.White,
                0.0f,
                Vector2.Zero,
                1.0f,
                flip,
                0.0f);
        }

        /// <summary>
        /// Draws the player jumping
        /// </summary>
        /// <param name="flip"> whether it will face left or not </param>
        /// <param name="sb"> drawn with spritebatch </param>
        private void DrawPlayerJumping(SpriteEffects flip, SpriteBatch sb) 
        {
            sb.Draw(
                texture,
                new Vector2(position.X - 25, position.Y - 50),
                new Rectangle(
                    100,
                    100,
                    spriteWidth,
                    100),
                Color.White,
                0.0f,
                Vector2.Zero,
                1.0f,
                flip,
                0.0f);
        }

        /// <summary>
        /// draws the player as a bat
        /// </summary>
        /// <param name="flip"> whether it will face left or not </param>
        /// <param name="sb"> drawn with spritebatch </param>
        private void DrawAsBat(SpriteEffects flip, SpriteBatch sb)
        {
            sb.Draw(
                texture,
                new Vector2(position.X - 25, position.Y - 50),
                new Rectangle(
                    200,
                    100,
                    spriteWidth,
                    100),
                Color.White,
                0.0f,
                Vector2.Zero,
                1.0f,
                flip,
                0.0f);
        }
    }
}

