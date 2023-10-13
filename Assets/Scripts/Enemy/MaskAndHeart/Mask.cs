
using UnityEngine;

public class Mask : MonoBehaviour
{
    [SerializeField] GameObject _heart;
    [SerializeField] float _moveSpeed = 1;
    [SerializeField] float _dashSpeed = 3;
    Vector2[] directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    Vector2 moveVector;
    Rigidbody2D _rig;
    Transform _player;
    bool _isChase = false;
    Animator _animator;
    void Start()
    {
        GameObject player = GameObject.Find("PlayerHead");
        _player = player.transform;
        _rig = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        moveVector = directions[Random.Range(0, 4)];
    }

    void Update()
    {
        if (_heart != null)
        {
            ChangeDirection();
            ChasePlayer();
            AnimeCon();
            _rig.velocity = moveVector * _moveSpeed;
        }
        else
        {
            gameObject.transform.parent.gameObject.SetActive(false);
            gameObject.SetActive(false);
            GenericSingleton<GameManager>.Instance.CheckState();
        }
    }

    void ChasePlayer()
    {
        RaycastHit2D hit;
        Vector2 dirPlayer = Vector2.zero;
        for(int i = 0; i < directions.Length; i++)
        {
            hit = Physics2D.Raycast(transform.position, directions[i], Mathf.Infinity, 1 << LayerMask.NameToLayer("Player"));
            if (hit)
            {
                dirPlayer = directions[i];
                moveVector = dirPlayer;
                _moveSpeed = _dashSpeed;
                _isChase = true;

                break;
            }
            else
            {
                
                _moveSpeed = 1;
                _isChase = false;
            }
        }
        
    }
    void ChangeDirection()
    {
        if (!_isChase)
        {
            RaycastHit2D hit;
            Vector2 dirPlayer = Vector2.zero;
            hit = Physics2D.Raycast(transform.position, moveVector, Mathf.Infinity, 1 << LayerMask.NameToLayer("Wall"));
            if (hit.distance < Random.Range(0.3f, 1.5f))
            {
                
                moveVector = directions[Random.Range(0, 4)];
            }
        }

    }
    void AnimeCon()
    {
        if (moveVector == Vector2.down)
        {
            _animator.Play("MaskDown");
        }
        else if (moveVector == Vector2.up)
        {
            _animator.Play("MaskUp");
        }
        else if (moveVector == Vector2.right)
        {
            _animator.Play("MaskRight");
        }
        else
        {
            _animator.Play("MaskLeft");
        }
    }


}
