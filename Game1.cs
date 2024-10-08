﻿using Microsoft.Xna.Framework;
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
        private FireballsManager fireballManager;

        //for priest
        private Texture2D tex_priest;

        // player
        private Player player;
        private Texture2D tex_player;
        private Rectangle rect_health;
        private Rectangle rect_batTimer;
        private Texture2D tex_bar;

        // Tiling system
        private Texture2D tex_key;
        private Texture2D tex_tiles;
        private Tile tiles;

        // Goal
        private Texture2D tex_goal;
        private int level;

        // Window
        private int windowWidth;

        //attack system
        private Texture2D tex_detector;
        private Texture2D tex_light;

        // Play Game button in main menu
        private Button startButton;
        private Texture2D debugButtonTexture;
        private Texture2D pressedButtonTexture;

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

        // quit button
        private Button quitButton;

        // return to game button
        private Button returnButton;

        // keyboard states
        private KeyboardState currentKbState;
        private KeyboardState previousKbState;

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
            windowWidth = GraphicsDevice.Viewport.Width;
            
            gs = GameState.Title;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            tex_fireball = Content.Load<Texture2D>("fireball");
            tex_priest = Content.Load<Texture2D>("priest");
            tex_bar = Content.Load<Texture2D>("SolidWhite");
            tex_key = Content.Load<Texture2D>("key");
            tex_tiles = Content.Load<Texture2D>("tilesAssets");
            tex_detector = Content.Load<Texture2D>("lamp");
            tex_light = Content.Load<Texture2D>("light");
            tex_goal = Content.Load<Texture2D>("goal");
            tex_player = Content.Load<Texture2D>("player_sprites");
            titleScreen = Content.Load<Texture2D>("title_screen");
            hitSound = Content.Load<SoundEffect>("hit");

            // player
            player = new Player(tex_player, new Rectangle(100, 400, 50, 50));
            level = 1;       
            rect_health = new Rectangle(10, 10, 100, 20);
            rect_batTimer = new Rectangle(10, 40, 100, 20);

            //Attack system and Manager class for firballs
            rect_fireball = new Rectangle(windowWidth + 100, player.Position.Y, tex_fireball.Width / 7, tex_fireball.Height / 7);
            fireballManager = new FireballsManager(tex_fireball, rect_fireball, hitSound);

            //fonts
            header = Content.Load<SpriteFont>("Header");
            body = Content.Load<SpriteFont>("BodyFont");
                                     
            //DEBUG PURPOSES
            debugButtonTexture = Content.Load<Texture2D>("buttonTexture");
            pressedButtonTexture = Content.Load<Texture2D>("pressedButtonTexture");

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
            startButton = new Button(debugButtonTexture, pressedButtonTexture, new Rectangle((windowWidth / 2) - 75, 400, 150, 50), 
                                     Color.Red, Color.Orange, Color.DarkRed, "Play Game", body, Color.Black);
            settingsButton = new Button(debugButtonTexture, pressedButtonTexture, new Rectangle((windowWidth / 2) - 300, 400, 150, 50), 
                                     Color.Red, Color.Orange, Color.DarkRed, "Settings", body, Color.Black);
            backButton = new Button(debugButtonTexture, pressedButtonTexture, new Rectangle(150, 100, 70, 30), Color.Red, 
                                     Color.Orange, Color.DarkRed, "Back", body, Color.Black);
            controlsButton = new Button(debugButtonTexture, pressedButtonTexture, new Rectangle((windowWidth / 2) + 150, 400, 150, 50), 
                                     Color.Red, Color.Orange, Color.DarkRed, "Controls", body, Color.Black);
            muteButton = new Button(debugButtonTexture, pressedButtonTexture, new Rectangle(150, 150, 200, 30), Color.Red,
                                     Color.Orange, Color.DarkRed, "Mute Audio: False", body, Color.Black);
            godModeButton = new Button(debugButtonTexture, pressedButtonTexture, new Rectangle(150, 200, 200, 30), Color.Red,
                                     Color.Orange, Color.DarkRed, "God Mode: False", body, Color.Black);
            quitButton = new Button(debugButtonTexture, pressedButtonTexture, new Rectangle((windowWidth / 2) - 75, 500, 150, 50),
                                    Color.Red, Color.Orange, Color.DarkRed, "Quit", body, Color.Black);
            returnButton = new Button(debugButtonTexture, pressedButtonTexture, new Rectangle((windowWidth / 2) - 100, 400, 200, 50),
                                     Color.Red, Color.Orange, Color.DarkRed, "Return to Game", body, Color.Black);
            // hooking up
            returnButton.OnButtonClick += this.StartGame;
            startButton.OnButtonClick += this.StartGame;
            settingsButton.OnButtonClick += this.SettingsMenu;
            backButton.OnButtonClick += this.TitleScreen;
            controlsButton.OnButtonClick += this.ControlsMenu;
            muteButton.OnButtonClick += this.MuteMusic;
            godModeButton.OnButtonClick += this.ToggleGodMode;
            quitButton.OnButtonClick += this.Exit;
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            switch (gs)
            {
                // finite state machine
                case GameState.Title: // title
                    // stage 0 is title screen
                    tiles.LoadStage(0);
                    // buttons
                    startButton.Update(gameTime);
                    controlsButton.Update(gameTime);
                    settingsButton.Update(gameTime);
                    quitButton.Update(gameTime);
                    break;
                case GameState.Game: // game
                    player.ResetX = 150;
                    player.ResetY = 500;
                    player.Update(gameTime);
                    fireballManager.Update(gameTime);

                    // pause menu
                    // keyboard state for only pause menu

                    currentKbState = Keyboard.GetState();
                    if (currentKbState.IsKeyDown(Keys.Escape) && previousKbState.IsKeyUp(Keys.Escape))
                    {
                        gs = GameState.Title;
                    }
                    // previous
                    previousKbState = currentKbState;
                    // detectors will summon fireballs when player walks under them
                    for (int i = 0; i < tiles.Detector.Count; i++)
                    {
                        if (tiles.Detector[i].Detection.Intersects(player.Position) &&
                            !tiles.Detector[i].Detection.Intersects(player.PrevPos))
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                fireballManager.Add(player);
                            }
                        }
                        // detector also turns red and slowly drains the player health
                        if (tiles.Detector[i].Detection.Intersects(player.Position))
                        {
                            tiles.Detector[i].Collided = true;
                            player.Health -= .3f;
                        }
                        else
                        {
                            tiles.Detector[i].Collided = false;
                        }
                    }

                    // player takes damage when colliding with a priest.
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
                    catch
                    {
                        gs = GameState.Victory;
                    }

                    rect_health.Width = (int)(player.Health * 2.5);
                    rect_batTimer.Width = (int)(player.BatTime * 83.3);

                    tiles.Update(gameTime);

                    base.Update(gameTime);

                    player.PrevPos = player.Position;
                    break;
                case GameState.GameOver: // if the player dies
                    // load the background
                    tiles.LoadStage(0);
                    // button
                    backButton.Update(gameTime);
                    // clear the priests
                    tiles.Priests.Clear();
                    break;
                case GameState.Settings: // settings menu
                    // loads background
                    tiles.LoadStage(0);
                    // if the audio is muted
                    if (MediaPlayer.IsMuted)
                    {
                        // draw the button as "muted"
                        muteButton.Text = "Mute Audio: True";
                        muteButton.ButtonColor = Color.DarkOrange;
                    }
                    // if the audio is not muted
                    else if (!MediaPlayer.IsMuted)
                    {
                        // draw theb utton as "not muted"
                        muteButton.Text = "Mute Audio: False";
                        muteButton.ButtonColor = Color.Red;
                    }
                    // if godmode is enabled
                    if (player.GodMode)
                    {
                        // draw the button as "god mode on"
                        godModeButton.Text = "God Mode: True";
                        godModeButton.ButtonColor = Color.DarkOrange;
                    }
                    // if godmode is off
                    else if (!player.GodMode)
                    {
                        // draw the button as "god mode off"
                        godModeButton.Text = "God Mode: False";
                        godModeButton.ButtonColor = Color.Red;
                    }
                    // buttons
                    backButton.Update(gameTime);
                    muteButton.Update(gameTime);
                    godModeButton.Update(gameTime);
                    break;

                case GameState.Controls: // controls menu
                    // loads the background
                    tiles.LoadStage(0);
                    // buttons
                    backButton.Update(gameTime);
                    break;

                case GameState.Victory: // if the player goes through all levels
                    // load background
                    tiles.LoadStage(0);
                    // buttons
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
                    // draw background
                    tiles.Draw(_spriteBatch);
                    // old title screen:
                    //_spriteBatch.Draw(titleScreen, new Vector2(0, 0), Color.White);
                    // title card: (centered)
                    Vector2 textSize = header.MeasureString("The Blood of Christ");
                    _spriteBatch.DrawString(header,
                                            "The Blood of Christ",
                                            new Vector2((windowWidth / 2) - (textSize.X / 2), 100),
                                            Color.Red);

                    //buttons
                    startButton.Draw(_spriteBatch);
                    controlsButton.Draw(_spriteBatch);
                    settingsButton.Draw(_spriteBatch);
                    quitButton.Draw(_spriteBatch);
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
                            "Press ESC to return to title screen\n" +
                            "Press WASD to move around\n" +
                            "Press E to toggle bat mode and fly. Watch the timer!\n" +
                            "Stay out of the light, it will burn you.\n" +
                            "The light will also trigger a trap, be ready to jump.",
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

                  


                    break;
                case GameState.Settings: // settings menu
                    // buttons
                    tiles.Draw(_spriteBatch);
                    backButton.Draw(_spriteBatch);
                    muteButton.Draw(_spriteBatch);
                    godModeButton.Draw(_spriteBatch);
                    break;

                case GameState.Controls: // controls menu
                    // background
                    tiles.Draw(_spriteBatch);
                    // buttons
                    backButton.Draw(_spriteBatch);
                    // instructions
                    _spriteBatch.DrawString(body,
                                            "Avoid the priest and fireballs to get out of the church! \n" +
                                            "Instructions: \n" +
                                            "WASD keys for movement\n" +
                                            "E for turning into a bat\n\n" +
                                            "CREDITS--- \n" +
                                            "Tile assets credit: https://blackspirestudio.itch.io/medieval-pixel-art-asset-free\n" +
                                            "Vampire, bat, light, and priest textures made by Sean Bethel\n" +
                                            "Music by Edward Choi\n" +
                                            "Sound Effect: https://freesound.org/people/leviclaassen/sounds/107788/\n" +
                                            "Fireball : https://www.freeiconspng.com/img/46732\n",
                                            new Vector2(150, 150),
                                            Color.White);
                    break;

                case GameState.GameOver: // ifplayer dies
                    // draw background
                    tiles.Draw(_spriteBatch);
                    // game over text (centered)
                    Vector2 textSize2 = header.MeasureString("You Died!");
                    _spriteBatch.DrawString(header, "You Died!", new Vector2((windowWidth/2) - (textSize2.X /2 ), 300), Color.DarkRed);
                    backButton.Draw(_spriteBatch);
                    break;
                case GameState.Victory: // if player beats all levels
                    // draw background
                    tiles.Draw(_spriteBatch);
                    // victory text (centered)
                    Vector2 textSize3 = header.MeasureString("You Win!");
                    _spriteBatch.DrawString(header, "You win!", new Vector2((windowWidth/2) - (textSize3.X / 2), 300), Color.DarkRed);
                    backButton.Draw(_spriteBatch);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        /// <summary>
        /// resets the game so it starts from the beginning if from title screen
        /// </summary>
        protected void StartGame()
        {
            // if coming from title screen, reset levels
            if(gs == GameState.Title)
            {
                player.Reset(150, 540);
                level = 1;
                tiles.LoadStage(level);
            }
            // if not, just return to game
            gs = GameState.Game;
        }
        /// <summary>
        /// sets the state to title screen 
        /// </summary>
        protected void TitleScreen()
        {
            gs = GameState.Title;
        }
        /// <summary>
        /// sets the state to the settings menu
        /// </summary>
        protected void SettingsMenu()
        {
            gs = GameState.Settings;
        }
        /// <summary>
        /// sets the state to controls menu
        /// </summary>
        protected void ControlsMenu()
        {
            gs = GameState.Controls;    
        }
        /// <summary>
        /// Toggles music when called
        /// </summary>
        protected void MuteMusic()
        {
            if (MediaPlayer.IsMuted)
                MediaPlayer.IsMuted = false;
            else
                MediaPlayer.IsMuted = true;
        }
        /// <summary>
        /// Disables damage to the player
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
