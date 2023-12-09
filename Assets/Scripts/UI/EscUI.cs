using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class EscUI : MonoBehaviour
{
    
    [SerializeField] GameObject _item;
    [SerializeField] Transform _content;
    [SerializeField] RectTransform[] _menus;
    [SerializeField] RectTransform _arrow;
    [SerializeField] GameObject[] _speed;
    [SerializeField] GameObject[] _range;
    [SerializeField] GameObject[] _attackSpeed;
    [SerializeField] GameObject[] _bulletSpeed;
    [SerializeField] GameObject[] _damage;
    [SerializeField] GameObject[] _luck;
    int _currentIdx = 1;
    public void DrawItem(int i)
    {
        GameObject temp = Instantiate(_item,_content);
        temp.GetComponent<UIItemImage>().Init(i);
    }

    public void MoveArrowDown()
    {
        if (_currentIdx < _menus.Length - 1)
        {
            _currentIdx++;

        }
        UpdateArrowPosition();
    }

    public void MoveArrowUp()
    {
        if (_currentIdx > 0)
        {
            _currentIdx--;
        }
        UpdateArrowPosition();
    }

    public void EscUIinit()
    {
        _currentIdx = 1;
        UpdateArrowPosition();
    }
    public void UpdateArrowPosition()
    {
        _arrow.anchoredPosition = _menus[_currentIdx].anchoredPosition + new Vector2(150, 0);
    }

    public void CurrentUI()
    {
        switch (_currentIdx)
        {
            case 0:
                {
                    GenericSingleton<UIBase>.Instance.ShowOptionUI(true);
                }
                break;
            case 1:
                {
                    GenericSingleton<GameManager>.Instance.ResumeGame();
                }
                break;

            case 2:
                {
                    GenericSingleton<UIBase>.Instance.GoTitle();
                }
                break;
        }
    }
    public void Init()
    {
        Debug.Log("√ ±‚»≠");
        GameObject[] _items = GameObject.FindGameObjectsWithTag("ItemUI" );
        foreach (GameObject _item in _items)
        {            
            Destroy(_item);
        }
        foreach(var item in GenericSingleton<PlayerCon>.Instance.ItemIdx)
        {
            DrawItem(item);
        }
    }
    public void StatInit()
    {
        foreach (var speed in _speed)
        {
            speed.SetActive(false);   
        }
        int SpeedIdx = Mathf.FloorToInt(GenericSingleton<PlayerCon>.Instance.Pstat.Speed);
        for(int i = 0; i < SpeedIdx; i++)
        {
            _speed[i].SetActive(true);
        }

        foreach (var range in _range)
        {
            range.SetActive(false);
        }
        int RangeIdx = Mathf.FloorToInt(GenericSingleton<PlayerCon>.Instance.Pstat.Range/2);
        for (int i = 0; i < RangeIdx; i++)
        {
            _range[i].SetActive(true);
        }

        foreach (var attackSpeed in _attackSpeed)
        {
            attackSpeed.SetActive(false);
        }
        int ASIdx = Mathf.FloorToInt(1/GenericSingleton<PlayerCon>.Instance.Pstat.AttackSpeed);
        for (int i = 0; i <ASIdx; i++)
        {
            _attackSpeed[i].SetActive(true);
        }

        foreach (var bulletSpeed in _bulletSpeed)
        {
            bulletSpeed.SetActive(false);
        }
        int BSIdx = Mathf.FloorToInt(GenericSingleton<PlayerCon>.Instance.Pstat.BulletSpeed/2);
        for (int i = 0; i < BSIdx; i++)
        {
            _bulletSpeed[i].SetActive(true);
        }

        foreach (var damage in _damage)
        {
            damage.SetActive(false);
        }
        int DamageIdx = Mathf.FloorToInt(GenericSingleton<PlayerCon>.Instance.Pstat.Power);
        for (int i = 0; i < DamageIdx; i++)
        {
            _damage[i].SetActive(true);
        }

        foreach (var luck in _luck)
        {
            luck.SetActive(false);
        }
        int LuckIdx = GenericSingleton<PlayerCon>.Instance.Pstat.Luck;
        for (int i = 0; i < LuckIdx; i++)
        {
            _luck[i].SetActive(true);
        }
    }

}
