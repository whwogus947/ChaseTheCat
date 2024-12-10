using UnityEngine;

namespace Com2usGameDev
{
    public static class TupleExtensions
    {
        public static int RandomValue(this (int min, int max)tuple)
        {
            return Random.Range(tuple.min, tuple.max);
        }
    }
}
