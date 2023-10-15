using UnityEngine;

public class EnemyBulletCon : MonoBehaviour
{
    string _enemyName; 
    void Start()
    {
       Destroy(gameObject,2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    public void SetOwner(string name)
    {
        _enemyName = name;
    }
    public string GetName()
    {
        return _enemyName;
    }
}
