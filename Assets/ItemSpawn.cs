
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    [SerializeField]GameObject[] _items;
    void Start()
    {

        foreach (GameObject item in _items)
        {
            item.SetActive(false);
        }
        _items[Random.Range(0, _items.Length)].SetActive(true);

    }

    
}
