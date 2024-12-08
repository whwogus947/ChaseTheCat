using UnityEngine;

namespace Com2usGameDev
{
    public class StatAbility : IVisitableAbility
    {
        public int hp;

        public void Accept(IVisitorAbility visitorAbility)
        {
            visitorAbility.Visit(this);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
