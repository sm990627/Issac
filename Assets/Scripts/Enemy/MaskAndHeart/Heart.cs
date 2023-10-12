using System.Collections.Generic;
using UnityEngine;


public class Heart : MonoBehaviour
{

    [SerializeField] float _moveSpeed = 2;
    [SerializeField] float _hp = 5;
    [SerializeField] float _attackSpeed = 3f;
    [SerializeField] float _bulletSpeed = 6.0f;
    [SerializeField] GameObject _bullet;
    Rigidbody2D _rig;
    Transform _playerTf;
    GameObject _player;
    AttackCon _attcnt;
    bool _isEscape = false;
    Vector2 dirVector;
    bool _inAttack = false;
    SpriteRenderer _spriteRender;

    private void Start()
    {
       
        _rig =GetComponent<Rigidbody2D>();
        _player = GameObject.Find("PlayerHead");
        _attcnt = _player.GetComponent<AttackCon>();
        _playerTf = _player.transform;
        Invoke("StopAttack", _attackSpeed);
        _spriteRender = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        FindDirection();
        
        if (_inAttack)
        {
            Attack();
            _inAttack = false;
            Invoke("StopAttack", _attackSpeed);
        }
        _rig.velocity = dirVector * _moveSpeed;

    }
    void OnDamage()
    {
        _hp = _hp - _attcnt.GetPower();
        _spriteRender.color = new Vector4(225 / 255f, 60 / 255f, 60 / 255f, 255 / 255f);


        if (_hp <= 0)
        {
            Destroy(gameObject);

        }
        Invoke("DamageEnd", 0.1f);
    }
    
    void DamageEnd()
    {
        _spriteRender.color = Color.white;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            OnDamage();
        }
    }
    void FindDirection()
    {
        float _poX = CalDistance(Vector3.right);
        float _poY = CalDistance(Vector3 .up);
        float _naX = CalDistance(Vector3.left);
        float _naY = CalDistance(Vector3.down);
        
        float _quadrant1 = _poX * _poY;
        float _quadrant2 = _naX * _poY;
        float _quadrant3 = _naX * _naY;
        float _quadrant4 = _poX * _naY;
        float _angle = GetAngle(transform.position,_playerTf.position);
        int idx = 0;
        float _minWidth = float.MaxValue;
        List<float> quadrants = new List<float>() {_quadrant1, _quadrant2, _quadrant3, _quadrant4};
        for (int i = 0; i < quadrants.Count; i++)
        {
            if (_minWidth > quadrants[i])
            {
                _minWidth = quadrants[i];
                idx = i;
            }
        }

        if (!_isEscape)
        {
            _isEscape = true;
            Invoke("CalDelay", 0.3f);
            if (_minWidth < 1)
            {
                switch (idx)
                {
                    case 0:
                        {
                            if (_angle < -135)
                            {
                                dirVector = Vector2.down;
                            }
                            else
                            {
                                dirVector = Vector2.left;
                            }
                            break;
                        }
                    case 1:
                        {
                            if (_angle < -45)
                            {
                                dirVector = Vector2.right;
                            }
                            else
                            {
                                dirVector = Vector2.down;
                            }
                            break;
                        }
                    case 2:
                        {
                            if (_angle < 45)
                            {
                                dirVector = Vector2.up;
                            }
                            else
                            {
                                dirVector = Vector2.right;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (_angle < 135)
                            {
                                dirVector = Vector2.left;
                            }
                            else
                            {
                                dirVector = Vector2.up;
                            }
                            break;
                        }
                }

            }
            else
            {
                dirVector = -(_playerTf.position - transform.position).normalized;

            }

        }

       

    }
    float CalDistance(Vector3 direction)
    {
        RaycastHit2D hitData;
        hitData = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, 1 << LayerMask.NameToLayer("Wall"));
        float _distance = hitData.distance;
        return _distance;
    }

    float GetAngle(Vector2 v1, Vector2 v2)
    {
        float angle;
        float dx = v2.x - v1.x;
        float dy = v2.y - v1.y;
        float rad = Mathf.Atan2(dy, dx);
        angle = Mathf.Rad2Deg * rad;
        return angle;
    }

    void Attack()
    {
        //생성 위치 벡터
        Vector3 VI = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        GameObject[] _FirePool = new GameObject[4];
        float ang = 0;
        for (int i = 0; i < _FirePool.Length; i++)
        {
            GameObject temp = Instantiate(_bullet, VI, Quaternion.identity);
            Rigidbody2D rb = temp.GetComponent<Rigidbody2D>();
            _FirePool[i] = temp;
            Vector3 direction = Quaternion.AngleAxis(ang, Vector3.forward) * Vector3.right;
            rb.AddForce(direction * _bulletSpeed, ForceMode2D.Impulse);
            ang -= 90;
        }


    }
    void CalDelay()
    {
        _isEscape = false;
    }
    void StopAttack()
    {
        _inAttack = true;
    }
   

}