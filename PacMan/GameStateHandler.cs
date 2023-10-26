using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    internal static class GameStateHandler
    {
        public static void UpdateGame(Game1 game, GameTime gameTime)
        {
            game.player.Update(gameTime);
        }
        public static void UpdateWin(Game1 game, GameTime gameTime)
        {

        }
        public static void UpdateLoss()
        {

        }
        public static void UpdateStart()
        {

        }
        public static void DrawGame(Game1 game)
        {
            //Ritar upp kartan
            for (int i = 0; i < game.TilesArray.GetLength(0); i++)
            {
                for (int j = 0; j < game.TilesArray.GetLength(1); j++)
                {
                    game.TilesArray[i, j].Draw(game._spriteBatch);
                }
            }

            game.player.Draw(game._spriteBatch);
        }
        public static void DrawWin()
        {

        }
        public static void DrawLoss()
        {

        }
        public static void DrawStart()
        {

        }
    }
}
