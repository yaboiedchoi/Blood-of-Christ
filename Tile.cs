using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blood_of_Christ
{
    /// <summary>
    /// Tiling system for the platforms and collectibles
    /// </summary>
    internal class Tile
    {
        // Fields
        private Texture2D tex_tiles;
        private Texture2D tex_key;
        private Texture2D tex_goal;
        private Player player;
        private Platform[,] windowTiles;
        private Platform[,] platformTiles;

        // Rectangles that specify the position of the tile in sprite sheet
        private Rectangle top = new Rectangle(64, 64, 48, 48);
        private Rectangle bottom = new Rectangle(64, 256, 48, 48);
        private Rectangle left = new Rectangle(16, 160, 48, 48);
        private Rectangle right = new Rectangle(592, 160, 48, 48);
        private Rectangle topleft = new Rectangle(16, 64, 48, 48);
        private Rectangle bottomleft = new Rectangle(16, 256, 48, 48);
        private Rectangle topright = new Rectangle(592, 64, 48, 48);
        private Rectangle bottomright = new Rectangle(592, 256, 48, 48);
        private Rectangle background = new Rectangle(160, 640, 48, 48);
        private Rectangle gutter = new Rectangle(16, 16, 48, 48);

        // Collectibles
        Dictionary<Key, List<Door>> keyDoorPairs;
        private Key key1;
        private Key key2;
        private List<Goal> goal;
        private List<Door> doorsA;
        private List<Door> doorsB;

        public List<Goal> Goal
        {
            get { return goal; }
        }

        // Constructor
        public Tile(Texture2D tex_tiles, Texture2D tex_key, Texture2D tex_goal, Player player)
        {
            this.tex_tiles = tex_tiles;
            this.tex_key = tex_key;
            this.tex_goal = tex_goal;
            this.player = player;
            windowTiles = new Platform[15, 25];
            platformTiles = new Platform[11, 19];

            keyDoorPairs = new Dictionary<Key, List<Door>>();
            doorsA = new List<Door>();
            doorsB = new List<Door>();
            goal = new List<Goal>();
        }

        // Methods

        /// <summary>
        /// Call Player's Physics method for every platform tiles
        /// and check Player's collision with collectibles
        /// </summary>
        /// <param name="gameTime">The current snapshot of the game time</param>
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < windowTiles.GetLength(0); i++)
            {
                for (int j = 0; j < windowTiles.GetLength(1); j++)
                {
                    player.Physics(windowTiles[i, j].Position);
                }
            }

            for (int i = 0; i < platformTiles.GetLength(0); i++)
            {
                for (int j = 0; j < platformTiles.GetLength(1); j++)
                {
                    player.Physics(platformTiles[i, j].Position);
                }
            }

            foreach (KeyValuePair<Key, List<Door>> entry in keyDoorPairs)
            {
                foreach (Door door in entry.Value)
                {
                    player.Physics(door.Position);
                }
                if (entry.Key.CheckCollision(player))
                {
                    keyDoorPairs.Remove(entry.Key);
                }
            }
        }

        /// <summary>
        /// Draw the stage and keys
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                tex_tiles,
                new Rectangle(0, 0, 1200, 720),
                background,
                Color.White);


            for (int i = 0; i < windowTiles.GetLength(0); i++)
            {
                for (int j = 0; j < windowTiles.GetLength(1); j++)
                {
                    windowTiles[i, j].Draw(sb);
                }
            }

            for (int i = 0; i < platformTiles.GetLength(0); i++)
            {
                for (int j = 0; j < platformTiles.GetLength(1); j++)
                {
                    platformTiles[i, j].Draw(sb);
                }
            }

            foreach (KeyValuePair<Key, List<Door>> entry in keyDoorPairs)
            {
                foreach (Door door in entry.Value)
                {
                    door.Draw(sb);
                }
                entry.Key.Draw(sb);
            }

            for (int i = 0; i< goal.Count; i++) 
            {
                goal[i].Draw(sb);
            }
        }

        #region WindowTiles
        /// <summary>
        /// Generate the window tiles that prevent the Player from going out of screen
        /// </summary>
        public void WindowTiles()
        {
            for (int r = 0; r < windowTiles.GetLength(0); r++)
            {
                for (int c = 0; c < windowTiles.GetLength(1); c++)
                {
                    // State where to have platforms
                    if (r <= 1 || r >= windowTiles.GetLength(0) - 2 || c <= 2 || c >= windowTiles.GetLength(1) - 3)
                    {
                        windowTiles[r, c] = new Platform(tex_tiles, new Rectangle(c * 48, r * 48, 48, 48));
                    }
                    else
                    {
                        windowTiles[r, c] = new Platform(tex_tiles, new Rectangle());
                    }

                    // State the type of sprite to use
                    if (r == 0 || r == windowTiles.GetLength(0) - 1 || c <= 1 || c >= windowTiles.GetLength(1) - 2)
                    {
                        windowTiles[r, c].SpritePosition = gutter;
                    }
                    else if (r == 1 && c == 2)
                    {
                        windowTiles[r, c].SpritePosition = topleft;
                    }
                    else if (r == 1 && c == windowTiles.GetLength(1) - 3)
                    {
                        windowTiles[r, c].SpritePosition = topright;
                    }
                    else if (r == windowTiles.GetLength(0) - 2 && c == 2)
                    {
                        windowTiles[r, c].SpritePosition = bottomleft;
                    }
                    else if (r == windowTiles.GetLength(0) - 2 && c == windowTiles.GetLength(1) - 3)
                    {
                        windowTiles[r, c].SpritePosition = bottomright;
                    }
                    else if (r == 1)
                    {
                        windowTiles[r, c].SpritePosition = top;
                    }
                    else if (r == windowTiles.GetLength(0) - 2)
                    {
                        windowTiles[r, c].SpritePosition = bottom;
                    }
                    else if (c == 2)
                    {
                        windowTiles[r, c].SpritePosition = left;
                    }
                    else if (c == windowTiles.GetLength(1) - 3)
                    {
                        windowTiles[r, c].SpritePosition = right;
                    }
                    else
                    {
                        windowTiles[r, c].SpritePosition = background;
                    }
                }
            }
        }
        #endregion

        #region LoadStage
        /// <summary>
        /// Load stage from a text file
        /// </summary>
        public void LoadStage()
        {
            StreamReader reader = null;
            try
            {
                reader = new StreamReader("../../../Stage1.txt");

                string line = "";

                int r = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    List<string> tilesData = new List<string>();
                    foreach (char ch in line)
                    {
                        tilesData.Add(ch.ToString());
                    }

                    // Determine what type of tile the character is,
                    // and initialize the appropriate object.
                    // Also state the sprite
                    for (int c = 0; c < tilesData.Count; c++)
                    {
                        if (tilesData[c] == ".")
                        {
                            platformTiles[r, c] = new Platform(tex_tiles, new Rectangle());
                        }
                        else if (tilesData[c] == "P")
                        {
                            platformTiles[r, c] = new Platform(tex_tiles, new Rectangle(144 + c * 48, 96 + r * 48, 48, 48));
                            platformTiles[r, c].SpritePosition = bottom;
                        }
                        else if (tilesData[c] == "1")
                        {
                            platformTiles[r, c] = new Platform(tex_tiles, new Rectangle());
                            key1 = new Key(tex_key, new Rectangle(144 + c * 48, 96 + r * 48, 48, 48));
                        }
                        else if (tilesData[c] == "2")
                        {
                            platformTiles[r, c] = new Platform(tex_tiles, new Rectangle());
                            key2 = new Key(tex_key, new Rectangle(144 + c * 48, 96 + r * 48, 48, 48));
                        }
                        else if (tilesData[c] == "Q")
                        {
                            platformTiles[r, c] = new Platform(tex_tiles, new Rectangle());
                            doorsA.Add(new Door(tex_tiles, new Rectangle(144 + c * 48, 96 + r * 48, 48, 48)));
                        }
                        else if (tilesData[c] == "W")
                        {
                            platformTiles[r, c] = new Platform(tex_tiles, new Rectangle());
                            doorsB.Add(new Door(tex_tiles, new Rectangle(144 + c * 48, 96 + r * 48, 48, 48)));
                        }
                        else if (tilesData[c] == "G")
                        {
                            platformTiles[r, c] = new Platform(tex_tiles, new Rectangle());
                            goal.Add(new Goal(new Rectangle(144 + c * 48, 96 + r * 48, 48, 48), tex_goal));
                        }
                    }
                    r++;
                }

                // Assign doors and keys into the dictionary
                if (key1 != null && doorsA.Any())
                {
                    keyDoorPairs.Add(key1, doorsA);
                }
                if (key2 != null && doorsB.Any())
                {
                    keyDoorPairs.Add(key2, doorsB);
                }
            }
            catch (Exception fileError)
            {
                System.Diagnostics.Debug.WriteLine(fileError.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
        #endregion
    }
}
