using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHackMan
{
    class HackMan
    {
        enum Direction
        {
            LEFT,
            RIGHT,
            UP,
            DOWN
        }
        private Direction _direction;
        private Dictionary<Direction, Texture2D> _textures;
        private Vector2 _position;
        private int _speed = 1;

        /// <summary>
        /// Here is a property that returns a rectangle which surounds our object.
        /// It will be needed for the moethod CheckCollisions() in class Game1
        /// </summary>
        public Rectangle BoundingBox
        {
            get
            {
                Rectangle rectangle = new Rectangle((int)_position.X, (int)_position.Y, Game1.TILE_SIZE, Game1.TILE_SIZE);
                return rectangle;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HackMan(int x, int y)
        {
            _textures = new Dictionary<Direction, Texture2D>();
            _position = new Vector2();
            _position.X = x * Game1.TILE_SIZE;
            _position.Y = y * Game1.TILE_SIZE;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public void Initialize()
        {
            // TODO: Add your initialization logic here
            //_position = Vector2.Zero;        //position = new Vector2(0,0);
            _direction = Direction.RIGHT;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent(ContentManager content)       //since Content cannot be seen from class Game, so we have to give a perameter to fix this problem
        {
            // TODO: use this.Content to load your game content here
            _textures.Add(Direction.RIGHT, content.Load<Texture2D>("Hackman_right"));
            _textures.Add(Direction.LEFT, content.Load<Texture2D>("Hackman_left"));
            _textures.Add(Direction.UP, content.Load<Texture2D>("Hackman_up"));
            _textures.Add(Direction.DOWN, content.Load<Texture2D>("Hackman_down"));
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update()
        {
            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.Up) == true)
                _direction = Direction.UP;
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) == true)
                _direction = Direction.DOWN;
            else if (Keyboard.GetState().IsKeyDown(Keys.Right) == true)
                _direction = Direction.RIGHT;
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) == true)
                 _direction = Direction.LEFT;

            if (_direction == Direction.UP)
                _position.Y -= _speed;
            else if (_direction == Direction.DOWN)
                _position.Y += _speed;
            else if (_direction == Direction.RIGHT)
                _position.X += _speed;
            else if (_direction == Direction.LEFT)
                _position.X -= _speed;

            if (_position.X < 0) _position.X = Game1.SCREEN_WIDTH;
            if (_position.Y < 0) _position.Y = Game1.SCREEN_HEIGHT;
            if (_position.X > Game1.SCREEN_WIDTH) _position.X = 0;
            if (_position.Y > Game1.SCREEN_HEIGHT) _position.Y = 0;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Add your drawing code here
            spriteBatch.Draw(_textures[_direction], destinationRectangle: new Rectangle((int)_position.X, (int)_position.Y, Game1.TILE_SIZE, Game1.TILE_SIZE));
        }
    }
}
