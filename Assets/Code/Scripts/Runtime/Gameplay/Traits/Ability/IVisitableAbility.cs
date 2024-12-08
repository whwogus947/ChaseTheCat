using UnityEngine;

namespace Com2usGameDev
{
    public interface IVisitableAbility
    {
        void Accept(IVisitorAbility visitorAbility);
    }

    public interface IVisitorAbility
    {
        void Visit(SkillAbility skill);
        void Visit(StatAbility stat);
        void Visit(WeaponAbility weapon);
    }
}
