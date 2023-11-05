using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace PacMan
{
    internal class LoadTexAndPos
    {
        internal List<string> Mapper;
        internal StreamReader Sr;
        internal SpriteFont Font;

        internal Texture2D WallsTileTex;
        internal Texture2D EmptyTileTex;
        internal Texture2D PlayerTex;
        internal Texture2D PlayerTexTurned;
        internal Texture2D StandardPointTex;
        internal Texture2D TransTex;
        internal Texture2D EnemiesAndFruitTex;
        internal Texture2D PathTile;
        internal Texture2D EndScreen;
        internal Texture2D WinScreen;
        internal Texture2D StartMenu;
        internal Vector2 tilePos;
        internal Vector2 PlayerStartPos;
        internal Vector2 EnemyStartPos1;
        internal Vector2 EnemyStartPos2;

        internal Rectangle[,] WallTiles;
        internal Rectangle[] PlayerSprites;
        internal Rectangle[,] EnemyAndFruitSprites;

        internal int SpriteSizePlayer;
        internal int TileSize;
        public LoadTexAndPos(Game1 game)
        {
            WallsTileTex = game.Content.Load<Texture2D>("TilesetWalls");
            EmptyTileTex = game.Content.Load <Texture2D>("emptyTile");
            PlayerTex = game.Content.Load<Texture2D>("pacmansheet");
            PlayerTexTurned = game.Content.Load<Texture2D>("pacmansheet90");
            StandardPointTex = game.Content.Load<Texture2D>("StandardPoint");
            TransTex = game.Content.Load<Texture2D>("Transparent");
            EnemiesAndFruitTex = game.Content.Load<Texture2D>("SpriteSheetEnemiesAndFruits");
            PathTile = game.Content.Load<Texture2D>("PathTile");
            EndScreen = game.Content.Load<Texture2D>("LoseScreen-1.png");
            WinScreen = game.Content.Load<Texture2D>("WinScreen");
            StartMenu = WinScreen;

            Font = game.Content.Load<SpriteFont>("Font");

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
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[0, 0], new bool[] { false, false, false, false });
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
                        case '1':
                            game.TilesArray[i, j] = new Tiles(EmptyTileTex, tilePos, true, new bool[]{true, false, false, false});
                            break;
                        default:
                            game.TilesArray[i, j] = new Tiles(EmptyTileTex, tilePos, false, new bool[] { false, false, false, false });
                            break;
                    }
                }
            }

        }

        public void LoadMap2(Game1 game)
        {
            //Läser in textfilen i en lista
            Sr = new StreamReader("map2.txt");
            Mapper = new List<string>();

            while (!Sr.EndOfStream)
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

            for (int i = 0; i < game.TilesArray.GetLength(0); i++)
            {
                for (int j = 0; j < game.TilesArray.GetLength(1); j++)
                {
                    tilePos = new Vector2(TileSize * j * 2, TileSize * i * 2);

                    switch (Mapper[i][j])
                    {
                        case 'a':
                            game.TilesArray[i, j] = new Tiles(WallsTileTex, tilePos, WallTiles[0, 0], new bool[] { false, false, false, false });
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
                        case '1':
                            game.TilesArray[i, j] = new Tiles(EmptyTileTex, tilePos, true, new bool[] { true, false, false, false });
                            break;
                        default:
                            game.TilesArray[i, j] = new Tiles(EmptyTileTex, tilePos, false, new bool[] { false, false, false, false });
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

            PlayerStartPos = tilesArray[tilesArray.GetLength(0) - 2, tilesArray.GetLength(1) - 1].Pos;
        }

        //Laddar in spritsen för friender och frukt, samt start positionen för fiender;
        public void LoadEnemyAndFruitSpritesAndEnemyPos(Tiles[,] tilesArray)
        {
            EnemyAndFruitSprites = new Rectangle[6, 8];

            for(int i = 0; i < EnemyAndFruitSprites.GetLength(0); i++)
            {
                for(int j = 0; j < EnemyAndFruitSprites.GetLength(1); j++)
                {
                    EnemyAndFruitSprites[i, j] = new Rectangle(SpriteSizePlayer * j, SpriteSizePlayer * i, SpriteSizePlayer, SpriteSizePlayer);
                }
            }

            for(int i = 0; i < tilesArray.GetLength(0); i++)
            {
                for (int j = 0;j < tilesArray.GetLength(1);j++)
                {
                    if (tilesArray[i, j].Ghost)
                    {
                        EnemyStartPos1 = tilesArray[i, j].Pos;
                        EnemyStartPos2 = tilesArray[i, j + 1].Pos;
                        break;
                    }
                }
            }
        }

        //Laddar in poängen
        public void LoadPoints(Tiles[,] tileArray, PointManager pointsManager)
        {
            pointsManager.PointArray = new PointItems[tileArray.GetLength(0), tileArray.GetLength(1)];
            
            for(int i = 0; i < tileArray.GetLength(0); i++)
            {
                for(int j = 0; j < tileArray.GetLength(1); j++)
                {
                    if (tileArray[i, j].PointTile)
                    {
                        if ((i * 10 + j) % 12 == 0)
                        {
                            pointsManager.PointArray[i, j] = new SpecialPointItems(EnemiesAndFruitTex, tileArray[i, j].Pos, EnemyAndFruitSprites);
                        }
                        else
                        {
                            pointsManager.PointArray[i, j] = new StandardPoint(StandardPointTex, tileArray[i, j].Pos, false);
                        }
                    }
                    else
                    {
                        pointsManager.PointArray[i, j] = new StandardPoint(EmptyTileTex, tileArray[i,j].Pos, false);
                    }
                }
            }
        }
    }
}
