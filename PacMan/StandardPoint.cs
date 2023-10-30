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
        }
        //Uppdateringsmetod för standard poäng föremål
        public void Update()
        {

        }
        //Ritmetod för standard poäng föremål
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Tex, Pos, Color.White);
        }
    }
}
