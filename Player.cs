using System;
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
        private int health;         // total player health. Starts at 100.
        private int resetX;         // X coordinate for the current level start
        private int resetY;         // Y coordinate for the current level start
        private bool isBat;         // if the player is in a bat form
        private double batTime;     // timer for how long bat can stay a bat
        private double hitTime;     // When player is hit, they will be invulnerable until this value == 0
        private bool godMode;       // Whether or not the player takes damage
        private animState anim;     

        // Property
        public int Health
        {
            get { return health; }
            set { health = value; }
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
            hitTime = 1;
            anim = animState.standingRight;
        }

        // Methods

        public override void Update(GameTime gameTime)
        {
            hitTime -= gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState kbstate = Keyboard.GetState();
            // player can only take damage when hitTime = 0.
            if (hitTime < 0)
            {
                hitTime = 0;
                
            }

           // If player is dead, calls Reset
           // now no need, since there is a game over screen
            //if (isDead)
            //{
            //    Reset(ResetX, ResetY);
            //}

            //if player is vampire
            if (!isBat)
            {
                position.Width = playerSize;
                position.Height = playerSize;

                // Bat Meter recharges when not in bat form
                if (batTime < 3)
                {
                    batTime += gameTime.ElapsedGameTime.TotalSeconds/2;
                }

                // transforming into bat
                if (kbstate.IsKeyDown(Keys.E))
                {
                    position.Width /= 2;    // bat width is half the size of human
                    position.Height /= 2;   // bat form hitbox is a perfect square
                    isBat = true;
                }
                // changes the player position by the Y velocity
                switch (anim)
                {
                    case animState.standingRight:
                        {
                            if (kbstate.IsKeyDown(Keys.Left))
                            {
                                anim = animState.walkingLeft;
                            }
                            if (kbstate.IsKeyDown(Keys.Right))
                            {
                                anim = animState.walkingRight;
                            }
                            break;
                        }
                    case animState.standingLeft:
                        {
                            if (kbstate.IsKeyDown(Keys.Left))
                            {
                                anim = animState.walkingLeft;
                            }
                            if (kbstate.IsKeyDown(Keys.Right))
                            {
                                anim = animState.walkingRight;
                            }
                            if (kbstate.IsKeyDown(Keys.Space))
                            {
                                anim = animState.jumpingLeft;
                            }
                            break;
                        }

                    case animState.walkingLeft:
                        {

                            if (kbstate.IsKeyUp(Keys.Left))
                            {
                                anim = animState.standingLeft;
                            }

                            if (kbstate.IsKeyDown(Keys.Right))
                            {
                                anim = animState.walkingRight;
                            }

                            if (kbstate.IsKeyDown(Keys.Space))
                            {
                                anim = animState.jumpingLeft;
                            }
                            position.X -= 5;
                            break;

                        }
                    case animState.walkingRight:
                        {

                            if (!kbstate.IsKeyDown(Keys.Right))
                            {
                                anim = animState.standingRight;
                            }
                            if (kbstate.IsKeyDown(Keys.Left))
                            {
                                anim = animState.walkingLeft;
                            }

                            if (kbstate.IsKeyDown(Keys.Space))
                            {
                                anim = animState.jumpingRight;
                            }
                            position.X += 5;
                            break;
                        }
                    case animState.jumpingLeft:
                        {

                            if (kbstate.IsKeyUp(Keys.Left))
                            {
                                anim = animState.fallingLeft;
                            }
                            if (kbstate.IsKeyDown(Keys.Right))
                            {
                                anim = animState.jumpingRight;
                            }
                            if (yVelocity == 0 && kbstate.IsKeyDown(Keys.Right))
                            {
                                anim = animState.walkingRight;
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

                            if (kbstate.IsKeyUp(Keys.Right))
                            {
                                anim = animState.fallingRight;
                            }
                            if (kbstate.IsKeyDown(Keys.Left))
                            {
                                anim = animState.jumpingLeft;
                            }
                            if (yVelocity == 0 && kbstate.IsKeyDown(Keys.Left))
                            {
                                anim = animState.walkingLeft;
                            }
                            if (yVelocity == 0)
                            {
                                anim = animState.standingRight;
                            }
                            position.X += 5;
                            break;                            


                        }
                    case animState.fallingRight:
                        {
                            if (kbstate.IsKeyDown(Keys.Right))
                            {
                                anim = animState.jumpingRight;
                            }
                            if (kbstate.IsKeyDown(Keys.Left))
                            {
                                anim = animState.jumpingLeft;
                            }
                            if (yVelocity >= 0)
                            {
                                anim = animState.standingRight;
                            }
                            break;
                        }
                    case animState.fallingLeft:
                        {
                            if (kbstate.IsKeyDown(Keys.Right))
                            {
                                anim = animState.jumpingRight;
                            }
                            if (kbstate.IsKeyDown(Keys.Left))
                            {
                                anim = animState.jumpingLeft;
                            }
                            if (yVelocity >= 0)
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

                // transforming back into a player
                if (kbstate.IsKeyDown(Keys.W) && kbstate.IsKeyUp(Keys.E))
                {
                    position.Width *= 2;
                    position.Height *= 2;
                    isBat = false;
                }

                yVelocity = -5;
            }
            /*
            // basic
            if (kbstate.IsKeyDown(Keys.Left))
            {
                position.X -= 5;
                anim = animState.walkingLeft;
            }
            if (kbstate.IsKeyDown(Keys.Right))
            {
                position.X += 5;
                anim = animState.walkingRight;
            }
            */


            
            // adds gravity to the y velocity
            yVelocity += gravity;  

        }

        public override void Draw(SpriteBatch sb)
        {
            switch(anim)
            {
                case animState.jumpingLeft:
                    {
                        sb.Draw(
                            base.texture,
                            position,
                            Color.Black);
                        break;
                    }
                case animState.walkingLeft: 
                    {
                        sb.Draw(
                            base.texture,
                            position, 
                            Color.White); 
                        break;
                    }
                case animState.walkingRight:
                    {
                        sb.Draw(
                            base.texture,
                            position,
                            Color.Black);
                        break;
                    }
                case animState.jumpingRight:
                    {
                        sb.Draw(
                            base.texture,
                            position,
                            Color.White);
                        break;
                    }
                case animState.standingLeft:
                    {
                        sb.Draw(
                            base.texture,
                            position,
                            Color.White);
                        break;
                    }
                case animState.standingRight:
                    {
                        sb.Draw(
                            base.texture,
                            position,
                            Color.Black);
                        break;
                    }
                case animState.fallingRight:
                    {
                        sb.Draw(
                            base.texture,
                            position,
                            Color.Black);
                        break;
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
                if (kbstate.IsKeyDown(Keys.Space) &&
                    yVelocity == 0)
                {
                    yVelocity = -15;
                }
            }

            while ((prevPos.Y >= platform.Y + platform.Height &&    // If player was down from the wall
                    position.Intersects(platform)))                 // and now intersects the wall
            {
                position.Y++;
                if (yVelocity < 0)
                {
                    yVelocity = 0;
                }
            }
        }
        #endregion

        /// <summary>
        /// Written by Sean
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
        /// Written by Sean
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
            godMode = false;
        }
    }
}

