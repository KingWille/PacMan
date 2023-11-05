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
        internal bool Ghost;
        internal bool Eaten;
        public Enemy(int tileSize, Texture2D tex, Texture2D path, Vector2 pos, Rectangle[,] sprites, int indexer, Tiles[,] tileArray)
        {
            this.Pos = pos;
            this.Tex = tex;

            rnd = new Random();
            Speed = (float)rnd.Next(100, 150);
            animation = new Animation(sprites, tex);
            Gates = false;
            Ghost = false;
            Eaten = false;

            if(indexer == 0)
            {
                Controller = new EnemyController(animation, Tex, path, tileSize, Speed, tileArray);
            }
            else
            {
                Controller = new EnemyControllerRandom(animation, Tex, tileArray, Speed, tileSize);
            }

            Rect = new Rectangle((int)Pos.X, (int)Pos.Y, tileSize, tileSize);


        }
        public override void Update(GameTime gameTime, Vector2 playerPos, bool invurnability)
        {
            Pos = Controller.KeepMoving(Pos, playerPos, gameTime, Gates, Ghost, Eaten);
            Rect.X = (int)Pos.X;
            Rect.Y = (int)Pos.Y;
        }
        public override void Draw(SpriteBatch sb, GameTime gameTime)
        {
            Controller.DrawMovement(Pos, gameTime, sb);
        }
    }
}
