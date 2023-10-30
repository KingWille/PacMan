using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PacMan
{
    internal class Animation
    {
        internal Rectangle[] Rects;
        internal Rectangle[,] Rects2;
        internal Texture2D SpriteSheet;
        internal Texture2D SpriteSheetTurned;
        private int Frames;
        private int CurrentFrame;
        private float FrameInterval;
        private float FrameTimer;
        public Animation(Rectangle[,] rects, Texture2D spriteSheet)
        {
            Rects2 = rects;
            Frames = 2;
            FrameInterval = 200f;
            FrameTimer = 200f;
            CurrentFrame = 0;
        }
        public Animation(Rectangle[] rects, Texture2D spriteSheet, Texture2D spriteSheetTurned)
        {
            Rects = rects;
            Frames = rects.Length;
            FrameInterval = 100f;
            FrameTimer = 100f;
            CurrentFrame = 0;
            SpriteSheet = spriteSheet;
            SpriteSheetTurned = spriteSheetTurned;
        }
        public int RunAnimation(GameTime gameTime)
        {
            FrameTimer -= gameTime.ElapsedGameTime.Milliseconds;

            if(FrameTimer < 0 )
            {
                FrameTimer = FrameInterval;
                CurrentFrame++;
                
                if(CurrentFrame == Frames)
                {
                    CurrentFrame = 0;
                }
            }

            return CurrentFrame;
        }
    }
}
