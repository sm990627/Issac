using UnityEngine;
using System.Collections;
using System;

public class Monstro : MonoBehaviour
{
    Transform _player;
    GameObject _boss;
    AudioSource _audioSouce;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float _jumpSpeed = 3;
    [SerializeField] float _highJumpSpeed = 8;
    [SerializeField] AudioClip[] _jumpSound;
    [SerializeField] AudioClip _attackSound;

    [SerializeField] float _maxHp = 50;
    float _speed;
    public float MaxHP { get { return _maxHp; } }
    float _hp;
    bool _isClear;
    public float HP { get { return _hp; } }
    [SerializeField] Rigidbody2D _rig;
    SpriteRenderer _ren;
    Animator _animator;
    [SerializeField] GameObject _firePos;
    [SerializeField]  ParticleSystem _AttackParticle;
    [SerializeField]  ParticleSystem _LandingParticle;
    
    BossState _currentState;
    AttackCon _attCnt;
    enum BossState
    {
        Spawn,
        Idle,
        Jump,
        HighJump,
        Attack,
        Die,
    }


    IEnumerator StartDelayed()
    {
        

        while (GenericSingleton<GameManager>.Instance.CurrentState == GameState.Playing)
        {
            yield return null;
        }

        StartCoroutine(StateMachine());
    }
    IEnumerator StateMachine()
    {
        while (_hp > 0)
        {

            switch (_currentState)
            {
                case BossState.Spawn:

                    _animator.Play("MonstroSpawn");
                    yield return new WaitForSeconds(0.5f);
                    _currentState = BossState.Idle; // 다음 상태로 전환
                    break;
                case BossState.Idle:

                    _animator.Play("MonstroIdle");
                    yield return new WaitForSeconds(0.5f);
                    _currentState = GetRandomState(); // 다음 상태로 전환
                    break;

                case BossState.Attack:
                    _animator.Play("MonstroAttack");
                    yield return new WaitForSeconds(2.5f);
                    _currentState = BossState.Idle; // 다음 상태로 전환
                    break;

                case BossState.Jump:
                    _speed = _jumpSpeed;
                    _animator.Play("MonstroJump");
                    int _jumpCount = UnityEngine.Random.Range(2,4);   //1~2번 랜덤 점프
                    for (int i = 0; i < _jumpCount; i++)
                    {
                        yield return new WaitForSeconds(1.2f);
                        _currentState = BossState.Idle;
                        // Attack 상태 로직
                    }
                    break;
                case BossState.HighJump:
                    _speed = _highJumpSpeed;
                    _animator.Play("MonstroHighJump");
                    yield return new WaitForSeconds(2.3f);
                    _currentState = BossState.Idle; // 다음 상태로 전환
                    break;
            }
            
        }   
    }
    BossState GetRandomState()  //Idle,spawn 을제외한 모션중 랜덤
    {
        int randomidx = UnityEngine.Random.Range(2, Enum.GetValues(typeof(BossState)).Length-1);
        return (BossState)randomidx;
    }


    void Start()
    {
        _player = GameObject.Find("PlayerHead").transform;
        _attCnt = _player.GetComponent<AttackCon>();
        _boss = GameObject.FindGameObjectWithTag("Boss");
        _animator = GetComponent<Animator>();
        _audioSouce = GetComponent<AudioSource>();
        _ren = _boss.GetComponent<SpriteRenderer>();
        _currentState = BossState.Spawn;
        _hp = _maxHp;
        StartCoroutine(StartDelayed());

    }

    void Update()
    {
        CheckWall();

    }

    void MoveToPlayer() 
    {
        Vector3 dirVector = (_player.position - _boss.transform.position).normalized;
        if (dirVector.x > 0)
        {
            _ren.flipX = true;
        }
        else
        {
            _ren.flipX= false;
        }
        
        StartCoroutine(Move(transform.position + dirVector * _speed * 10 * Time.deltaTime, 0.5f));
    }
    
    IEnumerator Move(Vector3 targetPos, float moveTime)
    {
        Vector3 startPos = transform.position;
        float startTime = Time.time;

        for (int i = 0; i < 100; i++)
        {
            float t = (Time.time - startTime) / moveTime;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return new WaitForEndOfFrame();
        }

        CheckWall();
        transform.position = targetPos;
    }

    void CheckWall()  //물리계산이 아닌 트랜스폼으로 애니메이션을 관리하기때문에 벽과 충돌을 하지않아서 강제로 벽뒤로 이동 막기 
    {   
        Vector3 currentPosition = transform.position;
        currentPosition.x = Mathf.Clamp(currentPosition.x, -3.4f, 3.5f);
        currentPosition.y = Mathf.Clamp(currentPosition.y, -1.6f, 1.7f);
        transform.position = currentPosition;
    }
    void Attack()
    {
        Vector3 dirVector = (_player.position - _boss.transform.position).normalized;
        if (dirVector.x > 0)
        {
            _ren.flipX = true;
        }
        else
        {
            _ren.flipX = false;
        }
        _firePos.transform.LookAt(dirVector);

        _AttackParticle.Play();
        _audioSouce.PlayOneShot(_attackSound);

    }
    void Landing()
    {
        _LandingParticle.Play();
        _audioSouce.PlayOneShot(_jumpSound[UnityEngine.Random.Range(0, _jumpSound.Length)]);
    }
    public void JumpSound()
    {
        _audioSouce.PlayOneShot(_jumpSound[UnityEngine.Random.Range(0,_jumpSound.Length)]);
    }
    public void OnDamage()
    {
         _hp = _hp - _attCnt.GetPower();
        GenericSingleton<UIBase>.Instance.UpdateBossHP(_hp,_maxHp);
        _boss.GetComponent<SpriteRenderer>().color = new Vector4(225,60,60,255);
        Invoke("DamageEnd", 0.4f);
        if (_hp <= 0)
        {
            _animator.Play("MonstroDie");
            Invoke("Clear",1.5f);

        }
    }
    void DamageEnd()
    {
        _boss.GetComponent<SpriteRenderer>().color = Color.white;
    }
    void Clear()
    {
        if (!_isClear)
        {
            _isClear = true;
            GenericSingleton<SoundManager>.Instance.SetBasement();
            GenericSingleton<Doors>.Instance.TrapDoor(true);
            GenericSingleton<GameManager>.Instance.SetGameState(GameState.EnemiesOff);
            GenericSingleton<UIBase>.Instance.ShowBossHpBar(false);
            gameObject.SetActive(false);
        }
        
    }


    
}



