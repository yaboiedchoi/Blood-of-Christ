using Blood_of_Christ.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Blood_of_Christ
{
    public enum GameState
    {
        Title,
        Game,
        GameOver,
        Controls, 
        Settings
    }
    public class Game1 : Game
    {
        // KNOWN BUGS:
        // fireball does not reset location when restarting game
        // TODO: Add a pause menu
        // NOTE: we should probably move all of these to manager classes later
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Game State
        private GameState gs;

        //for fireballs
        private Texture2D tex_fireball;
        private Rectangle rect_fireball;
        private Fireballs fireballs;
        private Detector detector;
        private FireballsManager fireballManager;

        //Keys
        private KeyboardState prevKey;
        private KeyboardState currentKey;

        //for debug ONLY
        private SpriteFont debugFont;
        private double playerHealth;
        private Rectangle rect_player;

        //for priest and attack
        private Texture2D tex_priest;
        private Rectangle rect_priest;
        private Priest priest;
        Rectangle priestPrevPosition;
        private bool isMoving = false;

        // player
        private Player player;
        private Rectangle rect_health;
        private Rectangle rect_batTimer;
        private Texture2D tex_bar;
        private Rectangle rect_playerPrevPos; //For detectors to set off the fireballs
                                              //private Priest priest;
        // Tiling system
        private Texture2D tex_key;
        private Texture2D tex_tiles;
        private Tile tiles;

        // Goal
        private Goal goal;
        private Texture2D tex_goal;

        // Window
        private int windowWidth;
        private int windowHeight;

        //attack system
        private Texture2D tex_detector;
        private Rectangle rect_detector;
        private Texture2D tex_light;

        // Play Game button in main menu
        private Button startButton;
        private Texture2D debugButtonTexture;

        // Settings button in main menu
        private Button settingsButton;

        // Back button, goes back to the main menu
        private Button backButton;

        // Controls button in main menu
        private Button controlsButton;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            prevKey = Keyboard.GetState();
            windowWidth = GraphicsDevice.Viewport.Width;
            windowHeight = GraphicsDevice.Viewport.Height;
            
            gs = GameState.Title;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            tex_fireball = Content.Load<Texture2D>("fireball");
            tex_priest = Content.Load<Texture2D>("back up priest");
            tex_bar = Content.Load<Texture2D>("health_bar_placeholder");
            tex_key = Content.Load<Texture2D>("key");
            tex_tiles = Content.Load<Texture2D>("tilesAssets");
            tex_detector = Content.Load<Texture2D>("detector");
            tex_light = Content.Load<Texture2D>("light");
            tex_goal = Content.Load<Texture2D>("SolidWhite");

            // player
            player = new Player(tex_bar, new Rectangle(100, 400, 50, 50));
            rect_playerPrevPos = rect_player;        
            rect_health = new Rectangle(10, 10, 100, 20);
            rect_batTimer = new Rectangle(10, 40, 100, 20);

            //Attack system and Manager class for firballs
            rect_fireball = new Rectangle(windowWidth + 100, player.Position.Y, tex_fireball.Width / 5, tex_fireball.Height / 5);
            rect_detector = new Rectangle(500, 100, tex_detector.Width, tex_detector.Height);
            fireballManager = new FireballsManager(tex_fireball, rect_fireball);

            detector = new Detector(tex_detector, rect_detector, windowHeight, tex_light);
            rect_priest = new Rectangle(0, 100, tex_priest.Width / 5, tex_priest.Height / 5);

            //fireball
            fireballs = new Fireballs(tex_fireball,
                                      rect_fireball);
                                      
            //Adding for priest
            //demo_texPriest = Content.Load<Texture2D>("priest");
            priest = new Priest(windowWidth, windowHeight, tex_priest, rect_priest);
            priestPrevPosition = priest.Position;
            //DEBUG PURPOSES
            rect_player = new Rectangle(100, 0, 50, 50);

            debugFont = Content.Load<SpriteFont>("debugFont2");
            debugButtonTexture = Content.Load<Texture2D>("SolidWhite");

            // Tiles
            tiles = new Tile(tex_tiles, tex_key, tex_goal, player);
            tiles.WindowTiles();
            tiles.LoadStage();

            // All buttons
            startButton = new Button(debugButtonTexture, new Rectangle(50, 150, 50, 20), 
                                     Color.Red, Color.Orange, Color.DarkRed, "play game", debugFont, Color.Black);
            settingsButton = new Button(debugButtonTexture, new Rectangle(150, 150, 50, 20), 
                                     Color.Red, Color.Orange, Color.DarkRed, "settings", debugFont, Color.Black);
            backButton = new Button(debugButtonTexture, new Rectangle(20, 20, 50, 20), Color.Red, 
                                     Color.Orange, Color.DarkRed, "back", debugFont, Color.Black);
            controlsButton = new Button(debugButtonTexture, new Rectangle(250, 150, 50, 20), 
                                     Color.Red, Color.Orange, Color.DarkRed, "controls", debugFont, Color.Black);
            // hooking up
            startButton.OnButtonClick += this.StartGame;
            settingsButton.OnButtonClick += this.SettingsMenu;
            backButton.OnButtonClick += this.TitleScreen;
            controlsButton.OnButtonClick += this.ControlsMenu;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            switch (gs)
            {
                case GameState.Title: // title
                    // button test
                    startButton.Update(gameTime);
                    controlsButton.Update(gameTime);
                    settingsButton.Update(gameTime);

                    break;
                case GameState.Game: // game
                    player.ResetX = 150;
                    player.ResetY = 500;
                    player.Update(gameTime);
                    priest.Update(gameTime);
                    fireballManager.Update(gameTime);

                    if (detector.Detection.Intersects(player.PrevPos) &&
                        !detector.Detection.Intersects(player.Position))
                    {
                        isMoving = true;
                        fireballManager.Add(player);
                    }

                    // Player Takes Damage if colliding with fireballs
                        
                    for (int i = 0; i < fireballManager.Count; i++)
                    {
                        player.TakeDamage(fireballManager.Fireballs[i]);
                        if (fireballManager.Fireballs[i].Position.Intersects(player.Position))
                        {
                            fireballManager.Fireballs.RemoveAt(i);
                        }
                    }

                    // IF player dies, change state to game over screen and removes fireballs
                    if (player.IsDead)
                    {
                        fireballManager.Clear();
                        gs = GameState.GameOver;
                    }

                    for (int i = 0; i < tiles.Goal.Count; i++)
                    {
                        if (tiles.Goal[i].CheckCollision(player))
                        {
                            gs = GameState.GameOver;
                        }
                    }
                    

                    // I made some slight changes based on an idea I had
                    // player now has a timer that gives them 1 second of invulnerability when they're hit
                    // this means they won't immediately lose all their health
                    // - Sean
                    if (player.Position.Intersects(priest.Position) &&
                        player.HitTime <= 0)
                    {
                        player.TakeDamage(priest);
                    }                    

                    rect_health.Width = (int)(player.Health * 2.5);
                    rect_batTimer.Width = (int)(player.BatTime * 83.3);

                    tiles.Update(gameTime);

                    //player.PrevPos = player.Position;
                    base.Update(gameTime);

                    //priestPrevPosition = priestCurrentPos;
                    player.PrevPos = player.Position;
                    break;
                case GameState.GameOver:
                    backButton.Update(gameTime);
                    break;
                case GameState.Settings:
                    backButton.Update(gameTime);
                    break;
                case GameState.Controls:
                    backButton.Update(gameTime);
                    break;
            }

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            switch (gs)
            {
                case GameState.Title: // title

                    //button test
                    startButton.Draw(_spriteBatch);
                    controlsButton.Draw(_spriteBatch);
                    settingsButton.Draw(_spriteBatch);
                    break;

                case GameState.Game: // game
                    tiles.Draw(_spriteBatch);
                    
                    // health and ability bars
                    _spriteBatch.Draw(
                        tex_bar,
                        rect_health,
                        Color.White);
                    _spriteBatch.Draw(
                        tex_bar,
                        rect_batTimer,
                        Color.AliceBlue);
                    player.Draw(_spriteBatch);

                    // Enemy
                    priest.Draw(_spriteBatch);
                    detector.Draw(_spriteBatch);                    
                    fireballManager.Draw(_spriteBatch);

                    // Goal

                    break;
                case GameState.Settings:
                    backButton.Draw(_spriteBatch);
                    break;

                case GameState.Controls:
                    backButton.Draw(_spriteBatch);
                    break;

                case GameState.GameOver:
                    _spriteBatch.DrawString(debugFont, "game over", new Vector2(20, 50), Color.Black);
                    backButton.Draw(_spriteBatch);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        /// <summary>
        /// simple event method (will be changed later
        /// </summary>
        protected void StartGame()
        {
            player.Reset(150, 500);
            gs = GameState.Game;
        }
        protected void TitleScreen()
        {
            gs = GameState.Title;
        }
        protected void SettingsMenu()
        {
            gs = GameState.Settings;
        }
        protected void ControlsMenu()
        {
            gs = GameState.Controls;    
        }
    }
}
