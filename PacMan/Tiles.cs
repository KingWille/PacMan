using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PacMan
{
    internal class Tiles : BaseVariables
    {
        bool Wall;
        public Tiles(Texture2D tex, Vector2 pos, bool wall)
        {
            Tex = tex;
            Pos = pos;
            Wall = wall;
        }

        public void Draw(Game1 game)
        {
            game._spriteBatch.Draw(Tex, Pos, Color.White);
        }
    }
}
