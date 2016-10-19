namespace GameHackMan.Entities
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
