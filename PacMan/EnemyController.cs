using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace PacMan
{
    internal class EnemyController
    {
        private Animation Animation;
        private Texture2D Tex;
        private int Index;
        private int SecondIndexer;
        private int IndexMultiplierFrame1;
        private int IndexMultiplierFrame2;
        private int DirectionIndex;
        private int TileSize;
        private float Speed;
        private Vector2 Destination;
        private Vector2 Direction;
        private Vector2 LastLineUpTile;
        private Tiles[,] TileArray;
        public EnemyController(Animation animation, int indexer, Texture2D tex, int tileSize, float speed, Tiles[,] tileArray)
        {
            Animation = animation;
            Tex = tex;
            DirectionIndex = 1;
            TileSize = tileSize;
            Speed = speed;
            TileArray = tileArray;
        }

        //Vänder uppåt
        public void DirectionUp(Vector2 position, GameTime gameTime)
        {

            Movement(new Vector2(0, -1), position);
            DirectionIndex = 0;
        }
        //Vänder neråt
        public void DirectionDown(Vector2 position, GameTime gameTime)
        {

            Movement(new Vector2(0, 1), position);
            DirectionIndex = 2;
        }
        //Vänder åt vänster
        public void DirectionLeft(Vector2 position, GameTime gameTime)
        {

            Movement(new Vector2(-1, 0), position);
            DirectionIndex = 3;
        }

        //Vänder åt höger
        public void DirectionRight(Vector2 position, GameTime gameTime)
        {
            Movement(new Vector2(1, 0), position);
            DirectionIndex = 1;
        }

        //Upprepar fienders rörelse
        public Vector2 KeepMoving(Vector2 position, Vector2 playerPos, GameTime gameTime)
        {
            Vector2 newPos = position;

            //Kollar att där inte är en vägg i vägen
            if (CurrentTile(LastLineUpTile)[DirectionIndex])
            {
                Movement(Direction, position);
                newPos += Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            //Kollar att fienden är upplinad med en ruta
            for (int i = 0; i < TileArray.GetLength(0); i++)
            {
                for (int j = 0; j < TileArray.GetLength(1); j++)
                {
                    if (Vector2.Distance(newPos, TileArray[i, j].Pos) < 2 && LastLineUpTile != newPos)
                    {
                        newPos = TileArray[i, j].Pos;
                        LastLineUpTile = TileArray[i, j].Pos;
                        CheckAvailablePaths(TileArray[i, j].Pos, playerPos, gameTime);
                    }
                }
            }


            return newPos;


        }

        //Sätter nästa destination för spelaren
        public void Movement(Vector2 direction, Vector2 position)
        {

            Direction = direction;

            Vector2 newDestination = position + direction * TileSize * 2;

            Destination = newDestination;

        }

        //Kollar om man får lov att röra sig i riktningen man vill
        public Dictionary<int, bool> CurrentTile(Vector2 position)
        {
            if (position.X < TileSize)
            {
                position.X = TileSize;
            }
            return TileArray[(int)position.Y / (TileSize * 2), (int)position.X / (TileSize * 2)].AllowedDirections;
        }

        public void CheckAvailablePaths(Vector2 pos, Vector2 playerPos, GameTime gameTime)
        {
            if (playerPos.Y > pos.Y && playerPos.X > pos.X)
            {
                if (CurrentTile(pos)[0])
                {
                    DirectionUp(pos, gameTime);
                }
                else if (CurrentTile(pos)[1])
                {
                    DirectionRight(pos, gameTime);
                }
                else if (CurrentTile(pos)[2])
                {
                    DirectionDown(pos, gameTime);
                }
                else if (CurrentTile(pos)[3])
                {
                    DirectionLeft(pos, gameTime);
                }
            }
            else if (playerPos.Y > pos.Y && playerPos.X < pos.X)
            {
                if (CurrentTile(pos)[0])
                {
                    DirectionUp(pos, gameTime);
                }
                else if (CurrentTile(pos)[3])
                {
                    DirectionLeft(pos, gameTime);
                }
                else if (CurrentTile(pos)[1])
                {
                    DirectionRight(pos, gameTime);
                }
                else if (CurrentTile(pos)[2])
                {
                    DirectionDown(pos, gameTime);
                }
            }
            else if (playerPos.Y < pos.Y && playerPos.X < pos.X)
            {
                if (CurrentTile(pos)[2])
                {
                    DirectionDown(pos, gameTime);
                }
                else if (CurrentTile(pos)[3])
                {
                    DirectionLeft(pos, gameTime);
                }
                else if (CurrentTile(pos)[1])
                {
                    DirectionRight(pos, gameTime);
                }
                else if (CurrentTile(pos)[0])
                {
                    DirectionUp(pos, gameTime);
                }
            }
            else if (playerPos.Y < pos.Y && playerPos.X > pos.X)
            {
                if (CurrentTile(pos)[2])
                {
                    DirectionDown(pos, gameTime);
                }
                else if (CurrentTile(pos)[1])
                {
                    DirectionRight(pos, gameTime);
                }
                else if (CurrentTile(pos)[3])
                {
                    DirectionLeft(pos, gameTime);
                }
                else if (CurrentTile(pos)[2])
                {
                    DirectionDown(pos, gameTime);
                }
            }
        }
        //Ritar fiendens animation
        public void DrawMovement(Vector2 Pos, GameTime gameTime, SpriteBatch sb)
        {
            switch(DirectionIndex)
            {
                case 0:
                    IndexMultiplierFrame1 = 4;
                    IndexMultiplierFrame2 = 5;
                    break;
                case 1:
                    IndexMultiplierFrame1 = 0;
                    IndexMultiplierFrame2 = 1;
                    break;
                case 2:
                    IndexMultiplierFrame1 = 6;
                    IndexMultiplierFrame2 = 7;
                    break;
                case 3:
                    IndexMultiplierFrame1 = 2;
                    IndexMultiplierFrame2 = 3;
                    break;
            }

            SecondIndexer = Animation.RunAnimation(gameTime);
            
            if(SecondIndexer == 0)
            {
                sb.Draw(Tex, Pos, Animation.Rects2[Index, (SecondIndexer + 1) * IndexMultiplierFrame1], Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 1f);
            }
            else
            {
                sb.Draw(Tex, Pos, Animation.Rects2[Index, SecondIndexer * IndexMultiplierFrame2], Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 1f);
            }
        }
    }
}
