using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PacMan
{
    internal class SpecialPointItems : PointItems
    {
        internal bool Used;
        internal Rectangle TexRect;
        public SpecialPointItems(Texture2D tex, Vector2 pos, Rectangle[,] rect) 
        {
            Tex = tex;
            Pos = pos;
            PointValue = 200;
            Taken = false;
            Used = false;
            special = true;

            TexRect = rect[5, 0];
        }

        public void PowerUp(Enemy[] enemies)
        {
            if(Used)
            {
                foreach(Enemy enemy in enemies)
                {
                    enemy.Ghost = true;
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Tex, Pos, TexRect, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.1f);
        }

    }
}
