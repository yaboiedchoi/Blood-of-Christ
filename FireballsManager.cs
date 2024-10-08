﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

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
        private SoundEffect sound;

        /// <summary>
        /// Allows us to access fireballs inside the fireball manager
        /// </summary>
        public List<Fireballs> Fireballs
        {
            get { return fireballsManager; }
        }
        public FireballsManager(Texture2D asset, Rectangle rectangle, SoundEffect sound)
        {
            this.asset = asset;
            this.rect = rectangle;
            this.sound = sound; 
            fireballsManager = new List<Fireballs>();
        }

        /// <summary>
        /// To run the fireballs and check when it removes
        /// </summary>
        /// <param name="gametime">time param</param>
        public void Update(GameTime gametime)
        {
            foreach(Fireballs fireball in fireballsManager)
            {
                fireball.Update(gametime);
            }
            Remove();
        }

        /// <summary>
        /// Draws fireballs from the list
        /// </summary>
        /// <param name="sb">Takes in the spritebatch</param>
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
                if (fireballsManager[0].Position.X < 85)
                {
                    fireballsManager.RemoveAt(0);
                }
            }            
        }

        /// <summary>
        /// If player collides with the fireball, he takes damage and fireball is removed
        /// </summary>
        /// <param name="rect">Takes in the player rect</param>
        public int TakeDamage(Rectangle rect)
        {
            foreach(Fireballs fireball in fireballsManager)
            {
                if (fireball.Position.Intersects(rect) )
                {
                    if (!MediaPlayer.IsMuted)
                    {
                        sound.Play();
                    }
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
