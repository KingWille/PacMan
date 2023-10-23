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

        internal Tiles[,] TilesArray;
        internal Texture2D WallTex;

        internal Vector2 tilePos;

        internal int tileSize;
        public LoadTexAndPos(Game1 game)
        {
            WallTex = game.Content.Load<Texture2D>("wall");
        }

        //Laddar in kartan
        public void LoadMap()
        {
            Sr = new StreamReader("map.txt");
            Mapper = new List<string>();
            tileSize = 25;

            while(!Sr.EndOfStream)
            {
                Mapper.Add(Sr.ReadLine());
            }

            Sr.Close();

            TilesArray = new Tiles[Mapper.Count(), Mapper[0].Length];

            for(int i = 0; i < TilesArray.GetLength(0); i++)
            {
                for(int j = 0; i < TilesArray.GetLength(1); j++)
                {
                    tilePos = new Vector2(tileSize * j, tileSize * i);

                    switch (Mapper[i][j])
                    {
                        case '#':
                            TilesArray[i, j] = new Tiles(WallTex, tilePos, true);
                            break;
                    }
                }
            }

        }
    }
}
