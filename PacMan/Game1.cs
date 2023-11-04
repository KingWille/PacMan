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
        internal Enemy[] Enemies;
        internal StandardPoint[,] standardPointsArray;
        internal PointManager PointManager;
        internal LoseScreen loseScreen;
        internal StartMenu startMenu;
        internal WinScreen winScreen;

        internal Player player;

        internal Vector2 WindowSize;

        internal enum GameState
        {
            start,
            game,
            win,
            lose,
            restart
        }

        internal GameState state;
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
            state = GameState.start;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Enemies = new Enemy[2];

            loadTexAndPos = new LoadTexAndPos(this);
            PointManager = new PointManager(loadTexAndPos.Font, loadTexAndPos.TransTex);

            loadTexAndPos.LoadMap(this);
            loadTexAndPos.LoadPlayerSpritesAndPos(TilesArray);
            loadTexAndPos.LoadPoints(TilesArray, PointManager);
            loadTexAndPos.LoadEnemyAndFruitSpritesAndEnemyPos(TilesArray);

            _graphics.PreferredBackBufferWidth = TilesArray.GetLength(1) * loadTexAndPos.TileSize * 2;
            _graphics.PreferredBackBufferHeight = TilesArray.GetLength(0) * loadTexAndPos.TileSize * 2;
            _graphics.ApplyChanges();

            loseScreen = new LoseScreen(loadTexAndPos.EndScreen, loadTexAndPos.Font, PointManager.Points);
            startMenu = new StartMenu(loadTexAndPos.StartMenu, loadTexAndPos.Font);
            winScreen = new WinScreen(loadTexAndPos.WinScreen, loadTexAndPos.Font);
            

            WindowSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            player = new Player(loadTexAndPos.PlayerStartPos, loadTexAndPos.TileSize, loadTexAndPos.PlayerTex, loadTexAndPos.PlayerTexTurned, loadTexAndPos.PlayerSprites, TilesArray, WindowSize);

            for(int i = 0; i < Enemies.Length;  i++)
            {
                if (i == 0)
                {
                    Enemies[i] = new Enemy(loadTexAndPos.TileSize, loadTexAndPos.EnemiesAndFruitTex, loadTexAndPos.PathTile, loadTexAndPos.EnemyStartPos1, loadTexAndPos.EnemyAndFruitSprites, i, TilesArray);
                }
                else 
                {
                    Enemies[i] = new Enemy(loadTexAndPos.TileSize, loadTexAndPos.EnemiesAndFruitTex, loadTexAndPos.PathTile, loadTexAndPos.EnemyStartPos2, loadTexAndPos.EnemyAndFruitSprites, i, TilesArray); 
                }
            }
            

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
                    GameStateHandler.UpdateStart(this);
                    break;
                case GameState.game:
                    GameStateHandler.UpdateGame(this, gameTime);
                    break;
                case GameState.win:
                    GameStateHandler.UpdateWin(this, gameTime);
                    break;
                case GameState.lose:
                    GameStateHandler.UpdateLoss(this);
                    break;
                case GameState.restart:
                    LoadContent(); 
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
                    GameStateHandler.DrawStart(this);
                    break;
                case GameState.game:
                    GameStateHandler.DrawGame(this, gameTime);
                    break;
                case GameState.win:
                    GameStateHandler.DrawWin(this);
                    break;
                case GameState.lose:
                    GameStateHandler.DrawLoss(this);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}