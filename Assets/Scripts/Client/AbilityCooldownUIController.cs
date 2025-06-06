using UnityEngine;
using UnityEngine.UI;
public class AbilityCooldownUIController : MonoBehaviour
{
    public static AbilityCooldownUIController Instance;
    
    [SerializeField] private Image aoeAbilityMask;
    [SerializeField] private Image skillShotAbilityMask;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        aoeAbilityMask.fillAmount = 0f;
        skillShotAbilityMask.fillAmount = 0f;
    }
    public void UpdateAoeMask(float fillAmount)
    {
        aoeAbilityMask.fillAmount = fillAmount;
    }
    public void UpdateSkillShotMask(float fillAmount)
    {
        skillShotAbilityMask.fillAmount = fillAmount;
    }
}