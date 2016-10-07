using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameHackMan
{
    class Level : IComparable<Level>
    {
        private static readonly int TIME_MULTIPLIER = 420;
        private static readonly int FOOD_MULTIPLIER = 69;

        private HackMan _hackMan;
        private List<Food> _food;
        private List<Wall> _wall;

        private string _pattern = @"^[0-9]*\n([#\s\.P]{23}\n?){22}$";

        public HackMan HackMan
        {
            get
            {
                return (HackMan)_hackMan.Clone();
            }
        }
        public List<Food> Food
        {
            get
            {
                return _food.CloneAll();
            }
        }
        
        public List<Wall> Wall
        {
            get
            {
                return _wall;
            }
        }
        public int TimeInSeconds { get; private set; }

        public double Difficulty
        {
            get
            {
                return (double)TIME_MULTIPLIER / TimeInSeconds + (double)Food.Count/ FOOD_MULTIPLIER;
            }
        }

        private bool IsValidMap(string[] map)
        {
            string joinedMap = string.Join("\n", map);
            return joinedMap.Split('P').Length - 1 == 1 && Regex.IsMatch(joinedMap, _pattern);
        }

        public Level(string fileName)
        {
            _wall = new List<Wall>();
            _food = new List<Food>();
            ReadMap(fileName); // perduodam ta mum paduota failo varda
        }

        private void ReadMap(string fileName) // i cia
        {

            string[] map = System.IO.File.ReadAllLines(fileName); // ir tada is to failo skaitom
            if (!IsValidMap(map)) throw new OperationCanceledException("Map in the file "+fileName+" is invalidas");
            //reikes validuot su regex cia
            TimeInSeconds = int.Parse(map[0]);   //konvertuojam is stringo i inta
            for (int y = 1; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == 'P') _hackMan = new HackMan(x, y-1); // sukuriam nauja objekta is klases HackMan
                    else if (map[y][x] == '.') _food.Add(new Food(x, y-1));
                    else if (map[y][x] == '#') _wall.Add(new Wall(x, y-1));
                }
            }
        }

        public int CompareTo(Level other)
        {
            if (Difficulty < other.Difficulty)
                return -1;
            if (Difficulty > other.Difficulty)
                return 1;
            return 0;
        }
        
    }
}
