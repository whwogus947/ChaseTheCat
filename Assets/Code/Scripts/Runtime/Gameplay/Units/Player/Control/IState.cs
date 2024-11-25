using UnityEngine;

namespace Com2usGameDev
{
    public interface IState
    {
        void OnEnter(UnitBehaviour unitStat);
        void OnUpdate(UnitBehaviour unitStat);
        void OnExit(UnitBehaviour unitStat);
    }
}
