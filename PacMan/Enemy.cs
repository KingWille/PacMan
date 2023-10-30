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
        private EnemyController Controller;
        private Animation animation;
        private Random rnd;
        private int Indexer;
        private bool Gates;
        public Enemy(int tileSize, Texture2D tex, Vector2 pos, Rectangle[,] sprites, int indexer, Tiles[,] tileArray)
        {
            this.Pos = pos;
            this.Tex = tex;

            rnd = new Random();
            Speed = (float)rnd.Next(100, 150);
            animation = new Animation(sprites, tex);
            Controller = new EnemyController(animation, Indexer, Tex, tileSize, Speed, tileArray);
            Indexer = indexer;
        }
        public override void Update(GameTime gameTime, Vector2 playerPos)
        {
            Pos = Controller.KeepMoving(Pos, playerPos, gameTime);
        }
        public override void Draw(SpriteBatch sb, GameTime gameTime)
        {
            Controller.DrawMovement(Pos, gameTime, sb);
        }
    }
}
