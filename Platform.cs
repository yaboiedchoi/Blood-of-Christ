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
        public virtual void Collision(Player player, GraphicsDeviceManager _graphics)
        {
            while ((player.PrevPos.X + player.PrevPos.Width <= position.X &&                                 // If player was left from the wall
                    player.Position.Intersects(position)) ||                                                 // and now intersects the wall
                    player.Position.X + player.Position.Width >= _graphics.GraphicsDevice.Viewport.Width)    // Or, when player is getting out of screen
            {
                player.X--;
                if (player.XVelocity > 0)
                {
                    player.XVelocity = 0;
                }
                player.IsOnGround = false;
            }

            while ((player.PrevPos.X >= position.X + position.Width &&    // If player was right from the wall
                    player.Position.Intersects(position)) ||              // and now intersects the wall
                    player.Position.X <= 0)                               // Or, when player is getting out of screen
            {
                player.X++;
                if (player.XVelocity < 0)
                {
                    player.XVelocity = 0;
                }
                player.IsOnGround = false;
            }

            while ((player.PrevPos.Y + player.PrevPos.Height <= position.Y &&                                  // If player was up from the wall
                    player.Position.Intersects(position)) ||                                                // and now intersects the wall
                    player.Position.Y + player.Position.Height >= _graphics.GraphicsDevice.Viewport.Height)    // Or, when player is getting out of screen
            {
                player.Y--;
                if (player.YVelocity > 0)
                {
                    player.YVelocity = 0;
                }
                // Jump
                KeyboardState kbstate = Keyboard.GetState();
                if (kbstate.IsKeyDown(Keys.Space) &&
                    player.YVelocity == 0)
                {
                    player.YVelocity = -15;
                }
                player.IsOnGround = true;
            }

            while ((player.PrevPos.Y >= position.Y + position.Height &&    // If player was down from the wall
                    player.Position.Intersects(position)) ||               // and now intersects the wall
                    player.Position.Y <= 0)                                // Or, when player is getting out of screen
            {
                player.Y++;
                if (player.YVelocity < 0)
                {
                    player.YVelocity = 0;
                }
                player.IsOnGround = false;
            }
        }
    }
}
