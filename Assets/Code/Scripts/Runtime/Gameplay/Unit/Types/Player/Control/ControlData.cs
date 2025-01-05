using UnityEngine;

namespace Com2usGameDev
{
    public class ControlData
    {
        public bool controllable;
        public int velocityDirection;
        public bool groundValue;

        public ControlData()
        {
            controllable = true;
            velocityDirection = 0;
            groundValue = false;
        }
    }
}
