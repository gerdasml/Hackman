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
    class Food : Block
    {
        /// <summary>
        /// 
        /// </summary>
        public Food(int x, int y)
            : base (x, y)
        {
            _texture = Graphics.Food;
        }
    }
}
