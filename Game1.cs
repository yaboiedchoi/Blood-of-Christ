using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace Blood_of_Christ
{
    public enum GameState
    {
        Title,
        Game,
        GameOver,
        Controls, 
        Settings,
        Victory
    }
    public class Game1 : Game
    {
        // KNOWN BUGS:
        // player buffers a
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
        Rectangle priestPrevPosition;
        private bool isMoving = false;

        // player
        private Player player;
        private Texture2D tex_player;
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
        private Texture2D tex_goal;
        private int level;

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

        // mute song button
        private Button muteButton;

        // Back button, goes back to the main menu
        private Button backButton;

        // Controls button in main menu
        private Button controlsButton;

        //fonts
        private SpriteFont header;
        private SpriteFont body;

        // song
        private Song theme;
        //Sound effect for hit
        private SoundEffect hitSound;

        // title screen picture
        private Texture2D titleScreen;

        // god mode button
        private Button godModeButton;

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
            tex_bar = Content.Load<Texture2D>("SolidWhite");
            tex_key = Content.Load<Texture2D>("key");
            tex_tiles = Content.Load<Texture2D>("tilesAssets");
            tex_detector = Content.Load<Texture2D>("detector");
            tex_light = Content.Load<Texture2D>("light");
            tex_goal = Content.Load<Texture2D>("goal");
            tex_player = Content.Load<Texture2D>("player_sprites");
            titleScreen = Content.Load<Texture2D>("title_screen");
            hitSound = Content.Load<SoundEffect>("hit");

            // player
            player = new Player(tex_player, new Rectangle(100, 400, 50, 50));
            level = 1;
            rect_playerPrevPos = rect_player;        
            rect_health = new Rectangle(10, 10, 100, 20);
            rect_batTimer = new Rectangle(10, 40, 100, 20);

            //Attack system and Manager class for firballs
            rect_fireball = new Rectangle(windowWidth + 100, player.Position.Y, tex_fireball.Width / 7, tex_fireball.Height / 7);
            rect_detector = new Rectangle(500, 100, tex_detector.Width, tex_detector.Height);
            fireballManager = new FireballsManager(tex_fireball, rect_fireball, hitSound);

            rect_priest = new Rectangle(0, 100, tex_priest.Width / 5, tex_priest.Height / 5);

            //fireball
            fireballs = new Fireballs(tex_fireball,
                                      rect_fireball);

            //fonts
            header = Content.Load<SpriteFont>("Header");
            body = Content.Load<SpriteFont>("BodyFont");
                                     
            //DEBUG PURPOSES
            rect_player = new Rectangle(100, 0, 50, 50);

            debugFont = Content.Load<SpriteFont>("debugFont2");
            debugButtonTexture = Content.Load<Texture2D>("buttonTexture");

            // Tiles
            tiles = new Tile(tex_tiles, tex_key, tex_goal, 
                tex_detector, tex_light, tex_priest, player);
            tiles.WindowTiles();
            tiles.LoadStage(level);

            // song
            theme = Content.Load<Song>("blood_of_christ_theme");
            MediaPlayer.Play(theme);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.25f;

            // All buttons
            startButton = new Button(debugButtonTexture, new Rectangle(50, 150, 150, 50), 
                                     Color.Red, Color.Orange, Color.DarkRed, "Play Game", body, Color.Black);
            settingsButton = new Button(debugButtonTexture, new Rectangle(350, 150, 150, 50), 
                                     Color.Red, Color.Orange, Color.DarkRed, "Settings", body, Color.Black);
            backButton = new Button(debugButtonTexture, new Rectangle(20, 20, 70, 30), Color.Red, 
                                     Color.Orange, Color.DarkRed, "Back", body, Color.Black);
            controlsButton = new Button(debugButtonTexture, new Rectangle(650, 150, 150, 50), 
                                     Color.Red, Color.Orange, Color.DarkRed, "Controls", body, Color.Black);
            muteButton = new Button(debugButtonTexture, new Rectangle(20, 100, 200, 30), Color.Red,
                                     Color.Orange, Color.DarkRed, "Mute Audio: False", body, Color.Black);
            godModeButton = new Button(debugButtonTexture, new Rectangle(20, 150, 200, 30), Color.Red,
                                     Color.Orange, Color.DarkRed, "God Mode: False", body, Color.Black);
            // hooking up
            startButton.OnButtonClick += this.StartGame;
            settingsButton.OnButtonClick += this.SettingsMenu;
            backButton.OnButtonClick += this.TitleScreen;
            controlsButton.OnButtonClick += this.ControlsMenu;
            muteButton.OnButtonClick += this.MuteMusic;
            godModeButton.OnButtonClick += this.ToggleGodMode;
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
                    fireballManager.Update(gameTime);

                    // detectors will summon fireballs when player walks under them
                    for (int i = 0; i < tiles.Detector.Count; i++)
                    {
                        if (tiles.Detector[i].Detection.Intersects(player.Position) &&
                            !tiles.Detector[i].Detection.Intersects(player.PrevPos))
                        {
                            isMoving = true;
                            for (int j = 0; j < 3; j++)
                            {
                                fireballManager.Add(player);
                            }
                        }
                        // detector also turns red and slowly drains the player health
                        if (tiles.Detector[i].Detection.Intersects(player.Position))
                        {
                            tiles.Detector[i].Collided = true;
                            player.Health -= .1f;
                        }
                        else
                        {
                            tiles.Detector[i].Collided = false;
                        }
                    }

                    // player takes damage when colliding with the priest.
                    for (int i = 0; i < tiles.Priests.Count; i++)
                    {
                        if (player.Position.Intersects(tiles.Priests[i].Position))
                        {                            
                            player.TakeDamage(tiles.Priests[i]);
                        }
                    }

                    // Player Takes Damage if colliding with fireballs
                        
                    for (int i = 0; i < fireballManager.Count; i++)
                    {
                        player.TakeDamage(fireballManager.Fireballs[i]);
                        if (fireballManager.Fireballs[i].Position.Intersects(player.Position))
                        {
                            if (!MediaPlayer.IsMuted)
                            {
                                hitSound.Play(.05f, 0, 0);
                            }
                            fireballManager.Fireballs.RemoveAt(i);
                        }
                    }

                    // IF player dies, change state to game over screen and removes fireballs
                    if (player.IsDead)
                    {
                        fireballManager.Clear();
                        gs = GameState.GameOver;
                    }
                    try
                    {
                        for (int i = 0; i < tiles.Goal.Count; i++)
                        {
                            if (tiles.Goal[i].CheckCollision(player))
                            {
                                level++;
                                player.Reset(150, 540);
                                tiles.LoadStage(level);
                                fireballManager.Clear();
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        gs = GameState.Victory;
                    }

                    rect_health.Width = (int)(player.Health * 2.5);
                    rect_batTimer.Width = (int)(player.BatTime * 83.3);

                    tiles.Update(gameTime);

                    base.Update(gameTime);

                    player.PrevPos = player.Position;
                    break;
                case GameState.GameOver:
                    backButton.Update(gameTime);
                    tiles.Priests.Clear();
                    break;
                case GameState.Settings:
                    if (MediaPlayer.IsMuted)
                    {
                        muteButton.Text = "Mute Audio: True";
                        muteButton.ButtonColor = Color.DarkOrange;
                    }
                    else if (!MediaPlayer.IsMuted)
                    {
                        muteButton.Text = "Mute Audio: False";
                        muteButton.ButtonColor = Color.Red;
                    }

                    if (player.GodMode)
                    {
                        godModeButton.Text = "God Mode: True";
                        godModeButton.ButtonColor = Color.DarkOrange;
                    }
                    else if (!player.GodMode)
                    {
                        godModeButton.Text = "God Mode: False";
                        godModeButton.ButtonColor = Color.Red;
                    }
                    backButton.Update(gameTime);
                    muteButton.Update(gameTime);
                    godModeButton.Update(gameTime);
                    break;
                case GameState.Controls:
                    backButton.Update(gameTime);
                    break;
                case GameState.Victory:
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
                    _spriteBatch.Draw(titleScreen, new Vector2(0, 0), Color.White);
                    _spriteBatch.DrawString(header,
                                            "The Blood of Christ",
                                            new Vector2(10, 10),
                                            Color.DarkRed);

                    //button test
                    startButton.Draw(_spriteBatch);
                    controlsButton.Draw(_spriteBatch);
                    settingsButton.Draw(_spriteBatch);
                    break;

                case GameState.Game: // game
                    tiles.Draw(_spriteBatch);

                    _spriteBatch.DrawString(
                        body,
                        "Health",
                        new Vector2(250 + 40, rect_health.Y),
                        Color.White);
                    _spriteBatch.DrawString(
                        body,
                        "Bat Timer",
                        new Vector2(250 + 40, rect_batTimer.Y),
                        Color.White);

                    // Draw a string if it is level 1.
                    if (level == 1)
                    {
                        _spriteBatch.DrawString(
                            body,
                            "Press AD for horizontal movement and space for jump\n" +
                            "Press E to toggle bat mode and fly with WASD.\n" +
                            "Light detects you to give damage and shoot a fireball.",
                            new Vector2(300, 240),
                            Color.White);
                    }

                    // health and ability bars
                    _spriteBatch.Draw(
                        tex_bar,
                        rect_health,
                        Color.DeepPink);
                    _spriteBatch.Draw(
                        tex_bar,
                        rect_batTimer,
                        Color.BlueViolet);
                    player.Draw(_spriteBatch);
           
                    fireballManager.Draw(_spriteBatch);

                    // Goal

                    break;
                case GameState.Settings:
                    backButton.Draw(_spriteBatch);
                    muteButton.Draw(_spriteBatch);
                    godModeButton.Draw(_spriteBatch);
                    break;

                case GameState.Controls:
                    backButton.Draw(_spriteBatch);
                    _spriteBatch.DrawString(body,
                                            "Avoid the priest and fireballs to get out of the church! \n" +
                                            "Instructions: \n" +
                                            "WASD keys for movement\n" +
                                            "Spacebar for Jump \n" +
                                            "E for turning into a bat\n\n" +
                                            "CREDITS--- \n" +
                                            "Tile assets credit: https://blackspirestudio.itch.io/medieval-pixel-art-asset-free\n" +
                                            "Title screen credit: https://www.samsonhistorical.com/products/wooden-wine-chalice \n" +
                                            "Vampire and Bat made by Sean Bethel \n" +
                                            "Music by Edward Choi" +
                                            "Sound Effect: https://freesound.org/people/leviclaassen/sounds/107788/\n" +
                                            "Fireball : https://www.freeiconspng.com/img/46732\n" +
                                            "Priest: https://en.wikipedia.org/wiki/File:Coptic_Orthodox_Priest.png",
                                            new Vector2(10, 100),
                                            Color.DarkRed);
                    break;

                case GameState.GameOver:
                    _spriteBatch.DrawString(debugFont, "game over", new Vector2(20, 50), Color.Black);
                    backButton.Draw(_spriteBatch);
                    break;
                case GameState.Victory:
                    string victory = "You win!";
                    _spriteBatch.DrawString(header, "You win!", new Vector2(100, 10), Color.DarkRed);
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
            player.Reset(150, 540);
            level = 1;
            tiles.LoadStage(level);
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

        /// <summary>
        /// Mutes music
        /// </summary>
        protected void MuteMusic()
        {
            if (MediaPlayer.IsMuted)
                MediaPlayer.IsMuted = false;
            else
                MediaPlayer.IsMuted = true;
        }

        /// <summary>
        /// Disables damage
        /// </summary>
        protected void ToggleGodMode()
        {
            if (player.GodMode)
            {
                player.GodMode = false;
            }
            else
            {
                player.GodMode = true;
            }
        }
    }
}
