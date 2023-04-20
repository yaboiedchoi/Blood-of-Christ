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
        private List<Fireballs> fireballsManager;
        //private double windowWidth;
        private Texture2D asset;
        private Rectangle rect;
        //private int count;

        public List<Fireballs> Fireballs
        {
            get { return fireballsManager; }
        }
        public FireballsManager(Texture2D asset, Rectangle rectangle)
        {
            this.asset = asset;
            this.rect = rectangle;
            fireballsManager = new List<Fireballs>();
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
        public void Add(Player player)
        {
            fireballsManager.Add(new Fireballs(asset, new Rectangle(rect.X, player.Position.Y, rect.Width, rect.Height)));
        }

        /// <summary>
        /// Removes the top most fireball if it goes offscreen 
        /// </summary>
        public void Remove()
        {
            if(fireballsManager.Count > 0)
            {
                if (fireballsManager[0].Position.X < -asset.Width)
                {
                    fireballsManager.RemoveAt(0);
                }
            }            
        }

        /// <summary>
        /// If player collides with the fireball, he takes damage and fireball is removed
        /// </summary>
        /// <param name="rect"></param>
        public int TakeDamage(Rectangle rect)
        {
            foreach(Fireballs fireball in fireballsManager)
            {
                if (fireball.Position.Intersects(rect) )
                    //fireball.Position.Intersects(rectPrev))
                {
                    fireballsManager.Remove(fireball);
                    return 5;
                }
            }

            return 0;
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
