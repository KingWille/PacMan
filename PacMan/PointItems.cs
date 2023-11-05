using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PacMan
{
    internal class PointItems
    {
        internal Texture2D Tex;
        internal Vector2 Pos;
        internal int PointValue;
        internal bool Taken;
        internal bool special;
        public PointItems() 
        {

        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(Tex, Pos, Color.White);
        }
    }
}
