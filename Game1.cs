using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Blood_of_Christ
{
    public class Game1 : Game
    {
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
        private Rectangle rect_priest;
        private Priest priest;

        // player
        private Player player;

        // platforms
        List<Platform> platforms;

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

            //TODO: Not sure about how to place the fireballs- probably from right to left
            rect_fireball = new Rectangle(0,0, tex_fireball.Width/5, tex_fireball.Height/5);
            rect_priest = new Rectangle(0, 100, tex_priest.Width / 5, tex_priest.Height/5);

            // player
            player = new Player(tex_priest, new Rectangle(100, 0, 50, 50));
            platforms.Add(new Platform(tex_fireball, new Rectangle(0, 300, 300, 50)));
            platforms.Add(new Platform(tex_fireball, new Rectangle(400, 300, 500, 50)));
            platforms.Add(new Platform(tex_fireball, new Rectangle(500, 0, 50, 200)));

            debugFont = Content.Load<SpriteFont>("debug font");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            player.Update(gameTime);
            foreach (Platform platform in platforms)
            {
                platform.Collision(player, _graphics);
            }
            player.PrevPos = player.Position;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            player.Draw(_spriteBatch);

            foreach (Platform platform in platforms)
            {
                platform.Draw(_spriteBatch);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}