using GameHackMan.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHackMan
{
    class Ghost : MoveableBlock
    {
        private Dictionary<Direction, Vector2> _delta = new Dictionary<Direction, Vector2>();
        private static Random _random = new Random();

        public Ghost(int x, int y)
            : base(x, y)
        {
            _texture[Direction.UP] = Graphics.GhostUp;
            _texture[Direction.DOWN] = Graphics.GhostDown;
            _texture[Direction.LEFT] = Graphics.GhostLeft;
            _texture[Direction.RIGHT] = Graphics.GhostRight;
            _texture[Direction.NONE] = Graphics.GhostFront;
            _speed = 1;
            _delta[Direction.UP] = new Vector2(0, -_speed);
            _delta[Direction.DOWN] = new Vector2(0, _speed);
            _delta[Direction.RIGHT] = new Vector2(_speed, 0);
            _delta[Direction.LEFT] = new Vector2(-_speed, 0);
        }

        public override void Update()
        {
            // TODO: Add your update logic here

            while (_direction == Direction.NONE || Game1.CollisionChecker.IsPointInWall(new Vector2(_position.X + _delta[_direction].X, _position.Y + _delta[_direction].Y)))
            {
                var v = Enum.GetValues(typeof(Direction));
                _direction = (Direction)v.GetValue(_random.Next(v.Length));
                
            }
            MoveGhost();
        }
    }
}
