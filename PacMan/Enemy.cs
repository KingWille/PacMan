using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PacMan
{
    internal class Enemy : Characters
    {
        private PlayerController Controls;
        public Enemy(Texture2D tex, Vector2 pos)
        {
            this.Tex = tex;
            this.Pos = pos;

            Controls = new PlayerController();

        }
        public override void Update()
        {
            
        }
        public override void Draw()
        {
            
        }
    }
}
