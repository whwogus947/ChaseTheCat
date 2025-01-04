using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    public class AbilityHolder<T> : MonoBehaviour where T : AbilitySO
    {
        public List<T> initialItems;
    }
}
