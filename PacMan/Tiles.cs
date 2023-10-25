using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PacMan
{
    internal class Tiles
    {
        private Texture2D Tex;
        private Vector2 Pos;
        private Rectangle Rect;

        internal bool Empty;
        internal int TileScale;
        public Tiles(Texture2D tex, Vector2 pos, Rectangle rect)
        {
            Tex = tex;
            Pos = pos;
            Rect = rect;

            TileScale = 2;
        }
        public Tiles(Texture2D tex, Vector2 pos, bool empty)
        {
            Tex = tex;
            Pos = pos;
            Empty = empty;

        }

        public void Draw(Game1 game)
        {
            if (Empty)
            {
                game._spriteBatch.Draw(Tex, Pos, Color.White);
            }
            else
            {
                game._spriteBatch.Draw(Tex, Pos, Rect, Color.White, 0f, Vector2.Zero, new Vector2(TileScale, TileScale), SpriteEffects.None, 0f);
            }
        }
    }
}
