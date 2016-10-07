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
    class HackMan : ICloneable
    {
        enum Direction
        {
            NONE,
            LEFT,
            RIGHT,
            UP,
            DOWN
        }
        private Direction _direction;
        private static Dictionary<Direction, Texture2D> _textures = new Dictionary<Direction, Texture2D>();
        // ^ sitas nebuvo static pries tai
        /*vadinas jis priklause konkreciam objektui. todel kuriant vis nauja objekta, reiktu vis is naujo
        i sita lauka uzkraut paveiksliukus

            dabar kai yra padaryta static, tai sitas laukas bendras VISIEM SITOS KLASES objektam
            todel nesvarbu kursi tu 1 ar 100 hackman objektu, visi jie dalinsis situo lauku

        */
        private Vector2 _position;
        private int _speed = 1;
        private Direction _previousDirection;

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
            _position = new Vector2();
            _position.X = x * Game1.TILE_SIZE;
            _position.Y = y * Game1.TILE_SIZE;
            _direction = Direction.NONE;
            _previousDirection = Direction.NONE;
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public static void LoadContent(ContentManager content)       //since Content cannot be seen from class Game, so we have to give a perameter to fix this problem
        {
            // TODO: use this.Content to load your game content here
            if (_textures.Count == 0)
            {
                _textures.Add(Direction.RIGHT, content.Load<Texture2D>("Hackman_right"));
                _textures.Add(Direction.LEFT, content.Load<Texture2D>("Hackman_left"));
                _textures.Add(Direction.UP, content.Load<Texture2D>("Hackman_up"));
                _textures.Add(Direction.DOWN, content.Load<Texture2D>("Hackman_down"));
                _textures.Add(Direction.NONE, content.Load<Texture2D>("Hackman"));
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update()
        {
            // TODO: Add your update logic here
            _previousDirection = _direction;

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !Game1.CollisionChecker.IsPointInWall(new Vector2(_position.X, _position.Y - _speed)))
                _direction = Direction.UP;
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) && !Game1.CollisionChecker.IsPointInWall(new Vector2(_position.X, _position.Y + _speed)))
                _direction = Direction.DOWN;
            else if (Keyboard.GetState().IsKeyDown(Keys.Right) && !Game1.CollisionChecker.IsPointInWall(new Vector2(_position.X + _speed, _position.Y)))
                _direction = Direction.RIGHT;
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) && !Game1.CollisionChecker.IsPointInWall(new Vector2(_position.X - _speed, _position.Y)))
                 _direction = Direction.LEFT;

            Move();
            Console.WriteLine("{0} {1}", _position.X, _position.Y);
        }

        private void Move ()
        {
            if (_direction == Direction.UP && !Game1.CollisionChecker.IsPointInWall(new Vector2(_position.X, _position.Y - _speed)))
                _position.Y -= _speed;
            else if (_direction == Direction.DOWN && !Game1.CollisionChecker.IsPointInWall(new Vector2(_position.X, _position.Y + _speed)))
                _position.Y += _speed;
            else if (_direction == Direction.RIGHT && !Game1.CollisionChecker.IsPointInWall(new Vector2(_position.X + _speed, _position.Y)))
                _position.X += _speed;
            else if (_direction == Direction.LEFT && !Game1.CollisionChecker.IsPointInWall(new Vector2(_position.X - _speed, _position.Y)))
                _position.X -= _speed;

            if (_position.X < 0) _position.X = Game1.SCREEN_WIDTH-1;
            if (_position.Y < 0) _position.Y = Game1.SCREEN_HEIGHT - Game1.TILE_SIZE -1;
            if (_position.X >= Game1.SCREEN_WIDTH) _position.X = 1;
            if (_position.Y >= Game1.SCREEN_HEIGHT - Game1.TILE_SIZE) _position.Y = 1;
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

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
