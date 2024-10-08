﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blood_of_Christ
{
    /// <summary>
    /// Platform allows player to move on ground or collide on wall
    /// Constructor, Update, and Draw inherit from GameObject class
    /// </summary>
    public class Platform : GameObject
    {
        // Field
        private Rectangle spritePosition;

        /// <summary>
        /// Read only sprite's position
        /// </summary>
        public Rectangle SpritePosition
        {
            set { spritePosition = value; }
        }

        // Constructor
        /// <summary>
        /// Constructor to initiate asset and position of platform
        /// </summary>
        /// <param name="asset">Texture of platform</param>
        /// <param name="position">Position of platform</param>
        public Platform(Texture2D asset, Rectangle position)
            : base(asset, position)
        {
            //Intentionally empty
        }

        // Methods

        /// <summary>
        /// Overriden method of Update but does nothing
        /// </summary>
        /// <param name="gameTime">The current snapshot of the game time</param>
        public override void Update(GameTime gameTime)
        {
            //Nothing to add
        }

        /// <summary>
        /// Overriden method of Draw to draw platform texture on its position
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(
                texture,
                position,
                spritePosition,
                Color.White);
        }
    }
}
