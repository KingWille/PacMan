using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    internal class EnemyControllerRandom : Controllers
    {
        private Random rnd;
        public EnemyControllerRandom(Animation animation, Texture2D tex, Tiles[,] tileArray, float speed, int tileSize)
        {
            TileArray = tileArray;
            Speed = speed;
            DirectionIndex = 0;
            Index = 1;
            Tex = tex;

            rnd = new Random();
            TileSize = tileSize;
            Animation = animation;
        }
        public override Vector2 KeepMoving(Vector2 position, Vector2 playerPos, GameTime gameTime, bool released, bool ghost, bool eaten)
        {
            Ghost = ghost;
            Eaten = eaten;
            Vector2 newPos = position;
            if (released)
            {

                //sätter fiendens lineup
                for (int i = 0; i < TileArray.GetLength(0); i++)
                {
                    for (int j = 0; j < TileArray.GetLength(1); j++)
                    {
                        if (Vector2.Distance(position, TileArray[i, j].Pos) < 5)
                        {
                            LastLinedUpTile = TileArray[i, j].Pos;

                            switch (rnd.Next(0, 4))
                            {
                                case 0:
                                    Movement(new Vector2(0, -1), position);
                                    DirectionIndex = 0;
                                    break;
                                case 1:
                                    Movement(new Vector2(1, 0), position);
                                    DirectionIndex = 1;
                                    break;
                                case 2:
                                    Movement(new Vector2(0, 1), position);
                                    DirectionIndex = 2;
                                    break;
                                case 3:
                                    Movement(new Vector2(-1, 0), position);
                                    DirectionIndex = 3;
                                    break;
                            }
                            break;
                        }
                    }
                }

                //Sätter positionen till lika med nästa tile
                if (Vector2.Distance(position, Destination) < 3)
                {
                    newPos = Destination;

                }

                //Kollar att man inte försöker gå igenom en vägg
                if (CurrentTile(LastLinedUpTile)[DirectionIndex])
                {
                    newPos += Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
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

        public override void DrawMovement(Vector2 Pos, GameTime gameTime, SpriteBatch sb)
        {
            if (!Ghost && !Eaten)
            {
                switch (DirectionIndex)
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
                Index = 1;

                if (SecondIndexer == 0)
                {
                    sb.Draw(Tex, Pos, Animation.Rects2[Index, (SecondIndexer + 1) * IndexMultiplierFrame1], Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 1f);
                }
                else
                {
                    sb.Draw(Tex, Pos, Animation.Rects2[Index, SecondIndexer * IndexMultiplierFrame2], Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 1f);
                }
            }
            else if(!Eaten)
            {
                Index = 4;
                SecondIndexer = Animation.RunAnimation(gameTime);

                sb.Draw(Tex, Pos, Animation.Rects2[Index, SecondIndexer + 2], Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 1f);
            }
            else
            {
                Index = 4;
                sb.Draw(Tex, Pos, Animation.Rects2[Index, 7], Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 1f);
            }
        }

    }
}
