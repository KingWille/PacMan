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
        internal Texture2D Tex;
        internal Vector2 Pos;

        public abstract void Update();
        public abstract void Draw();
    }
}
