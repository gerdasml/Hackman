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
        private GraphicsDeviceManager _graphics;     //can change window size 
        private SpriteBatch _spriteBatch;        //will be used in the drawing {spriteBatch.Begin(); spriteBatch.Draw(); spriteBatch.Draw(); ..... spriteBatch.End();}
        private HackMan _hackMan;
        private SpriteFont _font;
        public static readonly int TILE_SIZE = 22;
        public static readonly int SCREEN_HEIGHT = 484 + TILE_SIZE;
        public static readonly int SCREEN_WIDTH = 506;
        internal static CollisionChecker CollisionChecker { get; private set; }
        private Stopwatch _watch;
        private int _secondsAllowed = 60;

        private List<Food> _food;
        private List<Wall> _wall;
        private int _points = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);     //initialising the value in the constructor (Game1)
            _graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            _graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";          //the place from wich i take all content for my game
            _wall = new List<Wall>();
            _food = new List<Food>();
            ReadMap();
            CollisionChecker = new CollisionChecker(_hackMan, _food, _wall);
        }

        private void ReadMap()
        {
            string[] map = System.IO.File.ReadAllLines(@"Levels/Level0.txt");

            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == 'P') _hackMan = new HackMan(x, y);
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
            _hackMan.Initialize();       //cals initialize method from class HackMan

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
            _hackMan.LoadContent(Content);      //we have to give some kind of value to method, so since that object is the whole game we give the method itself, in this case this
            foreach (var w in _wall)
            {
                w.LoadContent(Content);
            }
            foreach (var f in _food)
            {
                f.LoadContent(Content);
            }
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (_watch == null && (
                Keyboard.GetState().IsKeyDown(Keys.Up) ||
                Keyboard.GetState().IsKeyDown(Keys.Down) ||
                Keyboard.GetState().IsKeyDown(Keys.Left) ||
                Keyboard.GetState().IsKeyDown(Keys.Right)
                ))
                _watch = Stopwatch.StartNew();
            _hackMan.Update();
            _points += CollisionChecker.CheckFoodCollitions();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

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
            _spriteBatch.End();

            base.Draw(gameTime);
        }

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
                resultString += String.Format("{0:000}", _secondsAllowed);
            else
               resultString += String.Format("{0:000}", _secondsAllowed - _watch.ElapsedMilliseconds / 1000);
            Vector2 size = _font.MeasureString(resultString);
            Rectangle boundaries = new Rectangle(0, 0, SCREEN_WIDTH, TILE_SIZE);
            float xScale = boundaries.Width / size.X;
            float yScale = boundaries.Height / size.Y;
            float scale = Math.Min(xScale, yScale);
            var position = new Vector2(SCREEN_WIDTH - size.X * scale, SCREEN_HEIGHT - TILE_SIZE);
            _spriteBatch.DrawString(_font, resultString, position, Color.Green, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }
    }
}
