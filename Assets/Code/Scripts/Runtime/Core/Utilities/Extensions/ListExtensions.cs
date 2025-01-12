using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    public static class ListExtensions
    {
        public static T GetRandom<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                throw new InvalidOperationException("The list is empty or null.");
            }

            int randomIndex = UnityEngine.Random.Range(0, list.Count);
            return list[randomIndex];
        }
    }
}
