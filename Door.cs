using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blood_of_Christ
{
    internal class Door : Platform
    {
        // Constructor
        public Door(Texture2D asset, Rectangle position)
            : base(asset, position)
        {
        }
    }
}
