using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blood_of_Christ
{
    /// <summary>
    /// Key class is collectible to open the doors
    /// Constructor, Update, and Draw inherit from GameObject class
    /// </summary>
    internal class Key : GameObject
    {
        // Constructor
        /// <summary>
        /// Constructor to initiate asset and position of key
        /// </summary>
        /// <param name="asset">Texture of platform</param>
        /// <param name="position">Position of platform</param>
        public Key(Texture2D asset, Rectangle position)
            : base(asset, position)
        {
        }

        //Methods

        /// <summary>
        /// Overriden method of Update but does nothing
        /// </summary>
        /// <param name="gameTime">The current snapshot of the game time</param>
        public override void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Overriden method of Draw to draw key texture on its position
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(asset,
                    Position,
                    Color.White);
        }

        /// <summary>
        /// Check if player collides to the key
        /// </summary>
        /// <param name="player">Player of this game</param>
        /// <returns>Boolean if player collides key</returns>
        public bool CheckCollision(Player player)
        {
            return player.Position.Intersects(position);
        }
    }
}
