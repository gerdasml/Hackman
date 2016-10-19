using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameHackMan.Entities
{
    abstract class MoveableBlock : Block
    {
        protected enum Direction
        {
            NONE,
            LEFT,
            RIGHT,
            UP,
            DOWN
        }
        protected Direction _direction;
        protected int _speed;
        protected new Dictionary<Direction, Texture2D> _texture;

        protected MoveableBlock(int x, int y)
            : base(x, y)
        {
            _texture = new Dictionary<Direction, Texture2D>();
            _direction = Direction.NONE;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture[_direction], destinationRectangle: new Rectangle((int)_position.X, (int)_position.Y, Game1.TILE_SIZE, Game1.TILE_SIZE));
        }

        protected void Move()
        {
            if (_direction == Direction.UP && !Game1.CollisionChecker.IsPointInWall(new Vector2(_position.X, _position.Y - _speed)))
                _position.Y -= _speed;
            else if (_direction == Direction.DOWN && !Game1.CollisionChecker.IsPointInWall(new Vector2(_position.X, _position.Y + _speed)))
                _position.Y += _speed;
            else if (_direction == Direction.RIGHT && !Game1.CollisionChecker.IsPointInWall(new Vector2(_position.X + _speed, _position.Y)))
                _position.X += _speed;
            else if (_direction == Direction.LEFT && !Game1.CollisionChecker.IsPointInWall(new Vector2(_position.X - _speed, _position.Y)))
                _position.X -= _speed;

            if (_position.X < 0) _position.X = Game1.SCREEN_WIDTH - 1;
            if (_position.Y < Game1.TILE_SIZE) _position.Y = Game1.SCREEN_HEIGHT - Game1.TILE_SIZE - 1;
            if (_position.X >= Game1.SCREEN_WIDTH) _position.X = 1;
            if (_position.Y >= Game1.SCREEN_HEIGHT - Game1.TILE_SIZE) _position.Y = Game1.TILE_SIZE;
        }

        protected void MoveGhost()
        {
            if (_direction == Direction.UP)
                _position.Y -= _speed;
            else if (_direction == Direction.DOWN)
                _position.Y += _speed;
            else if (_direction == Direction.RIGHT)
                _position.X += _speed;
            else if (_direction == Direction.LEFT)
                _position.X -= _speed;

            if (_position.X < 0) _position.X = Game1.SCREEN_WIDTH - 1;
            if (_position.Y < Game1.TILE_SIZE) _position.Y = Game1.SCREEN_HEIGHT - Game1.TILE_SIZE - 1;
            if (_position.X >= Game1.SCREEN_WIDTH) _position.X = 1;
            if (_position.Y >= Game1.SCREEN_HEIGHT - Game1.TILE_SIZE) _position.Y = Game1.TILE_SIZE;
        }

        public abstract void Update();
    }
}
