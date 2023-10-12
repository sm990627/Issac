using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour where T : class
{
    private static T instance;
    public static T Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<T>();
            DontDestroyOnLoad(gameObject);
            OnAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    protected virtual void OnAwake() { }
}