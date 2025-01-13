using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    [System.Serializable]
    public class MapEnemyProvider
    {
        public int maxCount = 5;
        public List<MonsterBehaviour> monsters;
    }
}
