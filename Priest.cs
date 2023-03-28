using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Blood_of_Christ
{
    public class Priest: GameObject
    {
        private int windowWidth;
        private int windowHeight;
        private Texture2D texture;
        private Rectangle position;

        //To ensure it moves back and forth
        private double xVel = 5;
        private double time;
        private int direction = 1;
        public Priest(int windowWidth, int windowHeight, Texture2D texture, Rectangle position):
            base(texture, position)
        {
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.texture = texture;
            this.position = position;
        }

        /// <summary>
        /// If 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {

            Movement(gameTime);
            //throw new NotImplementedException();
        }

        // I had to add this to priest to test player code without build errors. Feel free to get rid of it - Sean.
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(asset,
                    position,
                    Color.White);
        }

        /// <summary>
        /// To ensure priest bounces back in forth the screen
        /// </summary>
        /// <param name="gametime">Time param</param>
        public void Movement(GameTime gametime)
        {
            int deltaX = 5;

            //If priest reaches the edge of the window, he bounces back
            if(position.X + position.Width > windowWidth)
            {
                direction = -1; 
            }
            else if(position.X  < 0)
            {
                direction = 1;
            }
            position.X += (direction * deltaX);
        }

        public void Attack(Rectangle playerRect)
        {
            if (position.Intersects(playerRect))
            {

            }
        }

    }
}
