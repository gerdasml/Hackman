﻿using Microsoft.Xna.Framework;
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
    public partial class Game1 : Game
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
        private int _secondsAllowed;
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
            _secondsAllowed = _levels[_currentlevel].TimeInSeconds;
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
    }
}
