using UnityEngine;

namespace Com2usGameDev
{
    public interface IVisitable
    {
        void Accept(IVisitor visitor);
    }
}
