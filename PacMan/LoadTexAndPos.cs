using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PacMan
{
    internal class LoadTexAndPos
    {
        internal List<string> Mapper;
        internal StreamReader Sr;

        internal Texture2D WallsTileTex;
        internal Texture2D EmptyTileTex;
        internal Texture2D PlayerTex;

        internal Vector2 tilePos;
        internal Vector2 PlayerStartPos;

        internal Rectangle[,] WallTiles;
        internal Rectangle[] PlayerSprites;

        internal int SpriteSizePlayer;
        internal int TileSize;
        public LoadTexAndPos(Game1 game)
        {
            WallsTileTex = game.Content.Load<Texture2D>("TilesetWalls");
            EmptyTileTex = game.Content.Load <Texture2D>("emptyTile");
            PlayerTex = game.Content.Load<Texture2D>("pacmansheet");

            TileSize = 32;
            SpriteSizePlayer = 64;

        }

        //Laddar in kartan
        public void LoadMap(Game1 game)
        {
            //Läser in textfilen i en lista
            Sr = new StreamReader("map.txt");
            Mapper = new List<string>();

            while(!Sr.EndOfStream)
            {
                Mapper.Add(Sr.ReadLine());
            }

            Sr.Close();

            //Lägger till rectangel positioner från spritesheetet
            WallTiles = new Rectangle[4, 4];

            for (int i = 0; i < WallTiles.GetLength(0); i++)
            {
                for (int j = 0; j < WallTiles.GetLength(1); j++)
                {
                    WallTiles[i, j] = new Rectangle(TileSize * j, TileSize * i, TileSize, TileSize);
                }
            }

            //Läser in rätt tile från rätt bokstav
            game.TilesArray = new Tiles[Mapper.Count(), Mapper[0].Length];

            for(int i = 0; i < game.TilesArray.GetLength(0); i++)
            {
                for(int j = 0; j < game.TilesArray.GetLength(1); j++)
                {
                    tilePos = new Vector2(TileSize * j * 2, TileSize * i * 2);

                    switch (Mapper[i][j])
                    {
                        case 'a':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[0,0], new bool[] {false, false, false, false});
                            break;
                        case 'b':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[0, 1], new bool[] { false, true, false, false });
                            break;
                        case 'c':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[0, 2], new bool[] { true, false, false, false });
                            break;
                        case 'd':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[0, 3], new bool[] { true, true, false, false });
                            break;
                        case 'e':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[1, 0], new bool[] { false, false, false, true });
                            break;
                        case 'f':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[1, 1], new bool[] { false, true, false, true });
                            break;
                        case 'g':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[1, 2], new bool[] { true, false, false, true });
                            break;
                        case 'h':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[1, 3], new bool[] { true, true, false, true });
                            break;
                        case 'i':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[2, 0], new bool[] { false, false, true, false });
                            break;
                        case 'j':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[2, 1], new bool[] { false, true, true, false });
                            break;
                        case 'k':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[2, 2], new bool[] { true, false, true, false });
                            break;
                        case 'l':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[2, 3], new bool[] { true, true, true, false });
                            break;
                        case 'm':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[3, 0], new bool[] { false, false, true, true });
                            break;
                        case 'n':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[3, 1], new bool[] { false, true, true, true });
                            break;
                        case 'o':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[3, 2], new bool[] { true, false, true, true });
                            break;
                        case 'p':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[3, 3], new bool[] { true, true, true, true });
                            break;
                        default:
                            game.TilesArray[i, j] = new Tiles(EmptyTileTex, tilePos, true);
                            break;
                    }
                }
            }

        }

        //Laddar in spritsen för pacman
        public void LoadPlayerSpritesAndPos(Tiles[,] tilesArray)
        {
            PlayerSprites = new Rectangle[4];

            for (int i = 0; i < PlayerSprites.Length; i++)
            {
                PlayerSprites[i] = new Rectangle(SpriteSizePlayer * i, 0, SpriteSizePlayer, SpriteSizePlayer);
            }

            PlayerStartPos = tilesArray[tilesArray.GetLength(0) - 1, tilesArray.GetLength(1) - 1].Pos;
        }
    }
}
