using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Net.Http;
using System.Diagnostics;

namespace PacMan
{
    
    internal class Player : Characters
    {
        private Animation PlayerAnimation;
        private PlayerController Controller;
        private InputHandler InputHandler;
        private Rectangle[] FrameArray;
        private Vector2 WindowSize;
        public Player(Vector2 pos, int tileSize, Texture2D tex, Texture2D texTurned, Rectangle[] rect, Tiles[,] tileArray, Vector2 windowSize) 
        {
            Pos = pos;
            Speed = 200f;
            WindowSize = windowSize;
            FrameArray = rect;
            PlayerAnimation = new Animation(rect, tex, texTurned);
            Controller = new PlayerController(tileSize, tileArray, Speed, PlayerAnimation);
            InputHandler = new InputHandler();

            Rect = new Rectangle((int)Pos.X, (int)Pos.Y, tileSize, tileSize);
        }
        //Update metoden för spelaren
        public override void Update(GameTime gameTime, Vector2 playerPos, bool invurnability)
        {
            InputHandler.GetState();
            
            if (InputHandler.HasBeenPressed(Keys.Up))
            {
                Controller.DirectionUp(Pos, gameTime);
                Controller.inputHandler.SetLastKey(Keys.Up);
            }
            else if (InputHandler.HasBeenPressed(Keys.Down))
            {
                Controller.DirectionDown(Pos, gameTime);
                Controller.inputHandler.SetLastKey(Keys.Down);
            }
            else if (InputHandler.HasBeenPressed(Keys.Left))
            {
                Controller.DirectionLeft(Pos, gameTime);
                Controller.inputHandler.SetLastKey(Keys.Left);
            }
            else if (InputHandler.HasBeenPressed(Keys.Right))
            {
                Controller.DirectionRight(Pos, gameTime);
                Controller.inputHandler.SetLastKey(Keys.Right);
            }
            else
            {
                Pos = Controller.KeepMoving(Pos, gameTime, invurnability);
                Rect.X = (int)Pos.X;
                Rect.Y = (int)Pos.Y;
            }

            IfOutOfBound();
            Debug.WriteLine(Pos);
        }
        //Rit metoden för spelaren
        public override void Draw(SpriteBatch sb, GameTime gameTime)
        {
            Controller.DrawMovement(Pos, gameTime, sb);
        }
        //Flyttar pacman om man är utanför skärmen
        public void IfOutOfBound()
        {
            if(0 > Pos.X + FrameArray[0].Width)
            {
                Pos.X = WindowSize.X;
            }
            else if(WindowSize.X < Pos.X)
            {
                Pos.X = 0 - FrameArray[0].Width;
            }
        }
    }
}
