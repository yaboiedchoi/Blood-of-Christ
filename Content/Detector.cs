using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Blood_of_Christ.Content
{
    public delegate void DetectionCheck();
    public class Detector: GameObject
    {
        //calls the fireballs manager and and checks if anyone has entered or not.
        private Texture2D asset;
        private Texture2D lightAsset;
        private Rectangle position;
        private Rectangle rect_detection;
        private int windowHeight;
        private Rectangle prevPos;

        /// <summary>
        /// Based on the data given it makes a new rectangle which will check if user crosses through it
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="position"></param>
        /// <param name="windowHeight"></param>
        public Detector(Texture2D asset, Rectangle position,int windowHeight, Texture2D lightAsset) : base(asset, position)
        {
            this.asset = asset;
            this.position = position;
            rect_detection = new Rectangle(position.X, position.Y, position.Width, windowHeight);
            this.lightAsset = lightAsset;
        }

        /// <summary>
        /// Allows us to use in game1 for detection
        /// </summary>
        public Rectangle Detection
        {
            get { return rect_detection; }
        }


        /// <summary>
        /// No purpose as of now
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
            //As of now does nothing
        }

        /// <summary>
        /// Prints texture as intended
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(asset,
                    position,
                    Color.White);
            sb.Draw(lightAsset,
                    Detection,
                    Color.White);
        }
    }
}
