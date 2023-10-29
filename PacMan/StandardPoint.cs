using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace PacMan
{
    internal class StandardPoint : PointItems
    {
        public StandardPoint(Texture2D tex, Vector2 pos) 
        {
            this.Tex = tex;
            this.Pos = pos;
            this.PointValue = 100;
        }
        public void Update()
        {

        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Tex, Pos, Color.White);
        }
    }
}
