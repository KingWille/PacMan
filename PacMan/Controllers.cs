using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PacMan
{
    internal class Controllers
    {
        protected Animation Animation;
        protected Texture2D Tex;
        protected Tiles[,] TileArray;
        protected Vector2 Destination;
        protected Vector2 Direction;
        protected Vector2 LastLinedUpTile;

        protected int Index;
        protected int SecondIndexer;
        protected int IndexMultiplierFrame1;
        protected int IndexMultiplierFrame2;
        protected int DirectionIndex;
        protected int TileSize;
        protected float Speed;

        public virtual Vector2 KeepMoving(Vector2 pos, Vector2 playerPos, GameTime gameTime, bool released)
        {
            return Vector2.Zero;
        }

        public virtual void DrawMovement(Vector2 Pos, GameTime gameTime, SpriteBatch sb)
        {

        }
    }
}
