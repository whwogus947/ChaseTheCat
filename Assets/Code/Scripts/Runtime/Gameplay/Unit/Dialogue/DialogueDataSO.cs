using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Dialogue Data", menuName = "Cum2usGameDev/NPC/Dialogue")]
    public class DialogueDataSO : ScriptableObject
    {
        public DialogueText[] dialogueTexts;
        public string CurrentMessage => dialogueTexts[index].textMessage;
        public NPCTypeSO NextNpc => dialogueTexts[Mathf.Min(index + 1, dialogueTexts.Length - 1)].npc;

        private int index = -1;

        public void Reset() => index = -1;

        public bool TryNext()
        {
            if (index < dialogueTexts.Length - 1)
            {
                index++;
                return true;
            }
            return false;
        }
    }
}
