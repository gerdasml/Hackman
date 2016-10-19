using System;
using System.Collections.Generic;

namespace GameHackMan
{
    static class ListExtensions
    {

        /// <summary>
        /// Extension method to return a new list, which contains clones of the elements of the initial list
        /// </summary>
        // [6], [8]
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
