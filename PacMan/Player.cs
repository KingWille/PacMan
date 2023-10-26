using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PacMan
{
    
    internal class Player : Characters
    {

        private PlayerController Controller;
        private InputHandler InputHandler;
        private Rectangle tempRect;
        private Texture2D tempTex;
        public Player(Vector2 pos, int tileSize, Texture2D tex, Rectangle rect, Tiles[,] tileArray) 
        {
            Pos = pos;
            tempTex = tex;
            tempRect = rect;
            Speed = 100f;
            Controller = new PlayerController(tileSize, tileArray, Speed);
            InputHandler = new InputHandler();
        }
        public override void Update(GameTime gameTime)
        {
            InputHandler.GetState();

            if (InputHandler.HasBeenPressed(Keys.Up))
            {
                Controller.DirectionUp(Pos, gameTime);
            }
            else if (InputHandler.HasBeenPressed(Keys.Down))
            {
                Controller.DirectionDown(Pos, gameTime);
            }
            else if(InputHandler.HasBeenPressed(Keys.Left))
            {
                Controller.DirectionLeft(Pos, gameTime);
            }
            else if(InputHandler.HasBeenPressed(Keys.Right))
            {
                Controller.DirectionRight(Pos, gameTime);
            }
            else
            {
                Pos = Controller.KeepMoving(Pos, gameTime);
            }
        }
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tempTex, Pos, tempRect, Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 1f);
        }
    }
}
