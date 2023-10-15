using UnityEngine;

public class Horf : MonoBehaviour
{
    [Header("Àû ½ºÅÈ")]
    [SerializeField] float _hp = 5.0f;
    [SerializeField] float _attackSpeed = 3f;
    [SerializeField] float _bulletSpeed = 6.0f;
    [SerializeField] GameObject _bullet;
    int _poolIndex;
    bool _inAttack = false;
    Transform _playerTf;
    Animator _animator;
    GameObject player;
    AttackCon attcnt;
    SpriteRenderer _spriteRenderer;
    private void Start()
    {
        player = GameObject.Find("PlayerHead");
        attcnt = player.GetComponent<AttackCon>();
        _animator = GetComponent<Animator>();
        _playerTf = player.transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("StopAttack", _attackSpeed);
    }

    void Update()
    {
        if (_inAttack)
        {
            _inAttack = false;
            _animator.SetBool("inAttack", false);
            Invoke("StopAttack", _attackSpeed);
        }
        
    }
    void OnDamage()
    {
        _hp = _hp - attcnt.GetPower();
        _spriteRenderer.color = new Vector4(225/255f, 60/255f, 60/255f, 255/255f);
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
    void Attack()
    {
        //»ý¼º À§Ä¡ º¤ÅÍ
        Vector3 VI = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        GameObject temp = Instantiate(_bullet, VI, Quaternion.identity);
        temp.GetComponent<EnemyBulletCon>().SetOwner(gameObject.name);
        Rigidbody2D rb = temp.GetComponent<Rigidbody2D>();
        //¹ß»ç º¤ÅÍ  
        Vector2 dirVector = (_playerTf.position - transform.position).normalized;
        rb.AddForce(dirVector * _bulletSpeed, ForceMode2D.Impulse);

    }
    void StopAttack()
    {
        _inAttack = true;
        _animator.SetBool("inAttack", true);
    }



}