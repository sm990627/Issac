using UnityEngine;


public enum ExitDirection
{
    right,
    left,
    down,
    up,
}

public class Gate : MonoBehaviour
{
    [SerializeField] ExitDirection dir;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GenericSingleton<StageManager>.Instance.GetComponent<StageManager>().ChangeScene(dir);
        }
    }
    
}
