using GameHackMan.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace GameHackMan
{
    public partial class Game1 : Game           //partial because we divided code into several files
    {
        enum State          //enum for game states
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
        //private Ghost _ghost;          

        public static readonly int TILE_SIZE = 22;
        public static readonly int SCREEN_HEIGHT = 484 + 2 * TILE_SIZE;         //+TILE_SIZE because in the bottom of the window we can see score and time
        public static readonly int SCREEN_WIDTH = 506;
        public static readonly int BONUS_POINTS_MULTIPLIER = 7;
        private static readonly string HIGH_SCORE_FILE_NAME = "HighScore.txt";

        //[4 - auto-implemented]
        internal static CollisionChecker CollisionChecker { get; private set; }         //internal because we can see only in this asembly

        private Stopwatch _watch;
        private int _secondsAllowed;
        private State _state;

        private List<Ghost> _ghost; 
        private List<Food> _food;
        private List<Wall> _wall;
        private int _points = 0;
        private int _highScore;

        private int _cooldownInMilliseconds = 110;          //when going to other state we pause screen, so it would work
        private Stopwatch _cooldownWatch;

        private string[] _levelFileNames; // list of all levels file names
        private int _currentlevel = 0;
        private List<Level> _levels;

        public Game1()
        {
            _state = State.MENU;
            _graphics = new GraphicsDeviceManager(this);     //initialising the value in the constructor (Game1)
            _graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            _graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";          //the place from wich i take all content for my game
            _levelFileNames = Directory.GetFiles(@"Levels", "*.txt");       //list from names of diferent level
            LoadHighScore();
        }

        private void LoadLevels()           //from files loads levels into the list
        {
            try
            {
                //[11]
                _levels = new List<Level>();        //calls constructor in class Level
                foreach (var name in _levelFileNames)
                {
                    _levels.Add(new Level(name));
                }
                _levels.Sort();     //sorts levels by their difficulty
            }
            catch(OperationCanceledException e)
            {
                MessageBox.Show(e.Message, "Errortic massage", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Exit();
            }
        }

        //[5 - neprivalomasis]
        private void StartGame(bool preservePoints = false)         //starts new game, total points are 0
        {
            if (_levels == null)
                LoadLevels();
            _secondsAllowed = _levels[_currentlevel].TimeInSeconds;         //chooses witch level to load by the number of current level
            _wall = _levels[_currentlevel].Wall;
            _food = _levels[_currentlevel].Food;
            _hackMan = _levels[_currentlevel].HackMan;
            _ghost = _levels[_currentlevel].Ghost;
            _watch = null;          //since we haven't started a new game, so there is no time
            if (_currentlevel == _levels.Count - 1) _hackMan.MakeTrololo();
            if (!preservePoints)
                _points = 0;
            
            CollisionChecker = new CollisionChecker(_hackMan, _ghost, _food, _wall);            //checks wheather hackman hits walls or food
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

        private void LoadHighScore()
        {
            if (File.Exists(HIGH_SCORE_FILE_NAME))
            {
                var text = File.ReadAllText(HIGH_SCORE_FILE_NAME);
                _highScore = int.Parse(text);
            }
            else _highScore = 0;
        }

        private void WriteHighScore()
        {
            var x = new FileInfo(HIGH_SCORE_FILE_NAME);
            if (File.Exists(HIGH_SCORE_FILE_NAME))
            {
                x.IsReadOnly = false;
            }
            File.WriteAllText(HIGH_SCORE_FILE_NAME, _highScore.ToString());
            x.IsReadOnly = true;
        }
    }
}
