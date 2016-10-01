using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameHackMan
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        enum State
        {
            MENU,
            GAME,
            GAMEOVER,
            PAUSE
        }

        private GraphicsDeviceManager _graphics;     //can change window size 
        private SpriteBatch _spriteBatch;        //will be used in the drawing {spriteBatch.Begin(); spriteBatch.Draw(); spriteBatch.Draw(); ..... spriteBatch.End();}
        private HackMan _hackMan;
        private SpriteFont _font;

        public static readonly int TILE_SIZE = 22;
        public static readonly int SCREEN_HEIGHT = 484 + TILE_SIZE;
        public static readonly int SCREEN_WIDTH = 506;

        internal static CollisionChecker CollisionChecker { get; private set; }

        private Stopwatch _watch;
        private int _secondsAllowed = 15;
        private State _state;

        private List<Food> _food;
        private List<Wall> _wall;
        private int _points = 0;

        private int _cooldownInMilliseconds = 100;
        private Stopwatch _cooldownWatch;

        public Game1()
        {
            _state = State.MENU;
            _graphics = new GraphicsDeviceManager(this);     //initialising the value in the constructor (Game1)
            _graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            _graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";          //the place from wich i take all content for my game
            
        }

        private void StartGame() // pradedam nauja geima
        {
            _wall = new List<Wall>();
            _food = new List<Food>();
            _watch = null;
            _points = 0;
            ReadMap(); // einam cia
            CollisionChecker = new CollisionChecker(_hackMan, _food, _wall);
        }

        private void ReadMap()
        {
            string[] map = System.IO.File.ReadAllLines(@"Levels/Level0.txt");

            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == 'P') _hackMan = new HackMan(x, y); // sukuriam nauja objekta is klases HackMan
                    else if (map[y][x] == '.') _food.Add(new Food(x, y));
                    else if (map[y][x] == '#') _wall.Add(new Wall(x, y));
                }
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();      //calls initialize method from class Game (by default)
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            HackMan.LoadContent(Content);      //we have to give some kind of value to method, so since that object is the whole game we give the method itself, in this case this
            Wall.LoadContent(Content);
            Food.LoadContent(Content);

            _font = Content.Load<SpriteFont>("Font");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {


            // TODO: Add your update logic here
            if (_cooldownWatch == null)
            {
                if (_state == State.MENU)
                    UpdateMenu();
                else if (_state == State.GAME) // uzregistruos cia. nuves i pause
                    UpdateGame();
                else if (_state == State.GAMEOVER)
                    UpdateDeathScreen();
                else if (_state == State.PAUSE)
                {
                    UpdatePause(); // cia. ir grazins vel i game
                }
            }
            else if (_cooldownWatch.ElapsedMilliseconds >= _cooldownInMilliseconds)
                _cooldownWatch = null;

            base.Update(gameTime);
        }

        #region Lots of shit
        private void UpdateGame()
        {
            if (_watch == null && (
                Keyboard.GetState().IsKeyDown(Keys.Up) ||
                Keyboard.GetState().IsKeyDown(Keys.Down) ||
                Keyboard.GetState().IsKeyDown(Keys.Left) ||
                Keyboard.GetState().IsKeyDown(Keys.Right)
                ))
                _watch = Stopwatch.StartNew();
            if (_watch != null && _watch.ElapsedMilliseconds >= _secondsAllowed * 1000)
            {
                SwitchStateTo(State.GAMEOVER);
                _watch.Stop();
            }

            if (_state == State.GAME)
            {
                _hackMan.Update();
                _points += CollisionChecker.CheckFoodCollitions();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) // o cia reiktu kazkaip sustabdyt geima. kad tipo jei isejom i meniu tj vsio, nebegtu laikas ir pan.
            {
                SwitchStateTo(State.MENU);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                SwitchStateTo(State.PAUSE);
                _watch.Stop();
            }
                
        }

        private void UpdateMenu()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                SwitchStateTo(State.GAME); // cia reiktu kokio nors ResetGame() metodo. kad tipo kai pradedam nauja geima kad viskas butu is naujo
                StartGame();
            }
        }

        private void UpdateDeathScreen()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                StartGame();
                SwitchStateTo(State.GAME);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.M))
                SwitchStateTo(State.MENU);
        }

        private void UpdatePause()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && _state == State.PAUSE)
            {
                SwitchStateTo(State.GAME); // anksciau busena keitem tiesiog taip. db keisim SwitchStateTo(State.GAME);
                _watch.Start();
            }
        }

        private void SwitchStateTo(State newState)
        {
            _state = newState;
            _cooldownWatch = Stopwatch.StartNew();
        }
        #endregion
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            if (_state == State.MENU)
                DrawMenu();
            if (_state == State.GAME)
                DrawGame();
            if (_state == State.GAMEOVER)
                DrawDeathScreen();
            if (_state == State.PAUSE)
                DrawPauseScreen();
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
        #region Custom draw methods
        private void DrawScore()
        {
            string resultString = "Score: " + _points.ToString();
            var position = new Vector2(0, SCREEN_HEIGHT - TILE_SIZE);
            Vector2 size = _font.MeasureString(resultString);
            Rectangle boundaries = new Rectangle(0, 0, SCREEN_WIDTH, TILE_SIZE);
            float xScale = boundaries.Width / size.X;
            float yScale = boundaries.Height / size.Y;
            float scale = Math.Min(xScale, yScale);
            _spriteBatch.DrawString(_font, resultString, position, Color.Green, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        private void DrawTimeLeft()
        {
            string resultString = "Time left: ";
            if (_watch == null)
                resultString += String.Format("{0}:{1:00}", _secondsAllowed/60, _secondsAllowed % 60);
            else
               resultString += String.Format("{0}:{1:00}", (_secondsAllowed - _watch.ElapsedMilliseconds / 1000) / 60, (_secondsAllowed - _watch.ElapsedMilliseconds / 1000) % 60);
            Vector2 size = _font.MeasureString(resultString);
            Rectangle boundaries = new Rectangle(0, 0, SCREEN_WIDTH, TILE_SIZE);
            float xScale = boundaries.Width / size.X;
            float yScale = boundaries.Height / size.Y;
            float scale = Math.Min(xScale, yScale);
            var position = new Vector2(SCREEN_WIDTH - size.X * scale, SCREEN_HEIGHT - TILE_SIZE);
            _spriteBatch.DrawString(_font, resultString, position, Color.Green, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        private void DrawDeathScreen()
        {
            Texture2D background = new Texture2D(GraphicsDevice, 1, 1);
            background.SetData(new Color[] { Color.Black });
            _spriteBatch.Draw(background, destinationRectangle: new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), color: new Color(Color.Black, 0.8f));

            string gameOverString = "GAME OVER";
            Vector2 size = _font.MeasureString(gameOverString);
            Rectangle boundaries = new Rectangle(0, 0, SCREEN_WIDTH * 3 / 4, SCREEN_HEIGHT);
            float xScale = boundaries.Width / size.X;
            float yScale = boundaries.Height / size.Y;
            float scale = Math.Min(xScale, yScale);
            var position = new Vector2(SCREEN_WIDTH / 8, SCREEN_HEIGHT / 2 - size.Y * scale / 2);
            _spriteBatch.DrawString(_font, gameOverString, position, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        private void DrawMenu()
        {
            Texture2D background = new Texture2D(GraphicsDevice, 1, 1);
            background.SetData(new Color[] { Color.Black });
            _spriteBatch.Draw(background, destinationRectangle: new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), color: new Color(Color.Black, 1f));

            string nameOfGame = "HACHMAN";
            string instructions = "INSTRUCTIONS";
            string textOfInstructions = "Congratulations! You've entered to the secret developer's project.\n" +
                "Tomorrow is release and your mission is to destroy all the bugs before they destroyed your project.\n" +
                "But 'accidentally' you told your supervisor that everything is working so be carefull that he wouldn't cath you!\n"+
                "Glhf ☺";
            string controls = "CONTROLS";
            string textOfControls = "ARROW KEYS to move\n" +
                "SPACE to pause\n" +
                "M to quit to the main menu";
            string otherControls = "PRESS ENTER TO START A NEW GAME\n" +
                "PRESS ESC TO QUIT";
            MakingThingsHappen(nameOfGame, new Rectangle(0, 0, SCREEN_WIDTH / 4, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 15), Color.White);
            
        }

        private void DrawPauseScreen()
        {
            Texture2D background = new Texture2D(GraphicsDevice, 1, 1);
            background.SetData(new Color[] { Color.Black });
            _spriteBatch.Draw(background, destinationRectangle: new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), color: new Color(Color.Black, 0.2f));

            string gamePausedString = "Press SPACE to continue";
            MakingThingsHappen(gamePausedString, new Rectangle(0, 0, SCREEN_WIDTH * 3 / 4, SCREEN_HEIGHT), Color.White);
        }

        private void DrawGame()
        {
            foreach (var w in _wall)
            {
                w.Draw(_spriteBatch);
            }
            foreach (var f in _food)
            {
                f.Draw(_spriteBatch);
            }
            _hackMan.Draw(_spriteBatch);
            DrawScore();
            DrawTimeLeft();
        }

        private void MakingThingsHappen(string text, Rectangle boundaries, Color color)
        {
            Vector2 size = _font.MeasureString(text);
            float xScale = boundaries.Width / size.X;
            float yScale = boundaries.Height / size.Y;
            float scale = Math.Min(xScale, yScale);
            var position = new Vector2(boundaries.X, boundaries.Y);
            _spriteBatch.DrawString(_font, text, position, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }
        #endregion
    }
}
