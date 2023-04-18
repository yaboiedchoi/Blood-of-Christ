using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blood_of_Christ
{
    public class FireballsManager
    {
        //Holds fireball objects and removes them when they're offscreen from the list
        //private Fireballs fireballs;
        private Queue<Fireballs> fireballsManager;
        private double windowWidth;
        private Texture2D asset;
        private Rectangle rect;
        private int count;

        public FireballsManager(Texture2D asset, Rectangle rectangle)
        {
            this.asset = asset;
            this.rect = rectangle;
            fireballsManager = new Queue<Fireballs>();
        }

        public void Update(GameTime gametime)
        {
            foreach(Fireballs fireball in fireballsManager)
            {
                fireball.Update(gametime);
            }
            Remove();
        }

        public void Draw(SpriteBatch sb)
        {
            foreach(Fireballs fireball in fireballsManager)
            {
                fireball.Draw(sb);
            }
        }

        /// <summary>
        /// Counts amount of fireballs
        /// </summary>
        public int Count
        {
            get { return fireballsManager.Count; }
        }

        /// <summary>
        /// Adds a fireball
        /// </summary>
        public void Add()
        {
            fireballsManager.Enqueue(new Fireballs(asset, rect));
        }

        /// <summary>
        /// Removes the top most fireball if it goes offscreen 
        /// </summary>
        public void Remove()
        {
            if(fireballsManager.Count >0)
            {
                if (fireballsManager.Peek().Position.X < -asset.Width)
                {
                    fireballsManager.Dequeue();
                }
            }
            
        }
        
        /// <summary>
        /// Clears everything
        /// </summary>
        public void Clear()
        {
            fireballsManager.Clear();
        }


    }
}
