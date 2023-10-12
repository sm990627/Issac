using UnityEngine;

public class testDoor : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GetComponent<Animator>().Play("DoorOpen");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GetComponent<Animator>().Play("DoorClose");
        }
    }
}
