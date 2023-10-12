
using UnityEngine;

public class StoneScript : MonoBehaviour
{
    [SerializeField] Sprite[] _sprites; 
    SpriteRenderer _spriteRenderer;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
    }

    void Update()
    {
        
    }
}
