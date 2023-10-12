using UnityEngine;

public class EscUI : MonoBehaviour
{
    
    [SerializeField] GameObject _item;
    [SerializeField] Transform _content;

    public void DrawItem(int i)
    {
        GameObject temp = Instantiate(_item,_content);
        temp.GetComponent<UIItemImage>().Init(i);
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
