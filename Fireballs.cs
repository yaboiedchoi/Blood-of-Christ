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
    public class Fireballs: GameObject
    {
        private double xVelocity;
        private double time;
        private double totalTime;

        public Fireballs(Texture2D texture, Rectangle position):
            base(texture, position)
        {
            xVelocity = 1;
            this.texture = texture;
            this.position = position;
        }

        /// <summary>
        /// Ensures bullets move from right to left at constant speed
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            double deltaX;
            //Issue with timer ????
            time = gameTime.TotalGameTime.TotalSeconds;
            time -= gameTime.ElapsedGameTime.TotalSeconds;
            deltaX = xVelocity * time;
            position.X -= (int)deltaX;
            Position = position;
        }

        /// <summary>
        /// Draws the bullets onto the screen
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture,
                    Position,
                    Color.White);
        }
    }
}
