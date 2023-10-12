using UnityEngine;

public class EnemyBulletCon : MonoBehaviour
{
    void Start()
    {
       Destroy(gameObject,2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
    
  
}
