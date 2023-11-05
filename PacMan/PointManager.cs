using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace PacMan
{
    internal class PointManager
    {
        internal PointItems[,] PointArray;
        internal int Points;
        internal bool Invurnability;

        private SpriteFont Font;
        private int PointCounter;
        private int Minutes;
        private int Lives;
        private float InvurnabilityTimer;
        private float Seconds;
        private float GhostTimer;
        private Texture2D TransTex;
        public PointManager(SpriteFont font, Texture2D transparent) 
        {
            Font = font;
            TransTex = transparent;
            Minutes = 4;
            Seconds = 60;
            Lives = 3;
            Invurnability = false;
            InvurnabilityTimer = 3;
            PointCounter = 0;
            GhostTimer = 10;
        }
        
        //Uppdateringsmetod för poängen
        public void Update(Vector2 playerPos, Game1 game, GameTime gameTime)
        {
            PointItemRemoval(playerPos, game);
            GameTimer(gameTime, game);

            foreach(Enemy e in game.Enemies)
            {
                LivesManager(game.player.Rect, e.Rect, game);
            }

            Debug.WriteLine(game.Enemies[0].Ghost);

        }
        //Rit metod för poängen
        public void Draw(SpriteBatch sb)
        {
            DrawPointsAndTime(sb);

            for (int i = 0; i < PointArray.GetLength(0); i++)
            {
                for (int j = 0; j < PointArray.GetLength(1); j++)
                {
                    PointArray[i, j].Draw(sb);
                }
            }
        }
        //Tar bort föremålet som get poäng när man har tagit det
        public void PointItemRemoval(Vector2 pos, Game1 game)
        {
            for(int i = 0; i < PointArray.GetLength(0); i++)
            {
                for(int j = 0; j < PointArray.GetLength(1); j++)
                {
                    if (PointArray[i, j].Pos == pos && !PointArray[i, j].Taken)
                    {
                        if (!PointArray[i, j].special)
                        {
                            PointArray[i, j].Tex = TransTex;
                            PointArray[i, j].Taken = true;
                            Points += PointArray[i, j].PointValue;
                            PointCounter++;

                            game.loseScreen.UpdatePoints(Points);
                            game.winScreen.UpdatePoints(Points);
                        }
                        else
                        {
                            PointArray[i, j].Tex = TransTex;
                            PointArray[i, j].Taken = true;
                            (PointArray[i, j] as SpecialPointItems).Used = true;
                            Points += PointArray[i, j].PointValue;
                            PointCounter++;
                            
                            (PointArray[i, j] as SpecialPointItems).PowerUp(game.Enemies);

                            game.loseScreen.UpdatePoints(Points);
                            game.winScreen.UpdatePoints(Points);
                        }
                    }
                }
            }

            //Kollar om man äter ett spöke
            foreach(Enemy e in game.Enemies)
            {
                if(e.Ghost && game.player.Rect.Intersects(e.Rect) && !e.Eaten)
                {
                    e.Eaten = true;
                    Points += 500;
                }
                
            }

            if (PointCounter == PointArray.GetLength(0) * PointArray.GetLength(1) - 24)
            {
                game.state = Game1.GameState.win;
            }
        }
        //Ritar ut poängen på botten av skärmen
        public void DrawPointsAndTime(SpriteBatch sb)
        {
            sb.DrawString(Font, $"Points: {Points} Time: {Minutes}:{(int)Seconds} \n Lives: {Lives}", PointArray[PointArray.GetLength(0) - 1, 0].Pos, Color.White, 0f, Vector2.Zero, new Vector2(2,2), SpriteEffects.None, 1f);
        }

        //Timer för spelet
        public void GameTimer(GameTime gameTime, Game1 game)
        {
            //Hanterar tiden
            Seconds -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            Debug.WriteLine(gameTime.ElapsedGameTime.TotalSeconds);

            if(Seconds < 0)
            {
                if (Minutes <= 0)
                {
                    game.state = Game1.GameState.lose;
                }
                else
                {
                    Seconds = 60;
                    Minutes--;
                }
            }

            //Släpper ut fienderna efter 10 sekunder
            if(Minutes == 4 && Seconds <= 58)
            {
                foreach(Enemy e in game.Enemies)
                {
                    e.Gates = true;
                }
            }

            //Hanterar när invurnability ska ta slut
            if (Invurnability)
            {
                InvurnabilityTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if(InvurnabilityTimer <= 0)
            {
                InvurnabilityTimer = 3;
                Invurnability = false;
            }

            if (game.Enemies[0].Ghost)
            {
                GhostTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if(GhostTimer <= 0)
                {
                    GhostTimer = 10;

                    foreach(Enemy e in game.Enemies)
                    {

                        e.Ghost = false;
                    }
                }
            }
        }
        public void LivesManager(Rectangle player, Rectangle enemy, Game1 game)
        {
            //Hanterar när man förlorar liv
            if(player.Intersects(enemy) && !Invurnability && !game.Enemies[0].Eaten)
            {
                Lives--;
                Invurnability = true;
            }

            if (Lives == 0)
            {
                game.state = Game1.GameState.lose;
            }
        }
    }
}
