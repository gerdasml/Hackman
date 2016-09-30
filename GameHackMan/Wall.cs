using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHackMan
{
    class Wall
    {
        private readonly Vector2 _position;
        private static Texture2D _texture;

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public Wall(int x, int y)
        {
            _position = new Vector2();
            _position.X = x * Game1.TILE_SIZE;
            _position.Y = y * Game1.TILE_SIZE;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            if (_texture == null) _texture = content.Load<Texture2D>("Wall");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, destinationRectangle: new Rectangle((int)_position.X, (int)_position.Y, Game1.TILE_SIZE, Game1.TILE_SIZE));
        }
    }
}
