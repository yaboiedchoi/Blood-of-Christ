using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Blood_of_Christ
{
    //delegate
    public delegate void Attack();
    public class Priest: GameObject
    {
        private int windowWidth;
        private int windowHeight;

        //To ensure it moves back and forth
        private float xVelocity = 1;
        private float yVelocity;
        private float gravity;
        private Rectangle prevPos;

        //temporary for playtest
        private double movetime;

        /// <summary>
        /// To access prev position
        /// </summary>
        public Rectangle PrevPos
        {
            get
            {
                return prevPos;
            }
            set 
            {
                prevPos = value;
            }
        }
        public Priest(int windowWidth, int windowHeight, Texture2D texture, Rectangle position):
            base(texture, position)
        {
            yVelocity = 0f;
            gravity = .61f;
            movetime = 1;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
        }

        /// <summary>
        /// Pathfinding for priest
        /// </summary>
        /// <param name="gameTime"> Takes in time for movement</param>
        public override void Update(GameTime gameTime)
        {
            position.Y += (int)yVelocity;
            yVelocity += gravity;

            //The priest moves opposite direction once movetime is 0
            if (xVelocity > 0)
            {
                movetime -= gameTime.ElapsedGameTime.TotalSeconds;
                if (movetime < 0)
                {
                    xVelocity = -xVelocity;
                    movetime = 2;   
                }
            }
            else if (xVelocity < 0)
            {
                movetime -= gameTime.ElapsedGameTime.TotalSeconds;
                if (movetime < 0)
                {
                    xVelocity = -xVelocity;
                    movetime = 2;
                }
            }

            position.X += (int)xVelocity;
        }

        /// <summary>
        /// Priest physics to ensure it interacts with the environment
        /// </summary>
        /// <param name="platform">Takes in the rectangle that it will collide with</param>
        public void Physics(Rectangle platform)
        {
            while ((prevPos.Y + prevPos.Height <= platform.Y &&    // If player was up from the wall
                    position.Intersects(platform)))                // and now intersects the wall
            {
                position.Y--;
                if (yVelocity > 0)
                {
                    yVelocity = 0;
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

        /// <summary>
        /// Draws the priest facing left or right based on a certain xvelocity
        /// </summary>
        /// <param name="sb">Takes in spritebatch</param>
        public override void Draw(SpriteBatch sb)
        {
            // if going right, face right
            if (xVelocity > 0)
            {
                sb.Draw(texture,
                    position,
                    Color.White);
            }
            // otherwise face left
            else
            {
                sb.Draw(texture,
                    position,
                    null,
                    Color.White,
                    0f,
                    new Vector2(0, 0),
                    SpriteEffects.FlipHorizontally,
                    1f);
            }
        }
    }
}
