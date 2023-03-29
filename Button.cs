using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Blood_of_Christ
{
    /// <summary>
    /// If button is clicked, use buttons "on button click" event
    /// </summary>
    public delegate void OnButtonClickDelegate();
    internal class Button
    {
        // Button position / texture
        private Rectangle rect;

        // Button color
        private Color buttonColor;
        private Color hoveredColor;
        private Color pressedColor;

        private Color currentColor;

        // Text content / position
        private string text;
        private SpriteFont font;
        private Color textColor;
        private Vector2 textLocation;

        // Input
        private MouseState prevMState;

        /// <summary>
        /// On button click event
        /// </summary>
        public event OnButtonClickDelegate OnButtonClick;

        /// <summary>
        /// returns the x value of the button, setting the value moves the button
        /// </summary>
        public int X
        {
            get
            {
                return rect.X;
            }
            set
            {
                rect.X = value;
            }
        }
        /// <summary>
        /// returns the y value of the button, setting the value moves the button
        /// </summary>
        public int Y
        {
            get
            {
                return rect.Y;
            }
            set
            {
                rect.Y = value;
            }
        }
        public Button(GraphicsDevice gd, Rectangle rect, Color buttonColor, Color hoveredColor, Color pressedColor, string text, SpriteFont font, Color textColor)
        {
            this.rect = rect;
            this.buttonColor = buttonColor;
            this.hoveredColor = hoveredColor;
            this.pressedColor = pressedColor;
            this.text = text;
            this.font = font;
            this.textColor = textColor;

            currentColor = buttonColor;

            Vector2 textSize = font.MeasureString(text);
            textLocation = new Vector2(
                                      (rect.X + rect.Width / 2) - textSize.X / 2,
                                      (rect.Y + rect.Height / 2) - textSize.Y / 2
                                      );
        }
        public void Update()
        {
            // grabs mouse current state
            MouseState mState = Mouse.GetState();

            if (((mState.LeftButton == ButtonState.Released && prevMState.LeftButton == ButtonState.Pressed) || // if button is pressed via right or left click
                 (mState.RightButton == ButtonState.Released && prevMState.RightButton == ButtonState.Pressed)) &&
                  rect.Contains(mState.Position))
            {
                currentColor = pressedColor;
                if(OnButtonClick != null)
                {
                    OnButtonClick();
                }
            }
            else if (rect.Contains(mState.Position)) // if the mouse is on top of the button
            {
                currentColor = hoveredColor;
            }
            else if (currentColor != buttonColor)// resets the color back to original color
            {
                currentColor = buttonColor;
            }
            
            // grabs previous mouse state
            prevMState = mState;
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(null, rect, currentColor);
            sb.DrawString(font, text, textLocation, textColor);
        }
    }
}
