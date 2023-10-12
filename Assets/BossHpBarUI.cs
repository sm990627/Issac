
using UnityEngine;
using UnityEngine.UI;

public class BossHpBarUI : MonoBehaviour
{
    [SerializeField] Image _hpBar;

    public void Init()
    {
        _hpBar.fillAmount = GenericSingleton<Monstro>.Instance.HP / GenericSingleton<Monstro>.Instance.MaxHP;
    }
}
