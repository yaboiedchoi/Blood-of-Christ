using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blood_of_Christ
{
    public class FireballsManager
    {
        //Pesducode
        //Fireball is added when it's called into by detector
        //How to do that??

        //Holds fireball objects and removes them when they're offscreen from the list
        private Fireballs fireballs;
        private Queue<Fireballs> fireballsManager;
        private double windowWidth;

        private FireballsManager(Fireballs fireballs)
        {
            this.fireballs = fireballs;
            fireballsManager.Enqueue(fireballs);
        }

        public void Remove()
        {
            if(fireballs.Position.X < 0)
            {
                fireballsManager.Dequeue();
            }
        }
        



    }
}
