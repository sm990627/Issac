using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HpBarCon : MonoBehaviour
{
    float _maxHp;
    float _maxTotalHp;
    float _hp;
    GameObject[] _hpBar;
    Image[] _hpFills;
    [SerializeField] Transform _hpParent;
    [SerializeField] GameObject _hpBarPrefap;


    public void Init()
    {
        _maxTotalHp = GenericSingleton<PlayerCon>.Instance.Pstat.MaxTotalHp;
        _maxHp = GenericSingleton<PlayerCon>.Instance.Pstat.MaxHp;
        _hp = GenericSingleton<PlayerCon>.Instance.Pstat.Hp;

        _hpBar = new GameObject[(int)_maxTotalHp];
        _hpFills = new Image[(int)_maxTotalHp];
        InstantiateHpBar();

        MaxHpCon();
        HpCon();
        
    }

    public void HpUpdate()
    {
        _maxTotalHp = GenericSingleton<PlayerCon>.Instance.Pstat.MaxTotalHp;
        _maxHp = GenericSingleton<PlayerCon>.Instance.Pstat.MaxHp;
        _hp = GenericSingleton<PlayerCon>.Instance.Pstat.Hp;
        MaxHpCon();
        HpCon();
    }
    public void MaxHpCon()
    {
        if (_maxHp > _maxTotalHp) _maxHp = _maxTotalHp;
        for (int i = 0; i < _hpBar.Length; i++)
        {
            if (i < _maxHp)
            {
                _hpBar[i].SetActive(true);
            }
            else
            {
                _hpBar[i].SetActive(false);
            }
        }
    }

    public void HpCon()
    {
        if (_hp > _maxHp) _hp = _maxHp;
        for (int i = 0; i < _hpFills.Length; i++)
        {
            if (i < _hp)
            {
                _hpFills[i].fillAmount = 1;
            }
            else
            {
                _hpFills[i].fillAmount = 0;

            }
            if (_hp % 1 != 0)
            {
                int lastPos = Mathf.FloorToInt(_hp);
                _hpFills[lastPos].fillAmount = _hp % 1;
            }
        }

    }
    void InstantiateHpBar()
    {
        for (int i = 0; i < _maxTotalHp; i++)
        {
            GameObject temp = Instantiate(_hpBarPrefap);
            temp.transform.SetParent(_hpParent, false);
            _hpBar[i] = temp;
            _hpFills[i] = temp.transform.Find("HpFill").GetComponent<Image>();
        }
    }
   
}
