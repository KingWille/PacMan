using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace PacMan
{
    internal class PlayerController
    {
        private Vector2 Direction;
        private Vector2 Destination;
        private Vector2 LastLineUpTile;

        internal bool CanMove;
        internal bool CanTurn;
        private bool Wall;

        private int DirectionIndex;
        private int TileSize;
        private Tiles[,] TileArray;
        private Animation playerAnimation;
        private SpriteEffects SE;
        private Texture2D CurrentTex;
        internal InputHandler inputHandler;

        private float Speed;

        public PlayerController(int tileSize, Tiles[,] tileArray, float speed, Animation animation)
        {
            TileSize = tileSize;
            CanMove = true;
            CanTurn = true;
            Wall = false;
            Speed = speed;

            playerAnimation = animation;
            inputHandler = new InputHandler();
            TileArray = tileArray;
            Direction = Vector2.Zero;
            DirectionIndex = 0;
        }
        //Vänder uppåt
        public void DirectionUp(Vector2 position, GameTime gameTime)
        {
            if (CanTurn)
            {
                Movement(new Vector2(0, -1), position);
                CanMove = false;
                CanTurn = false;
                DirectionIndex = 0;
            }
            
        }
        //Vänder neråt
        public void DirectionDown(Vector2 position, GameTime gameTime)
        {
            if (CanTurn)
            {
                Movement(new Vector2(0, 1), position);
                CanMove = false;
                CanTurn = false;
                DirectionIndex = 2;
            }
            
        }
        //Vänder åt vänster
        public void DirectionLeft(Vector2 position, GameTime gameTime)
        {
            if (CanTurn)
            {
                Movement(new Vector2(-1, 0), position);
                CanMove = false;
                CanTurn = false;
                DirectionIndex = 3;
            }
            
        }

        //Vänder åt höger
        public void DirectionRight(Vector2 position, GameTime gameTime)
        {
            if (CanTurn)
            {

                Movement(new Vector2(1, 0), position);
                CanMove = false;
                CanTurn = false;
                DirectionIndex = 1;
            }
            
            
        }

        //Upprepar pacmans rörelse när man inte bytar riktning
        public Vector2 KeepMoving(Vector2 position, GameTime gameTime)
        {
            Vector2 newPos = position;

            //Kollar att där inte är en vägg i vägen
            if (CurrentTile(LastLineUpTile)[DirectionIndex])
            {
                Movement(Direction, position);
                newPos += Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            //Kollar att man är upplinad med en ruta
            for (int i = 0; i < TileArray.GetLength(0); i++)
            {
                for (int j = 0; j < TileArray.GetLength(1); j++)
                {
                    if (Vector2.Distance(newPos, TileArray[i, j].Pos) < 1 && LastLineUpTile != newPos)
                    {
                        newPos = TileArray[i, j].Pos;
                        LastLineUpTile = TileArray[i, j].Pos;
                        CanMove = true;
                    }
                }
            }

            //Bytar riktning när man är upplinad samt har ändrat riktning tidigare
            if (CurrentTile(LastLineUpTile)[inputHandler.LastTurn(DirectionIndex)] && !CanTurn)
            {
                switch (inputHandler.LastTurn(DirectionIndex))
                {
                    case 0:
                        CanTurn = true;
                        DirectionUp(newPos, gameTime);
                        break;
                    case 1:
                        CanTurn = true;
                        DirectionRight(newPos, gameTime);
                        break;
                    case 2:
                        CanTurn = true;
                        DirectionDown(newPos, gameTime);
                        break;
                    case 3:
                        CanTurn = true;
                        DirectionLeft(newPos, gameTime);
                        break;
                }
            }


            return newPos;


        }
        //Sätter nästa destination för spelaren
        public void Movement(Vector2 direction, Vector2 position)
        {
            if (CanMove)
            {
                Direction = direction;

                Vector2 newDestination = position + direction * TileSize * 2;

                Destination = newDestination;
            }
        }

        //Kollar om man får lov att röra sig i riktningen man vill
        public Dictionary<int, bool> CurrentTile(Vector2 position)
        {
            if(position.X < TileSize)
            {
                position.X = TileSize;
            }
            return TileArray[(int)position.Y / (TileSize * 2), (int)position.X / (TileSize * 2)].AllowedDirections;
        }

        public void DrawMovement(Vector2 Pos, GameTime gameTime, SpriteBatch sb) 
        {
            switch(DirectionIndex)
            {
                case 0:
                    CurrentTex = playerAnimation.SpriteSheetTurned;
                    SE = SpriteEffects.None;
                    break;
                case 1:
                    CurrentTex = playerAnimation.SpriteSheet;
                    SE = SpriteEffects.None;
                    break;
                case 2:
                    CurrentTex = playerAnimation.SpriteSheetTurned;
                    SE = SpriteEffects.FlipVertically;
                    break;
                case 3:
                    CurrentTex = playerAnimation.SpriteSheet;
                    SE = SpriteEffects.FlipHorizontally;
                    break;
            }

            sb.Draw(CurrentTex, Pos, playerAnimation.Rects[playerAnimation.RunAnimation(gameTime)], Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SE, 1f);
        }
    }
}
