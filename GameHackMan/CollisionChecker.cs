using GameHackMan.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GameHackMan
{
    //[2]
    struct CollisionChecker
    {
        private HackMan _hackMan;
        private List<Ghost> _ghost;
        private List<Food> _food;
        private List<Wall> _wall;

        public CollisionChecker(HackMan hackMan, List<Ghost> ghost, List<Food> food, List<Wall> wall)
        {
            _hackMan = hackMan;
            _ghost = ghost;
            _food = food;
            _wall = wall;
        }

        public bool IsHackmanDead()
        {
            foreach (var g in _ghost)
            {
                if (g.BoundingBox.Intersects(_hackMan.BoundingBox)) return true;
            }
            return false;
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
                }
            }
            return 0;
        }
    }
}
