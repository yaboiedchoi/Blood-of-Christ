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
    internal class Button : GameObject
    {
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
        /// Button perameterized constructor
        /// </summary>
        /// <param name="rect">rectangle location of button</param>
        /// <param name="buttonColor">default color of the button</param>
        /// <param name="hoveredColor">button color when mouse hovered</param>
        /// <param name="pressedColor">button color when pressed</param>
        /// <param name="text">text on button</param>
        /// <param name="font">font of button</param>
        /// <param name="textColor">color of text</param>
        public Button(Rectangle rect, Texture2D texture, Color buttonColor, Color hoveredColor, Color pressedColor, string text, SpriteFont font, Color textColor)
            : base (texture, rect)
        {
            texture = texture;
            rect = rect;
            this.buttonColor = buttonColor;
            this.hoveredColor = hoveredColor;
            this.pressedColor = pressedColor;
            this.text = text;
            this.font = font;
            this.textColor = textColor;

            currentColor = buttonColor;

            if(text != null)
            {
                Vector2 textSize = font.MeasureString(text);
                textLocation = new Vector2(
                                          (rect.X + rect.Width / 2) - textSize.X / 2,
                                          (rect.Y + rect.Height / 2) - textSize.Y / 2
                                          );
            }
        }
        /// <summary>
        /// Updates the button depending on its state
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // grabs mouse current state
            MouseState mState = Mouse.GetState();

            if (Position.Contains(mState.Position))
            {
                if ((mState.LeftButton == ButtonState.Released && prevMState.LeftButton == ButtonState.Pressed) || // if button is released after being pressed
                    (mState.RightButton == ButtonState.Released && prevMState.RightButton == ButtonState.Pressed))
                {
                    if (OnButtonClick != null)
                    {
                        OnButtonClick();
                    }
                }
                else if ((mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton == ButtonState.Pressed) || // if button is being held
                         (mState.RightButton == ButtonState.Pressed && prevMState.RightButton == ButtonState.Pressed))
                {
                    currentColor = pressedColor;
                }
                else // if the mouse is on top of the button
                {
                    currentColor = hoveredColor;
                }
            }
            else // resets the color back to original color
            {
                currentColor = buttonColor;
            }
            
            // grabs previous mouse state
            prevMState = mState;
        }
        /// <summary>
        /// Draws the buttons
        /// </summary>
        /// <param name="sb">spritebatch</param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(asset, Position, currentColor);
            sb.DrawString(font, text, textLocation, textColor);
        }
    }
}
