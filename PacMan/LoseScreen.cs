using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PacMan
{
    internal class LoseScreen : Screens
    {
        private int Points;
        public LoseScreen(Texture2D tex, SpriteFont font, int points) 
        {
            Tex = tex;
            Font = font;
            Points = points;
            Pos = Vector2.Zero;

        }

        public override void DrawString(SpriteBatch sb)
        {
            Vector2 measuredString = Font.MeasureString($"Total Points: {Points}\nPress Enter to restart...");
            string stringFont = $"Total Points: {Points}\nPress Enter to restart...";

            StringPos = new Vector2(Tex.Width / 2 -measuredString.X / 2, Tex.Height - measuredString.Y - 20);

            sb.DrawString(Font, stringFont, StringPos, Color.White);
        }

        public void UpdatePoints(int points)
        {
            Points = points;
        }

        public void CheckIfEnterPressed(Game1 game)
        {
            var KeyPressed = Keyboard.GetState();

            if(KeyPressed.IsKeyDown(Keys.Enter))
            {
                game.state = Game1.GameState.restart;
            }
        }
    }
}
