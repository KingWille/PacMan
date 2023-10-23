using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PacMan
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        internal SpriteBatch _spriteBatch;
        internal LoadTexAndPos loadTexAndPos;

        internal enum GameState
        {
            start,
            game,
            win,
            lose
        }

        GameState state;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            loadTexAndPos = new LoadTexAndPos(this);

            loadTexAndPos.LoadMap();

            _graphics.PreferredBackBufferWidth = loadTexAndPos.Mapper[0].Length;
            _graphics.PreferredBackBufferHeight = loadTexAndPos.Mapper.Count;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            switch(state)
            {
                case GameState.start:
                    break;
                case GameState.game:
                    break;
                case GameState.win:
                    break;
                case GameState.lose:
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack, null);

            switch (state)
            {
                case GameState.start:
                    for (int i = 0; i < loadTexAndPos.TilesArray.GetLength(0); i++)
                    {
                        for (int j = 0; i < loadTexAndPos.TilesArray.GetLength(1); j++)
                        {
                            loadTexAndPos.TilesArray[i, j].Draw(this);
                        }
                    }
                            break;
                case GameState.game:
                    break;
                case GameState.win:
                    break;
                case GameState.lose:
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}