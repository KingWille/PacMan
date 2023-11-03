using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace PacMan
{
    internal class PointManager
    {
        internal StandardPoint[,] StandardPointsArray;
        private SpriteFont Font;
        private int Points;
        private int Minutes;
        private float Seconds;
        private Texture2D TransTex;
        public PointManager(SpriteFont font, Texture2D transparent) 
        {
            Font = font;
            TransTex = transparent;
            Minutes = 4;
            Seconds = 60;
        }

        //Uppdateringsmetod för poängen
        public void Update(Vector2 playerPos, Game1 game, GameTime gameTime)
        {
            PointItemRemoval(playerPos);
            GameTimer(gameTime, game);
        }
        //Rit metod för poängen
        public void Draw(SpriteBatch sb)
        {
            DrawPointsAndTime(sb);

            for (int i = 0; i < StandardPointsArray.GetLength(0); i++)
            {
                for (int j = 0; j < StandardPointsArray.GetLength(1); j++)
                {
                    StandardPointsArray[i, j].Draw(sb);
                }
            }
        }
        //Tar bort föremålet som get poäng när man har tagit det
        public void PointItemRemoval(Vector2 pos)
        {
            for(int i = 0; i < StandardPointsArray.GetLength(0); i++)
            {
                for(int j = 0; j < StandardPointsArray.GetLength(1); j++)
                {
                    if (StandardPointsArray[i, j].Pos == pos && !StandardPointsArray[i, j].Taken)
                    {
                        StandardPointsArray[i, j].Tex = TransTex;
                        StandardPointsArray[i, j].Taken = true;
                        Points += StandardPointsArray[i, j].PointValue;
                    }
                }
            }
        }
        //Ritar ut poängen på botten av skärmen
        public void DrawPointsAndTime(SpriteBatch sb)
        {
            sb.DrawString(Font, $"Points: {Points} Time: {Minutes}:{(int)Seconds}", StandardPointsArray[StandardPointsArray.GetLength(0) - 1, 0].Pos, Color.White, 0f, Vector2.Zero, new Vector2(2,2), SpriteEffects.None, 1f);
        }

        //Timer för spelet
        public void GameTimer(GameTime gameTime, Game1 game)
        {
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

            if(Minutes == 4 && Seconds <= 58)
            {
                foreach(Enemy e in game.Enemies)
                {
                    e.Gates = true;
                }
            }
        }
    }
}
