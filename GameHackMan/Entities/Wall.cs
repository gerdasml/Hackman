using Microsoft.Xna.Framework;

namespace GameHackMan.Entities
{
    class Wall : Block
    {
        /// <summary>
        /// 
        /// </summary>
        public Wall(int x, int y, Color color)
            : base(x, y)
        {
            _texture = Graphics.Wall;
            _color = color;
        }
    }
}
