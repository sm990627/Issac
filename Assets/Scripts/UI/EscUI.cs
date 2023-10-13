using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class EscUI : MonoBehaviour
{
    
    [SerializeField] GameObject _item;
    [SerializeField] Transform _content;
    [SerializeField] RectTransform[] _menus;
    [SerializeField] RectTransform _arrow;
    int _currentIdx = 0;
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

    public void UpdateArrowPosition()
    {
        _arrow.anchoredPosition = _menus[_currentIdx].anchoredPosition + new Vector2(150, 0);
    }

    public void CurrentUI()
    {
        switch (_currentIdx)
        {
            case 0:
                GenericSingleton<UIBase>.Instance.ShowOptionUI(true);
                break;
        }
    }
    public void Init()
    {
        GameObject[] _items = GameObject.FindGameObjectsWithTag("ItemUI");
        foreach (GameObject _item in _items)
        {
            Destroy(_item);
        }
    }

}
