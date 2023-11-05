using Microsoft.VisualBasic;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PacMan
{
    internal class WinScreen : Screens
    {
        private int Points;
        public WinScreen(Texture2D tex, SpriteFont font)
        {
            Tex = tex;
            Pos = Vector2.Zero;
            Font = font;
        }

        //Ritar ut strängen
        public override void DrawString(SpriteBatch sb)
        {

            string stringFont = $"Congrats! You won!\nTotal Points: {Points}\nPress enter to restart...";
            Vector2 measuredString = Font.MeasureString(stringFont);
            StringPos = new Vector2(Tex.Width / 2 - measuredString.X, Tex.Height / 2 - measuredString.Y + 20);

            sb.DrawString(Font, stringFont, StringPos, Color.White, 0f, Vector2.Zero, new Vector2(2, 2), SpriteEffects.None, 1f);

        }

        //Uppdaterar poängen
        public void UpdatePoints(int points)
        {
            Points = points;
        }

        //Kollar om man har tryckt enter
        public void CheckIfEnterPressed(Game1 game)
        {
            var KeyPressed = Keyboard.GetState();

            if (KeyPressed.IsKeyDown(Keys.Enter))
            {
                game.state = Game1.GameState.restart;
            }
        }
    }
}
