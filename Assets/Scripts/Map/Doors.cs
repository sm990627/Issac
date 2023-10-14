using UnityEngine;

public class Doors : GenericSingleton<Doors>
{
    public GameObject[] _doors;
    bool _isOpen;
    public void DoorOpen()
    {
        if (!_isOpen)
        {
            GetComponent<Animator>().Play("DoorOpen");
            _isOpen = true;
        }
    } 
    public void DoorClose()
    {
        if (_isOpen)
        {
            GetComponent<Animator>().Play("DoorClose");
            _isOpen = false;
        }
    }
    public void TrapDoorOn()
    {
        _doors[4].SetActive(true);
    }
}
