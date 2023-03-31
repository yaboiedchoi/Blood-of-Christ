using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blood_of_Christ
{
    public abstract class GameObject
    {
        protected Texture2D asset;
        protected Rectangle position;

        /// <summary>
        /// To allow modifications with sprite's placing
        /// </summary>
        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        public GameObject(Texture2D texture, Rectangle position)
        {
            this.asset = texture;
            this.position = position;
        }

        /// <summary>
        /// For sprite movements
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        //Maybe to have it?
        // I think it'd be useful to use it, it's easier to draw everything this way - Sean
        public abstract void Draw(SpriteBatch sb);
    }
    
}
