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
        internal Tiles[,] TilesArray;

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
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            state = GameState.game;
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            loadTexAndPos = new LoadTexAndPos(this);

            loadTexAndPos.LoadMap(this);

            _graphics.PreferredBackBufferWidth = TilesArray.GetLength(1) * loadTexAndPos.tileSize * 2;
            _graphics.PreferredBackBufferHeight = TilesArray.GetLength(0) * loadTexAndPos.tileSize * 2;
            _graphics.ApplyChanges();

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
                    GameStateHandler.UpdateStart();
                    break;
                case GameState.game:
                    GameStateHandler.UpdateGame();
                    break;
                case GameState.win:
                    GameStateHandler.UpdateWin();
                    break;
                case GameState.lose:
                    GameStateHandler.UpdateLoss();
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack, null);

            switch (state)
            {
                case GameState.start:
                    GameStateHandler.DrawStart();
                    break;
                case GameState.game:
                    GameStateHandler.DrawGame(this);
                    break;
                case GameState.win:
                    GameStateHandler.DrawWin();
                    break;
                case GameState.lose:
                    GameStateHandler.DrawLoss();
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}