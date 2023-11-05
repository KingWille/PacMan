using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace PacMan
{
    internal class LevelEditor
    {
        private Tiles[] UseableTiles;
        private Rectangle[,] TexRects;
        private Texture2D Tex;
        private Vector2 Pos;
        private int TileSize;

        public LevelEditor(Texture2D tex, Rectangle[,] tileRects) 
        {
            Tex = tex;
            TexRects = tileRects;
            TileSize = 33;
            
        }

        public void Draw(SpriteBatch sb)
        {
            for(int i = 0; i < TexRects.GetLength(0); i++)
            {
                for(int j = 0;  j < TexRects.GetLength(1); j++)
                {
                    Pos = new Vector2(TileSize * j, TileSize * i);
                    sb.Draw(Tex, Pos, TexRects[i, j], Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                }
            }
        }
    }
}
