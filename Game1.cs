﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Blood_of_Christ
{
    public class Game1 : Game
    {
        // NOTE: we should probably move all of these to manager classes later
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //for fireballs
        private Texture2D tex_fireball;
        private Rectangle rect_fireball;

        //Keys
        private KeyboardState prevKey;
        private KeyboardState currentKey;

        //for debug
        private SpriteFont debugFont;

        //for priest
        private Texture2D tex_priest;
        private Texture2D demo_texPriest;
        private Rectangle rect_priest;
        private Priest priest;

        // player
        private Player player;
        private Rectangle rect_health;
        private Texture2D tex_bar;
        //private Priest priest;

        // platforms
        private List<Platform> platforms;
        private List<Door> doors;
        private Texture2D tex_platform;
        private int windowWidth;
        private int windowHeight;

        // collectibles
        private List<Key> keys;
        private Texture2D tex_key;

        // button example
        private Button button;
        private Texture2D debugButtonTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            prevKey = Keyboard.GetState();
            platforms = new List<Platform>();
            doors = new List<Door>();
            keys = new List<Key>();

            // using the fireball texture as a placeholder for player
            tex_fireball = Content.Load<Texture2D>("fireball");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            tex_fireball = Content.Load<Texture2D>("fireball");
            tex_priest = Content.Load<Texture2D>("back up priest");
            tex_bar = Content.Load<Texture2D>("health_bar_placeholder");
            tex_platform = Content.Load<Texture2D>("platform");
            tex_key = Content.Load<Texture2D>("key");

            //TODO: Not sure about how to place the fireballs- probably from right to left
            rect_fireball = new Rectangle(0,0, tex_fireball.Width/5, tex_fireball.Height/5);
            rect_priest = new Rectangle(0, 100, tex_priest.Width / 5, tex_priest.Height/5);
            
            // player
            player = new Player(tex_bar, new Rectangle(100, 400, 50, 50));

            // platforms
            platforms.Add(new Platform(tex_platform, new Rectangle(0, 300, 300, 50)));
            platforms.Add(new Platform(tex_platform, new Rectangle(400, 300, 500, 50)));
            platforms.Add(new Platform(tex_platform, new Rectangle(500, 0, 50, 200)));

            // doors[i] corresponds to keys[i]
            // Ex) keys[3] will be used to open doors[3]
            doors.Add(new Door(tex_platform, new Rectangle(300, 300, 100, 50)));
            doors.Add(new Door(tex_platform, new Rectangle(500, 200, 50, 100)));
            keys.Add(new Key(tex_key, new Rectangle(700, 400, 50, 50)));
            keys.Add(new Key(tex_key, new Rectangle(0, 100, 50, 50)));

            rect_health = new Rectangle(10, 10, 100, 20);

            //Adding for priest
            //demo_texPriest = Content.Load<Texture2D>("priest");
            priest = new Priest(windowWidth, windowHeight, tex_priest, rect_priest);

            debugFont = Content.Load<SpriteFont>("debugFont2");
            debugButtonTexture = Content.Load<Texture2D>("SolidWhite");
            // button test
            button = new Button(new Rectangle(50, 150, 50, 20), debugButtonTexture, Color.Red, Color.Orange, Color.DarkRed, "test", debugFont, Color.Black);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            player.ResetX = 100;
            player.ResetY = 100;
            player.Update(gameTime);
            // button test
            button.Update(gameTime);

            priest.Update(gameTime);
            rect_health.Width = (int)(player.Health * 2.5);
            foreach (Platform platform in platforms)
            {
                player.Physics(platform.Position, _graphics);
            }
            foreach (Door door in doors)
            {
                player.Physics(door.Position, _graphics);
            }
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].CheckCollision(player))
                {
                    keys.Remove(keys[i]);
                    doors.Remove(doors[i]);
                    i--;
                }
            }
            player.PrevPos = player.Position;
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();            
            _spriteBatch.Draw(
                tex_bar, 
                rect_health, 
                Color.White);
            player.Draw(_spriteBatch);

            //button test
            button.Draw(_spriteBatch);

            //enemy
            priest.Draw(_spriteBatch);

            foreach (Platform platform in platforms)
            {
                platform.Draw(_spriteBatch);
            }
            foreach (Door door in doors)
            {
                door.Draw(_spriteBatch);
            }
            foreach (Key key in keys)
            {
                key.Draw(_spriteBatch);
            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}