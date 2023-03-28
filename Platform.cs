using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blood_of_Christ
{
    internal class Platform : GameObject
    {
        // Constructor
        public Platform(Texture2D texture, Rectangle position)
            : base(texture, position)
        {
        }

        // Methods
        public override void Update(GameTime gameTime)
        {
        }
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(base.asset,
                position,
                Color.White);
        }
    }
}
