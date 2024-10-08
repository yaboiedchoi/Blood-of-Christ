﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blood_of_Christ
{
    public class Fireballs : GameObject
    {
        private double xVelocity;
        private double time;
        //private double totalTime;
        
        public Fireballs(Texture2D texture, Rectangle position):
            base(texture, position)
        {
            xVelocity = 7;
            this.texture = texture;
            this.position = position;
        }

        /// <summary>
        /// Bullets movve at an accelerated speed
        /// </summary>
        /// <param name="gameTime">takes time param</param>
        public override void Update(GameTime gameTime)
        {
            double deltaX;
            //time = gameTime.TotalGameTime.TotalSeconds;
            time += gameTime.ElapsedGameTime.TotalSeconds;
            deltaX = xVelocity  + time*2;
            position.X -= (int)deltaX;
            Position = position;
        }

        /// <summary>
        /// Draws the bullets onto the screen
        /// </summary>
        /// <param name="sb">takes in spritebatch</param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture,
                    Position,
                    Color.White);
        }


    }
}
