
using UnityEngine;
using UnityEngine.UI;

public class BossHpBarUI : MonoBehaviour
{
    [SerializeField] Image _hpBar;

    public void Init(float hp, float maxhp)
    {
        _hpBar.fillAmount = hp / maxhp;
    }
}
