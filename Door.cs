using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blood_of_Christ
{
    /// <summary>
    /// Door class has same physics as platforms but will open when key is collected
    /// Constructor inherit from GameObject class
    /// </summary>
    internal class Door : Platform
    {
        // Constructor
        /// <summary>
        /// Constructor to initiate asset and position of door
        /// </summary>
        /// <param name="asset">Texture of platform</param>
        /// <param name="position">Position of platform</param>
        public Door(Texture2D asset, Rectangle position)
            : base(asset, position)
        {
            SpritePosition = new Rectangle(64, 256, 48, 48);
        }
    }
}
