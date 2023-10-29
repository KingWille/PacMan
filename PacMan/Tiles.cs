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
        //0 = up, 1 = right, 2 = down, 3 = left
        internal Dictionary<int, bool> AllowedDirections;
        internal Vector2 Pos;
        internal Rectangle Rect;

        internal bool Empty;
        internal bool PointTile;
        internal int TileScale;
        public Tiles(Texture2D tex, Vector2 pos, Rectangle rect, bool[] boolArray)
        {
            Tex = tex;
            Pos = pos;
            Rect = rect;

            PointTile = true;

            TileScale = 2;

            AllowedDirections = new Dictionary<int, bool>();

            for(int i = 0; i < boolArray.Length; i++)
            {
                if (boolArray[i])
                {
                    AllowedDirections.Add(i, true);
                }
                else
                {
                    AllowedDirections.Add(i, false);
                }
            }
        }
        public Tiles(Texture2D tex, Vector2 pos, bool empty)
        {
            Tex = tex;
            Pos = pos;
            Empty = empty;

        }

        public void Draw(SpriteBatch sb)
        {
            if (Empty)
            {
                sb.Draw(Tex, Pos, Color.White);
            }
            else
            {
                sb.Draw(Tex, Pos, Rect, Color.White, 0f, Vector2.Zero, new Vector2(TileScale, TileScale), SpriteEffects.None, 0f);
            }
        }
    }
}
