
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    [SerializeField]GameObject[] _items;
    [SerializeField]GameObject  _test;
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            _test.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            _test.SetActive(false);
        }
    }


}
