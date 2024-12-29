using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class GachaHolder : MonoBehaviour
    {
        public AbilityController abilityController;
        public List<AbilitySO> abilities;
        public Transform storage;

        public void OpenGacha()
        {
            for (int i = abilities.Count - 1; i >= 0; i--)
            {
                if (abilityController.HasAbility(abilities[i]) && !abilities[i].IsObtainable)
                    abilities.RemoveAt(i);
            }
            gameObject.SetActive(true);
            Time.timeScale = 0f;
            var gachaButtons = storage.GetComponentsInChildren<Button>(true);
            foreach (var gachaButton in gachaButtons)
            {
                gachaButton.onClick.RemoveAllListeners();
                gachaButton.gameObject.SetActive(false);
            }
        
            var randomAbilities = GetRandomAbility(3);
            for (int i = 0; i < randomAbilities.Count; i++)
            {
                int idx = i;
                
                gachaButtons[idx].gameObject.SetActive(true);
                var item = gachaButtons[idx].GetComponent<GachaItem>();
                item.SetGachaItem(randomAbilities[idx].grade.selectionIcon, randomAbilities[idx].colorIcon);

                gachaButtons[idx].onClick.AddListener(() => OnClickGacha(randomAbilities[idx]));
            }
        }

        public List<AbilitySO> GetRandomAbility(int count)
        {
            var temp = new List<AbilitySO>(abilities);
            var result = new List<AbilitySO>();
            count = Mathf.Min(count, temp.Count);
            for (int i = 0; i < count; i++)
            {
                var index = Random.Range(0, temp.Count);
                result.Add(temp[index]);
                temp.RemoveAt(index);
            }
            return result;
        }
        
        public void OnClickGacha(AbilitySO ability)
        {
            Time.timeScale = 1f;
            abilityController.AddAbility(ability);
            CloseGacha();
        }

        private void CloseGacha()
        {
            gameObject.SetActive(false);
        }
    }
}
