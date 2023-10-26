using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SharpDX.Direct3D11;
using SharpDX.MediaFoundation.DirectX;
using System.Diagnostics;

namespace PacMan
{
    internal class PlayerController
    {
        private Vector2 Direction;
        private Vector2 Destination;
        private Vector2 LastLineUpTile;

        private bool CanMove;
        private bool Wall;

        private int DirectionIndex;
        private int TileSize;
        private Tiles[,] TileArray;

        private float Speed;

        public PlayerController(int tileSize, Tiles[,] tileArray, float speed)
        {
            TileSize = tileSize;
            CanMove = true;
            Wall = false;
            Speed = speed;

            TileArray = tileArray;
            Direction = Vector2.Zero;
            DirectionIndex = 0;
        }
        public void DirectionUp(Vector2 position, GameTime gameTime)
        {

            Movement(new Vector2(0, -1), position);
            CanMove = false;
            DirectionIndex = 0;
        }
        public void DirectionDown(Vector2 position, GameTime gameTime)
        {
            Movement(new Vector2(0, 1), position);
            CanMove = false;
            DirectionIndex = 2;
        }
        public void DirectionLeft(Vector2 position, GameTime gameTime)
        {
            Movement(new Vector2(-1, 0), position);
            CanMove = false;
            DirectionIndex = 3;
        }
        public void DirectionRight(Vector2 position, GameTime gameTime)
        {
            Movement(new Vector2(1, 0), position);
            CanMove = false;
            DirectionIndex = 1;
        }
        public Vector2 KeepMoving(Vector2 position, GameTime gameTime)
        {
            Vector2 newPos = position;

            Debug.WriteLine(LastLineUpTile.ToString());

            for (int i = 0; i < TileArray.GetLength(0); i++)
            {
                for(int j = 0;  j < TileArray.GetLength(1); j++)
                {
                    if (-2 < TileArray[i, j].Pos.X - newPos.X && TileArray[i, j].Pos.X - newPos.X < 2 
                        && -2 < TileArray[i, j].Pos.Y - newPos.Y && TileArray[i,j].Pos.Y - newPos.Y < 2 && LastLineUpTile != newPos)
                    {
                        LastLineUpTile = TileArray[i, j].Pos;
                    }
                }
            }

            if (CurrentTile(LastLineUpTile)[DirectionIndex])
            {
                Movement(Direction, position);
                newPos += Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (Vector2.Distance(position, Destination) < 1)
            {
                newPos = Destination;
                CanMove = true;
            }


            return newPos;


        }
        public void Movement(Vector2 direction, Vector2 position)
        {
            if (CanMove)
            {
                Direction = direction;

                Vector2 newDestination = position + direction * TileSize * 2;

                Destination = newDestination;
            }
        }

        public Dictionary<int, bool> CurrentTile(Vector2 position)
        {
            if(position.X < TileSize)
            {
                position.X = TileSize;
            }
            return TileArray[(int)position.Y / (TileSize * 2), (int)position.X / (TileSize * 2)].AllowedDirections;
        }
    }
}
