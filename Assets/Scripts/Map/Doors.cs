using UnityEngine;

public class Doors : GenericSingleton<Doors>
{
    public GameObject[] _doors;
    public void DoorOpen()
    {
        GetComponent<Animator>().Play("DoorOpen");
    } 
    public void DoorClose()
    {
        GetComponent<Animator>().Play("DoorClose");
    }
}
