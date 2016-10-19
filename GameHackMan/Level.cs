using GameHackMan.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GameHackMan
{
    //[13]
    class Level : IComparable<Level>
    {
        private static readonly int TIME_MULTIPLIER = 420;
        private static readonly int FOOD_MULTIPLIER = 69;

        private HackMan _hackMan;
        private List<Ghost> _ghost;
        private List<Food> _food;
        private List<Wall> _wall;
        private Color _wallColor;

        //[9]
        private string _pattern = @"^[0-9]+\n[0-9]+\s[0-9]+\s[0-9]+\n([#\s\.PG]{23}\n?){22}$";

        //[4 - standartinis]
        public HackMan HackMan
        {
            get
            {
                return (HackMan)_hackMan.Clone();
            }
        }

        public List<Ghost> Ghost
        {
            get
            {
                return _ghost.CloneAll();
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
                //[10-pleciantis]
                var difficulty = (double)TIME_MULTIPLIER / TimeInSeconds + (double)Food.Count / FOOD_MULTIPLIER;
                if (_wallColor.R == 0 && _wallColor.G == 0 && _wallColor.B == 0)
                    difficulty *= 100;
                return difficulty;
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
            _ghost = new List<Ghost>();
            ReadMap(fileName); // as the argument we give the name of the level
        }


        private void ReadMap(string fileName) 
        {
            //[7]
            string[] map = System.IO.File.ReadAllLines(fileName); // read simbols from file
            if (!IsValidMap(map)) throw new OperationCanceledException("Map in the file "+fileName+" is invalidas");
            TimeInSeconds = int.Parse(map[0]);   //converting from srtring into int
            var rgb = map[1].Split(' ');
            _wallColor = new Color(int.Parse(rgb[0]), int.Parse(rgb[1]), int.Parse(rgb[2]));
            for (int y = 2; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == 'P') _hackMan = new HackMan(x, y - 1);
                    else if (map[y][x] == '.') _food.Add(new Food(x, y - 1));
                    else if (map[y][x] == '#') _wall.Add(new Wall(x, y - 1, _wallColor));
                    else if (map[y][x] == 'G')
                    {
                        _ghost.Add(new Ghost(x, y - 1));
                        _food.Add(new Food(x, y - 1));
                    }
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
