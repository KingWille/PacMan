using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PacMan
{
    internal class Screens
    {
        internal Texture2D Tex;
        internal Vector2 Pos;
        internal Vector2 StringPos;
        internal SpriteFont Font;

        public virtual void Update()
        {

        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(Tex, Pos, null, Color.White, 0F, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            DrawString(sb);
        }

        public virtual void DrawString(SpriteBatch sb)
        {

        }
    }
}
