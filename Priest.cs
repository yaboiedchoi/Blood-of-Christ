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
        private double time;
        private int direction = 1;
        private float yVelocity;
        private float gravity;
        private Rectangle prevPos;

        //temporary for playtest
        private double movetime;

        public Rectangle Position
        {
            get
            {
                return position;
            }
        }
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
        /// If 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            position.Y += (int)yVelocity;
            yVelocity += gravity;

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
        // I had to add this to priest to test player code without build errors. Feel free to get rid of it - Sean.
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture,
                    position,
                    Color.White);
        }
    }
}
