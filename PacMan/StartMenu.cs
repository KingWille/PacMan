using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PacMan
{
    internal class StartMenu : Screens
    {
        public StartMenu(Texture2D tex, SpriteFont font)
        {
            Tex = tex;
            Pos = Vector2.Zero;
            Font = font;
        }

        //Ritar ut strängen
        public override void DrawString(SpriteBatch sb)
        {

            string stringFont = "Welcome! Press enter to start";
            Vector2 measuredString = Font.MeasureString(stringFont);
            StringPos = new Vector2(Tex.Width / 2 - measuredString.X, Tex.Height / 2 - measuredString.Y);

            sb.DrawString(Font, stringFont, StringPos, Color.White, 0f, Vector2.Zero, new Vector2(2, 2), SpriteEffects.None, 1f);

        }
        //Kollar om man har tryckt enter

        public void CheckIfEnterPressed(Game1 game)
        {
            var KeyPressed = Keyboard.GetState();

            if (KeyPressed.IsKeyDown(Keys.Enter))
            {
                game.state = Game1.GameState.game;
            }
        }
    }
}
