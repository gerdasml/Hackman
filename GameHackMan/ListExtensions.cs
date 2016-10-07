using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHackMan
{
    static class ListExtensions
    {
        public static List<T> CloneAll<T>(this List<T> list) where T : ICloneable
        {
            var temp = new List<T>();
            foreach (var f in list)
            {
                temp.Add((T)f.Clone());
            }
            return temp;
        }
    }
}
