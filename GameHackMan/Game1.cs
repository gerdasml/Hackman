using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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
            PAUSE,
            VICTORY
        }

        private GraphicsDeviceManager _graphics;     //can change window size 
        private SpriteBatch _spriteBatch;        //will be used in the drawing {spriteBatch.Begin(); spriteBatch.Draw(); spriteBatch.Draw(); ..... spriteBatch.End();}
        private HackMan _hackMan;

        public static readonly int TILE_SIZE = 22;
        public static readonly int SCREEN_HEIGHT = 484 + TILE_SIZE;
        public static readonly int SCREEN_WIDTH = 506;

        internal static CollisionChecker CollisionChecker { get; private set; }

        private Stopwatch _watch;
        private int _secondsAllowed = 110;
        private State _state;

        private List<Food> _food;
        private List<Wall> _wall;
        private int _points = 0;

        private int _cooldownInMilliseconds = 110;
        private Stopwatch _cooldownWatch;

        private string[] _levelFileNames; // cia turim sarasa visu failu musu Level foldery
        private int _currentlevel = 6;
        private List<Level> _levels;

        public Game1()
        {
            _state = State.MENU;
            _graphics = new GraphicsDeviceManager(this);     //initialising the value in the constructor (Game1)
            _graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            _graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";          //the place from wich i take all content for my game
            _levelFileNames = Directory.GetFiles(@"Levels", "*.txt");
        }

        private void LoadLevels()
        {
            _levels = new List<Level>();
            foreach (var name in _levelFileNames)
            {
                _levels.Add(new Level(name));
            }
            _levels.Sort();
        }

        private void StartGame(bool preservePoints = false) // pradedam nauja geima
        {
            if (_levels == null)
                LoadLevels();
            _wall = _levels[_currentlevel].Wall;
            _food = _levels[_currentlevel].Food;
            _hackMan = _levels[_currentlevel].HackMan;
            _watch = null;
            if (!preservePoints)
                _points = 0;
            
            CollisionChecker = new CollisionChecker(_hackMan, _food, _wall);
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
            Graphics.Load(Content);
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
                else if (_state == State.VICTORY)
                    UpdateVictory();
            }
            else if ((int)_cooldownWatch.ElapsedMilliseconds >= _cooldownInMilliseconds)
                _cooldownWatch = null;

            base.Update(gameTime);
        }

        #region Lots of shit :D
        private void UpdateGame()
        {
            if (_watch == null && (
                Keyboard.GetState().IsKeyDown(Keys.Up) ||
                Keyboard.GetState().IsKeyDown(Keys.Down) ||
                Keyboard.GetState().IsKeyDown(Keys.Left) ||
                Keyboard.GetState().IsKeyDown(Keys.Right)
                ))
                _watch = Stopwatch.StartNew();
            if (_watch != null && _watch.ElapsedMilliseconds >= (long)_secondsAllowed * 1000)
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

            if (_food.Count == 0)
            {
                _currentlevel++; // cia
                if (_currentlevel < _levelFileNames.Length)
                {
                    SwitchStateTo(State.GAME);
                    StartGame(preservePoints: true);
                }
                else
                {
                    _state = State.VICTORY;
                    _watch.Stop();
                }
            }

            
                
        }

        private void UpdateVictory()
        {
            
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                SwitchStateTo(State.GAME);
                _currentlevel = 0;
                StartGame();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                SwitchStateTo(State.MENU);
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
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                _currentlevel = 0;
                StartGame();
                SwitchStateTo(State.GAME);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
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
            if (_state == State.VICTORY)
                DrawVictory();
            _spriteBatch.End();

            base.Draw(gameTime);
        }
        #region Custom draw methods
        private void DrawVictory()
        {
            DrawGame();
            Texture2D background = new Texture2D(GraphicsDevice, 1, 1);
            background.SetData(new Color[] { Color.Black });
            _spriteBatch.Draw(background, destinationRectangle: new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), color: new Color(Color.Black, 0.8f));

            string victoryScreen = "CONGRATULATIONS! YOU WON!";
            MakingThingsHappen(victoryScreen, new Rectangle(SCREEN_WIDTH / 16, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 9, SCREEN_WIDTH / 16 * 14, SCREEN_HEIGHT), Color.White);
        }

        private void DrawScore()
        {
            string resultString = "Score: " + _points.ToString();
            var position = new Vector2(0, SCREEN_HEIGHT - TILE_SIZE);
            Vector2 size = Graphics.Font.MeasureString(resultString);
            Rectangle boundaries = new Rectangle(0, 0, SCREEN_WIDTH, TILE_SIZE);
            float xScale = boundaries.Width / size.X;
            float yScale = boundaries.Height / size.Y;
            float scale = Math.Min(xScale, yScale);
            _spriteBatch.DrawString(Graphics.Font, resultString, position, Color.DarkOliveGreen, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        private void DrawTimeLeft()
        {
            string resultString = "Time left: ";
            if (_watch == null)
                resultString += String.Format("{0}:{1:00}", _secondsAllowed/60, _secondsAllowed % 60);
            else
               resultString += String.Format("{0}:{1:00}", (_secondsAllowed - _watch.ElapsedMilliseconds / 1000) / 60, (_secondsAllowed - _watch.ElapsedMilliseconds / 1000) % 60);
            Vector2 size = Graphics.Font.MeasureString(resultString);
            Rectangle boundaries = new Rectangle(0, 0, SCREEN_WIDTH, TILE_SIZE);
            float xScale = boundaries.Width / size.X;
            float yScale = boundaries.Height / size.Y;
            float scale = Math.Min(xScale, yScale);
            var position = new Vector2(SCREEN_WIDTH - size.X * scale, SCREEN_HEIGHT - TILE_SIZE);
            _spriteBatch.DrawString(Graphics.Font, resultString, position, Color.DarkOliveGreen, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        private void DrawDeathScreen()
        {
            DrawGame();
            Texture2D background = new Texture2D(GraphicsDevice, 1, 1);
            background.SetData(new Color[] { Color.Black });
            _spriteBatch.Draw(background, destinationRectangle: new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), color: new Color(Color.Black, 0.8f));

            string gameOverString = "GAME OVER";
            Vector2 size = Graphics.Font.MeasureString(gameOverString);
            Rectangle boundaries = new Rectangle(0, 0, SCREEN_WIDTH * 3 / 4, SCREEN_HEIGHT);
            float xScale = boundaries.Width / size.X;
            float yScale = boundaries.Height / size.Y;
            float scale = Math.Min(xScale, yScale);
            var position = new Vector2(SCREEN_WIDTH / 8, SCREEN_HEIGHT / 2 - size.Y * scale / 2);
            _spriteBatch.DrawString(Graphics.Font, gameOverString, position, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        private void DrawMenu()
        {
            Texture2D background = new Texture2D(GraphicsDevice, 1, 1);
            background.SetData(new Color[] { Color.Black });
            _spriteBatch.Draw(background, destinationRectangle: new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), color: new Color(Color.Black, 1f));

            string nameOfGame = "HACKMAN";
            string menu = "MENU";
            string instructions = "STORYLINE";
            string textOfInstructions1 = "Congratulations!  You've entered the secret developer's ";
            string textOfInstructions2 = "project. The release is tommorow and your mission is to ";
            string textOfInstructions3 = "destroy all the bugs before they destroy your project. ";
            string textOfInstructions4 = "But 'accidentally' you told your supervisor that everything";
            string textOfInstructions5 = "is working so make sure to fix your mistakes.   Glhf!!!   ";
            string controls = "CONTROLS";
            string textOfControls1 = "ARROW KEYS to move";
            string textOfControls2 = "SPACE to pause";
            string textOfControls3 = "ESC to quit to the main menu";
            string otherControls1 = "PRESS ENTER TO START A NEW GAME";
            string otherControls2 = "PRESS ESC TO QUIT";
            MakingThingsHappen(nameOfGame, new Rectangle(SCREEN_WIDTH / 16 * 5, 7, SCREEN_WIDTH / 2, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 15), Color.DarkOliveGreen);
            MakingThingsHappen(menu, new Rectangle(SCREEN_WIDTH / 16 * 6, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 30, SCREEN_WIDTH / 4, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 15), Color.White);
            MakingThingsHappen(instructions, new Rectangle(SCREEN_WIDTH / 32, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 26, SCREEN_WIDTH / 5, SCREEN_HEIGHT - SCREEN_HEIGHT / 5), Color.White);
            MakingThingsHappen(textOfInstructions1, new Rectangle(SCREEN_WIDTH / 32, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 24, SCREEN_WIDTH / 16 * 15, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 5), Color.DarkOliveGreen);
            MakingThingsHappen(textOfInstructions2, new Rectangle(SCREEN_WIDTH / 32, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 23, SCREEN_WIDTH / 16 * 15, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 5), Color.DarkOliveGreen);
            MakingThingsHappen(textOfInstructions3, new Rectangle(SCREEN_WIDTH / 32, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 22, SCREEN_WIDTH / 16 * 15, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 5), Color.DarkOliveGreen);
            MakingThingsHappen(textOfInstructions4, new Rectangle(SCREEN_WIDTH / 32, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 21, SCREEN_WIDTH / 16 * 15, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 5), Color.DarkOliveGreen);
            MakingThingsHappen(textOfInstructions5, new Rectangle(SCREEN_WIDTH / 32, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 20, SCREEN_WIDTH / 16 * 15, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 5), Color.DarkOliveGreen);
            MakingThingsHappen(controls, new Rectangle(SCREEN_WIDTH / 32, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 17, SCREEN_WIDTH / 5, SCREEN_HEIGHT - SCREEN_HEIGHT / 8 * 2), Color.White);
            MakingThingsHappen(textOfControls1, new Rectangle(SCREEN_WIDTH / 32, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 15, SCREEN_WIDTH / 16 * 5, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 5), Color.DarkOliveGreen);
            MakingThingsHappen(textOfControls2, new Rectangle(SCREEN_WIDTH / 32, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 14, SCREEN_WIDTH / 16 * 4, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 5), Color.DarkOliveGreen);
            MakingThingsHappen(textOfControls3, new Rectangle(SCREEN_WIDTH / 32, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 13, SCREEN_WIDTH / 16 * 7, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 5), Color.DarkOliveGreen);
            MakingThingsHappen(otherControls1, new Rectangle(SCREEN_WIDTH / 16 * 3, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 10, SCREEN_WIDTH / 16 * 11, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 7), Color.White);
            MakingThingsHappen(otherControls2, new Rectangle(SCREEN_WIDTH / 16 * 5, SCREEN_HEIGHT - SCREEN_HEIGHT / 32 * 8, SCREEN_WIDTH / 16 * 6, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 7), Color.White);

        }

        private void DrawPauseScreen()
        {
            DrawGame();
            Texture2D background = new Texture2D(GraphicsDevice, 1, 1);
            background.SetData(new Color[] { Color.Black });
            _spriteBatch.Draw(background, destinationRectangle: new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), color: new Color(Color.Black, 0.8f));

            string gamePausedString = "Press SPACE to continue";
            MakingThingsHappen(gamePausedString, new Rectangle(SCREEN_WIDTH / 16, SCREEN_HEIGHT - SCREEN_HEIGHT / 16 * 9, SCREEN_WIDTH / 16 * 14, SCREEN_HEIGHT), Color.White);
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

            Texture2D background = new Texture2D(GraphicsDevice, 1, 1);
            background.SetData(new Color[] { Color.Black });
            _spriteBatch.Draw(background, destinationRectangle: new Rectangle(0, SCREEN_HEIGHT-TILE_SIZE, SCREEN_WIDTH, TILE_SIZE), color: new Color(Color.Black, 1f));
            DrawScore();
            DrawTimeLeft();
        }

        private void MakingThingsHappen(string text, Rectangle boundaries, Color color)
        {
            Vector2 size = Graphics.Font.MeasureString(text);
            float xScale = boundaries.Width / size.X;
            float yScale = boundaries.Height / size.Y;
            float scale = Math.Min(xScale, yScale);
            var position = new Vector2(boundaries.X, boundaries.Y);
            _spriteBatch.DrawString(Graphics.Font, text, position, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }
        #endregion
    }
}
