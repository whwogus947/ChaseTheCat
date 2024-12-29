using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Com2usGameDev
{
    public class TutorialGuidance : MonoBehaviour
    {
        public float countdown;

        void Start()
        {
            if (countdown > 0)
                SetInactiveAfter(countdown).Forget();

            var npcList = FindObjectsByType<NPCBehaviour>(FindObjectsSortMode.None);
            foreach (var npc in npcList)
            {
                npc.AddEvent(() => gameObject.SetActive(false));
            }
        }

        private async UniTaskVoid SetInactiveAfter(float timer)
        {
            await UniTask.WaitForSeconds(timer);
            if (gameObject != null)
                gameObject.SetActive(false);
        }
    }
}
