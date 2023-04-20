using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blood_of_Christ
{
    internal class Goal : GameObject
    {

        public Goal(Texture2D texture, Rectangle position)
            : base(texture, position)
        {
            this.position = position;
            this.texture = texture;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(
                texture,
                position,
                Color.White);
        }

        public override void Update(GameTime gameTime)
        {
        }
        public bool CheckCollision(Player player)
        {
            if (player.Position.Intersects(this.position)) 
            {
                return true;
            }
            return false;
        }
    }
}
