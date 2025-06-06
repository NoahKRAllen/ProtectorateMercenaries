using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class AbilityCooldownUIController : MonoBehaviour
    {
        public static AbilityCooldownUIController _instance;
    
        [SerializeField] private Image qAbilityMask;
        [SerializeField] private Image eAbilityMask;
        [SerializeField] private Image rAbilityMask;
        private void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }
        private void Start()
        {
            qAbilityMask.fillAmount = 0f;
            eAbilityMask.fillAmount = 0f;
            rAbilityMask.fillAmount = 0f;
        }
        public void UpdateQAbilityMask(float fillAmount)
        {
            qAbilityMask.fillAmount = fillAmount;
        }
        public void UpdateEAbilityMask(float fillAmount)
        {
            eAbilityMask.fillAmount = fillAmount;
        }
        public void UpdateRAbilityMask(float fillAmount)
        {
            rAbilityMask.fillAmount = fillAmount;
        }
    }
}
