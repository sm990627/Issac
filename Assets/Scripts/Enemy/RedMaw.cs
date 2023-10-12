using UnityEngine;

public class RedMaw : MonoBehaviour
{
    [Header("Àû ½ºÅÈ")]
    [SerializeField] float _moveSpeed = 3.0f;
    [SerializeField] float _hp = 5.0f;

    Rigidbody2D _rig;
    Transform _playerTf;
    GameObject _player;
    AttackCon _attcnt;
    SpriteRenderer _spriteRenderer;
    private void Start()
    {
        _rig = GetComponent<Rigidbody2D>();
        _player = GameObject.Find("PlayerHead");
        _attcnt = _player.GetComponent<AttackCon>();
        _playerTf = _player.transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector2 dirVector = (_playerTf.position - transform.position).normalized;
        _rig.velocity = dirVector * _moveSpeed;
    }

    void OnDamage()
    {
        _hp = _hp - _attcnt.GetPower();
        Debug.Log(GetComponent<SpriteRenderer>());
        _spriteRenderer.color = new Vector4(225 / 255f, 60 / 255f, 60 / 255f, 255 / 255f);
        Invoke("DamageEnd", 0.1f);
        if (_hp <= 0)
        {

            gameObject.SetActive(false);
            GenericSingleton<GameManager>.Instance.CheckState();
        }
    }

    void DamageEnd()
    {
        _spriteRenderer.color = Color.white;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            OnDamage();
        }
    }
}
