
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
        if (GenericSingleton<StageManager>.Instance.CurrentRoom.TreasureIdx != -1)
        {
            _items[GenericSingleton<StageManager>.Instance.CurrentRoom.TreasureIdx].SetActive(true);
        }

    }

    
}
