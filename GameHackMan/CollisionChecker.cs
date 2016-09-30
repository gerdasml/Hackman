using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHackMan
{
    class CollisionChecker
    {
        private HackMan _hackMan;
        private List<Food> _food;
        private List<Wall> _wall;

        public CollisionChecker(HackMan hackMan, List<Food> food, List<Wall> wall)
        {
            _hackMan = hackMan;
            _food = food;
            _wall = wall;
        }

        public bool IsPointInWall(Vector2 point)
        {
            var rectangle = new Rectangle((int)point.X, (int)point.Y, Game1.TILE_SIZE, Game1.TILE_SIZE);
            foreach (var w in _wall)
            {
                if (w.BoundingBox.Intersects(rectangle))
                    return true;
            }
            return false;
        }
        public int CheckFoodCollitions()
        {
            foreach (var f in _food)
            {
                if (f.BoundingBox.Intersects(_hackMan.BoundingBox))
                {
                    _food.Remove(f);
                    return 1;
                    //_points++;
                    //Console.WriteLine(_points);
                    //break;
                }
            }
            return 0;
        }
    }
}
