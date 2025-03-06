using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    [System.Serializable]
    public class BookData : ISaveable
    {
        public Dictionary<string, List<int>> abilities = new();
    }
}
