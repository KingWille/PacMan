using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacMan
{
    internal static class GameStateHandler
    {
        public static void UpdateGame(Game1 game, GameTime gameTime)
        {
            game.player.Update(gameTime, game.player.Pos, game.PointManager.Invurnability);
            game.PointManager.Update(game.player.Pos, game, gameTime);

            foreach(Enemy e in game.Enemies)
            {
                e.Update(gameTime, game.player.Pos, game.PointManager.Invurnability);
            }
            
        }
        public static void UpdateWin(Game1 game, GameTime gameTime)
        {
            game.winScreen.Update();
            game.winScreen.CheckIfEnterPressed(game);
        }
        public static void UpdateLoss(Game1 game)
        {
            game.loseScreen.Update();
            game.loseScreen.CheckIfEnterPressed(game);
        }
        public static void UpdateStart(Game1 game)
        {
            game.startMenu.Update();
            game.startMenu.CheckIfEnterPressed(game);
        }
        public static void UpdateLevelEditor(Game1 game)
        {
            
        }
        public static void DrawGame(Game1 game, GameTime gameTime)
        {
            //Ritar upp kartan
            for (int i = 0; i < game.TilesArray.GetLength(0); i++)
            {
                for (int j = 0; j < game.TilesArray.GetLength(1); j++)
                {
                    game.TilesArray[i, j].Draw(game._spriteBatch);
                }
            }

            //Ritar spelaren
            game.player.Draw(game._spriteBatch, gameTime);

            //Ritar upp poäng och tid
            game.PointManager.Draw(game._spriteBatch);

            for(int i = 0; i < game.Enemies.Length; i++)
            {
                game.Enemies[i].Draw(game._spriteBatch, gameTime);
            }
        }
        public static void DrawWin(Game1 game)
        {
            game.winScreen.Draw(game._spriteBatch);
        }
        public static void DrawLoss(Game1 game)
        {
            game.loseScreen.Draw(game._spriteBatch);
        }
        public static void DrawStart(Game1 game)
        {
            game.startMenu.Draw(game._spriteBatch);
        }
        public static void DrawLevelEditor(Game1 game)
        {
            game.levelEditor.Draw(game._spriteBatch);
        }
    }
}
