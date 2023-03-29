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
    internal class Player : GameObject
    {
        // Field
        private Rectangle prevPos;
        private float xVelocity;
        private float yVelocity;
        private float gravity;
        private bool isOnGround;
        private int health;
        private int resetX;
        private int resetY;
        private bool isDead;

        // Property
        public int Health
        {
            get { return health; }
            set { health = value; }
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

        public bool IsOnGround
        {
            get { return isOnGround; }
            set { isOnGround = value; }
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
            gravity = .6f;
            health = 100;
            isDead = false;
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

            position.Y += (int)yVelocity;

            // player movement
            if (kbstate.IsKeyDown(Keys.Left))
            {
                    position.X -= 5;
            }
            if (kbstate.IsKeyDown(Keys.Right))
            {
                    position.X += 5;
            }
            if (kbstate.IsKeyDown(Keys.Up))
            {
                TakeDamage(1);
            }
            if (isDead)
            {
                Reset(ResetX, ResetY);
            }
            yVelocity += gravity;
        }

        #region Player Physics
        /// <summary>
        /// Player physics to move on ground and collide to wall
        /// </summary>
        /// <param name="platform">Platform or door as obstacles</param>
        /// <param name="_graphics">GraphicDeviceManager</param>
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
                // The player can jump only if they are on the ground
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
        #endregion

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health < 0)
            {
                isDead = true;
            }
        }
        public void Reset(float x, float y)
        {
            health = 100;
            position.X = (int)x;
            position.Y = (int)y;
            isDead = false;
        }
    }
}

