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
        private Controllers Controller;
        private Animation animation;
        private Random rnd;
        private int Indexer;
        internal bool Gates;
        public Enemy(int tileSize, Texture2D tex, Texture2D path, Vector2 pos, Rectangle[,] sprites, int indexer, Tiles[,] tileArray)
        {
            this.Pos = pos;
            this.Tex = tex;

            rnd = new Random();
            Speed = (float)rnd.Next(100, 150);
            animation = new Animation(sprites, tex);
            Gates = false;

            if(indexer == 0)
            {
                Controller = new EnemyController(animation, Tex, path, tileSize, Speed, tileArray);
            }
            else
            {
                Controller = new EnemyControllerRandom(animation, Tex, tileArray, Speed, tileSize);
            }

        }
        public override void Update(GameTime gameTime, Vector2 playerPos)
        {
            Pos = Controller.KeepMoving(Pos, playerPos, gameTime, Gates);
        }
        public override void Draw(SpriteBatch sb, GameTime gameTime)
        {
            Controller.DrawMovement(Pos, gameTime, sb);
        }
    }
}
