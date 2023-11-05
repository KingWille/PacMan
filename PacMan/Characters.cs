using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace PacMan
{
    internal abstract class Characters
    {
        internal Vector2 Pos;
        internal Texture2D Tex;
        internal Rectangle Rect;
        internal float Speed;

        public abstract void Update(GameTime gameTime, Vector2 playerPos, bool invurnability);
        public abstract void Draw(SpriteBatch sb, GameTime gameTime);
    }
}
