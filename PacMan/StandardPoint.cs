using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace PacMan
{
    internal class StandardPoint : PointItems
    {
        public StandardPoint(Texture2D tex, Vector2 pos, bool taken) 
        {
            this.Tex = tex;
            this.Pos = pos;
            this.Taken = taken;
            this.PointValue = 100;
            this.special = false;
        }
    }
}
