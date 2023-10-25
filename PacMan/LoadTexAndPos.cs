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

        internal Vector2 tilePos;

        internal Rectangle[,] WallTiles;

        internal int tileSize;
        public LoadTexAndPos(Game1 game)
        {
            WallsTileTex = game.Content.Load<Texture2D>("TilesetWalls");
            EmptyTileTex = game.Content.Load <Texture2D>("emptyTile");

            tileSize = 32;
            WallTiles = new Rectangle[4, 4];

            for(int i = 0; i < WallTiles.GetLength(0); i++)
            {
                for(int j = 0;  j < WallTiles.GetLength(1); j++)
                {
                    WallTiles[i, j] = new Rectangle(tileSize * j, tileSize * i, tileSize, tileSize);
                }
            }
        }

        //Laddar in kartan
        public void LoadMap(Game1 game)
        {
            Sr = new StreamReader("map.txt");
            Mapper = new List<string>();

            while(!Sr.EndOfStream)
            {
                Mapper.Add(Sr.ReadLine());
            }

            Sr.Close();

            game.TilesArray = new Tiles[Mapper.Count(), Mapper[0].Length];

            for(int i = 0; i < game.TilesArray.GetLength(0); i++)
            {
                for(int j = 0; j < game.TilesArray.GetLength(1); j++)
                {
                    tilePos = new Vector2(tileSize * j * 2, tileSize * i * 2);

                    switch (Mapper[i][j])
                    {
                        case 'a':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[0,0]);
                            break;
                        case 'b':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[0, 1]);
                            break;
                        case 'c':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[0, 2]);
                            break;
                        case 'd':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[0, 3]);
                            break;
                        case 'e':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[1, 0]);
                            break;
                        case 'f':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[1, 1]);
                            break;
                        case 'g':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[1, 2]);
                            break;
                        case 'h':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[1, 3]);
                            break;
                        case 'i':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[2, 0]);
                            break;
                        case 'j':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[2, 1]);
                            break;
                        case 'k':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[2, 2]);
                            break;
                        case 'l':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[2, 3]);
                            break;
                        case 'm':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[3, 0]);
                            break;
                        case 'n':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[3, 1]);
                            break;
                        case 'o':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[3, 2]);
                            break;
                        case 'p':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[3, 3]);
                            break;
                        default:
                            game.TilesArray[i, j] = new Tiles(EmptyTileTex, tilePos, true);
                            break;
                    }
                }
            }

        }
    }
}
