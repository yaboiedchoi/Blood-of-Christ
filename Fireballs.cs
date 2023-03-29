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
        private double xVel;
        private double time;
        private double distance;

        public Fireballs(Texture2D asset, Rectangle position):
            base(asset, position)
        {
            xVel = 5;
            this.asset = asset;
            this.position = position;
        }

        /// <summary>
        /// Ensures bullets move from right to left at constant speed
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            double deltaX;
            time = gameTime.ElapsedGameTime.TotalSeconds;
            deltaX = xVel * time;
            position.X += (int)deltaX;
            Position = position;
        }

        /// <summary>
        /// Draws the bullets onto the screen
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(asset,
                    Position,
                    Color.White);
        }
    }
}
