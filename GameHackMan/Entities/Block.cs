using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameHackMan.Entities
{
    //[13]
    abstract class Block : ICloneable
    {
        protected Vector2 _position;
        protected Texture2D _texture;
        protected Color _color = Color.White;

        public Rectangle BoundingBox
        {
            get
            {
                //[10-siaurinanti]
                Rectangle rectangle = new Rectangle((int)_position.X, (int)_position.Y, Game1.TILE_SIZE, Game1.TILE_SIZE);
                return rectangle;
            }
        }

        protected Block(int x, int y)
        {
            _position = new Vector2();
            _position.X = x * Game1.TILE_SIZE;
            _position.Y = y * Game1.TILE_SIZE;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, destinationRectangle: new Rectangle((int)_position.X, (int)_position.Y, Game1.TILE_SIZE, Game1.TILE_SIZE), color: _color);
        }

        public object Clone()
        {
            return MemberwiseClone();       //shallow cloning
        }
    }
}
