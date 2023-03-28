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
        // Button position
        private float xValue;
        private float yValue;

        // Button color
        private Color buttonColor;
        private Color hoveredColor;
        private Color pressedColor;

        // Text content
        private string text;
        private SpriteFont font;
        private Color textColor;

        // Text position
        private float textXValue;
        private float textYValue;

        /// <summary>
        /// On button click event
        /// </summary>
        public event OnButtonClickDelegate OnButtonClick;

        /// <summary>
        /// returns the x value of the button, setting the value moves the button, and moves the text in relation
        /// </summary>
        public float X
        {
            get
            {
                return xValue;
            }
            set
            {
                float previousXValue = xValue;
                xValue = value;
                float differenceInXValues = xValue - previousXValue;
                textXValue += differenceInXValues;
            }
        }
        /// <summary>
        /// returns the y value of the button, setting the value moves the button, and moves the text in relation
        /// </summary>
        public float Y
        {
            get
            {
                return yValue;
            }
            set
            {
                float previousYValue = yValue;
                yValue = value;
                float differenceInYValues = yValue - previousYValue;
                textYValue += differenceInYValues;
            }
        }
        /// <summary>
        /// Default constructor
        /// </summary>
        public Button()
        {
            xValue = 0;
            yValue = 0;
            buttonColor = Color.Pink;
            hoveredColor = Color.White;
            pressedColor = Color.Red;
            text = null;
            font = null;
            textColor = Color.White;
            textXValue = 0;
            textYValue = 0;
        }
        /// <summary>
        /// Perameterized constructor
        /// </summary>
        /// <param name="xValue">x coordinate of the button</param>
        /// <param name="yValue">y coordinate of the button</param>
        /// <param name="buttonColor">color of the button</param>
        /// <param name="hoveredColor">color of the button when hovered with mouse</param>
        /// <param name="pressedColor">color of the button when pressed</param>
        /// <param name="text">text over the button</param>
        /// <param name="font">font of the text</param>
        /// <param name="textColor">color of the text</param>
        /// <param name="textXValue">text x value</param>
        /// <param name="textYValue">text y value</param>
        public Button(float xValue, float yValue, Color buttonColor, Color hoveredColor, Color pressedColor, string text, SpriteFont font, Color textColor, float textXValue, float textYValue, float x, float y)
        {
            this.xValue = xValue;
            this.yValue = yValue;
            this.buttonColor = buttonColor;
            this.hoveredColor = hoveredColor;
            this.pressedColor = pressedColor;
            this.text = text;
            this.font = font;
            this.textColor = textColor;
            this.textXValue = textXValue;
            this.textYValue = textYValue;
        }
    }
}
