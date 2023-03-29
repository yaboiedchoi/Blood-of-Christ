using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blood_of_Christ
{
    internal class Key : GameObject
    {
        // Constructor
        public Key(Texture2D asset, Rectangle position)
            : base(asset, position)
        {
        }

        //Methods
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(asset,
                    Position,
                    Color.White);
        }
        public override void Update(GameTime gameTime)
        {
        }
        public bool CheckCollision(Player player)
        {
            return player.Position.Intersects(position);
        }
    }
}
