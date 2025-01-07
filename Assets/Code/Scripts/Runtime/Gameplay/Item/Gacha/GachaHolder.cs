using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class GachaHolder : MonoBehaviour
    {
        public AbilityController abilityController;
        public Transform storage;
        
        private List<AbilitySO> abilities;

        public void OpenGacha(List<AbilitySO> abilities)
        {
            this.abilities = abilities.ToList();
            Time.timeScale = 0f;
            Initialize();
            ResetButtons();
        }

        private void Initialize()
        {
            for (int i = abilities.Count - 1; i >= 0; i--)
            {
                if (abilityController.HasAbility(abilities[i]) && !abilities[i].IsObtainable)
                    abilities.RemoveAt(i);
            }
            gameObject.SetActive(true);
        }

        private void ResetButtons()
        {
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

        private List<AbilitySO> GetRandomAbility(int count)
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
        
        private void OnClickGacha(AbilitySO ability)
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
