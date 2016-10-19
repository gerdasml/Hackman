using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameHackMan.Entities
{
    class HackMan : MoveableBlock
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HackMan(int x, int y)
            : base(x, y)
        {
            _texture[Direction.UP] = Graphics.HackManUp;
            _texture[Direction.DOWN] = Graphics.HackManDown;
            _texture[Direction.LEFT] = Graphics.HackManLeft;
            _texture[Direction.RIGHT] = Graphics.HackManRight;
            _texture[Direction.NONE] = Graphics.HackManFront;
            _speed = 1;
        }

        public void MakeTrololo()
        {
            _speed = -1;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update()
        {
            // TODO: Add your update logic here
            

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !Game1.CollisionChecker.IsPointInWall(new Vector2(_position.X, _position.Y - _speed)))
                _direction = Direction.UP;
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) && !Game1.CollisionChecker.IsPointInWall(new Vector2(_position.X, _position.Y + _speed)))
                _direction = Direction.DOWN;
            else if (Keyboard.GetState().IsKeyDown(Keys.Right) && !Game1.CollisionChecker.IsPointInWall(new Vector2(_position.X + _speed, _position.Y)))
                _direction = Direction.RIGHT;
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) && !Game1.CollisionChecker.IsPointInWall(new Vector2(_position.X - _speed, _position.Y)))
                 _direction = Direction.LEFT;

            Move();
        }
        
    }
}
