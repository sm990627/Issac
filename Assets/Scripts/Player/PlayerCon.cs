
using System.Collections.Generic;
using UnityEngine;


public class PlayerCon : GenericSingleton<PlayerCon>
{
    [Header("플레이어 스탯")]
    [SerializeField] float  _maxTotalHp = 12;
    [SerializeField] float _maxHp = 3;
    [SerializeField] float _speed = 4.0f;
    [SerializeField] float _power = 1.0f;
    [SerializeField] float _attackSpeed = 0.4f;
    [SerializeField] int _bulletCnt = 1;
    [SerializeField] float _range = 8.0f;
    [SerializeField] float _bulletSpeed = 6.0f;
    [SerializeField] AudioClip[] _hurtSounds;
    [SerializeField] AudioClip _dieSound;
    [SerializeField] AudioClip _itemGainSound;
    [SerializeField] AudioClip _healSound;

    AudioSource _audioSource;
    //상태관련 변수
    float axisH;
    float axisV;
    float angleM;
    bool inDamage = false;
    bool itemGain = false;

    GameObject newItem;
    List<int> itemIdx = new List<int>();

    //사용할 컴포넌트
    Rigidbody2D _rbody;
    GameObject _player;
    SpriteRenderer _rend;
    ItemManger im = new ItemManger();
    PlayerStat pStat;
    public PlayerStat Pstat { get { return pStat; } }   

    //생성할 오브젝트
    [SerializeField] GameObject _bomb;
    [SerializeField] GameObject bullet;
    public GameObject Bullet { get { return bullet; } }

    //이동 애니메이션
    string upAnime = "PlayerUp";
    string downAnime = "PlayerDown";
    string rightAnime = "PlayerRight";
    string idleAnime = "PlayerIdle";

    public  void Init()
    {
        gameObject.SetActive(true);
        pStat = new PlayerStat(_maxHp,_maxTotalHp, _maxHp, _speed,_power,_attackSpeed,_bulletCnt,_range,_bulletSpeed);
        _player = GameObject.Find("PlayerHead");        
    }
    public void Hitted(GameObject Enemy,float dmg)
    {
        OnDamage(Enemy, 1f);
    }
    void Start()
    {
        _audioSource =GetComponent<AudioSource>();
        _rbody = GetComponent<Rigidbody2D>();
        _rend = GetComponent<SpriteRenderer>();       
    }
    
    void Update()
    {
        if (GenericSingleton<GameManager>.Instance.CurrentState == GameManager.GameState.EnemiesOn || GenericSingleton<GameManager>.Instance.CurrentState == GameManager.GameState.EnemiesOff)
        {
            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical");
            Vector2 fromPt = transform.position;
            Vector2 toPt = new Vector2(fromPt.x + axisH, fromPt.y + axisV);
            angleM = GetAngleM(fromPt, toPt);
            if (!itemGain)
            {
                if (axisH != 0 || axisV != 0)
                {
                    if (angleM >= -45 && angleM < 45)
                    {
                        GetComponent<Animator>().Play(rightAnime);
                        _rend.flipX = false;
                    }
                    else if (angleM >= 45 && angleM <= 135)
                    {
                        GetComponent<Animator>().Play(upAnime);
                    }
                    else if (angleM >= -135 && angleM <= -45)
                    {
                        GetComponent<Animator>().Play(downAnime);
                    }
                    else
                    {
                        GetComponent<Animator>().Play(rightAnime);
                        _rend.flipX = true;
                    }

                }
                else
                {
                    GetComponent<Animator>().Play(idleAnime);
                }
            }

            if (Input.GetButtonDown("Bomb"))
            {
                SetBomb();
            }

        }
       
        
    }
    void FixedUpdate()
    {
        if (GenericSingleton<GameManager>.Instance.CurrentState == GameManager.GameState.EnemiesOn || GenericSingleton<GameManager>.Instance.CurrentState == GameManager.GameState.EnemiesOff)
        {
            if (inDamage)
            {
                float val = Mathf.Sin(Time.time * 50);

                if (val > 0)
                {   
                    _player.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
                    gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
                }
                else
                {
                    _player.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
                    gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
                }
            
            }
            else if (itemGain)
            {
                _rbody.velocity = new Vector2(0, 0);
            }
            else 
            {
                 _rbody.velocity = new Vector2(axisH, axisV) * pStat.Speed;
            }

        }
        else
        {
            _rbody.velocity = new Vector2(0, 0);
        }

        
    }

    
    //벡터 두개를 받아 이동각 계산
    float GetAngleM(Vector2 v1, Vector2 v2)
    {
        float angle;
        if (axisH != 0 || axisV != 0)
        {
            float dx = v2.x - v1.x;
            float dy = v2.y - v1.y;
            float rad = Mathf.Atan2(dy, dx);
            angle = Mathf.Rad2Deg * rad;         
        }
        else
        {
            angle = angleM;
        }
        return angle;
    }
    
    void SetBomb()
    {
        GameObject bombprefab = Instantiate(_bomb, transform.position, Quaternion.identity);
    }
    void OnDamage(GameObject enemy,float damage)
    {
        if (!inDamage)
        {
            pStat.Hp = pStat.Hp - damage;
            GenericSingleton<UIBase>.Instance.HpUpdate();
            if (pStat.Hp > 0)
            {
                _audioSource.PlayOneShot(_hurtSounds[Random.Range(0, _hurtSounds.Length)]);
                _rbody.velocity = new Vector2(0, 0);
                Vector2 dirVector = (transform.position - enemy.transform.position).normalized;
                _rbody.AddForce(new Vector2(dirVector.x * 3, dirVector.y * 3), ForceMode2D.Impulse);
                inDamage = true;
                gameObject.GetComponent<Animator>().SetBool("OnDamage", true);
                Invoke("DamageEnd", 0.25f);
            }
            else
            {
                _audioSource.PlayOneShot(_dieSound);
                gameObject.SetActive(false);
            }
        }
    }
    void Heal(float amount)
    {
        pStat.Hp = pStat.Hp + amount;
        _audioSource.PlayOneShot(_healSound);
        if (pStat.Hp >= pStat.MaxHp)
        {
            pStat.Hp = pStat.MaxHp;
        }
        GenericSingleton<UIBase>.Instance.HpUpdate();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy")|| collision.collider.CompareTag("EnemyBullet"))
        {
            OnDamage(collision.gameObject,0.5f);
        }
        if (collision.collider.CompareTag("Boss"))
        {
            OnDamage(collision.gameObject,1);
        }
        if (collision.collider.CompareTag("PickUp"))
        {
            Debug.Log(collision.collider.GetComponent<PickUpItem>()?.Type);
            if (collision.collider.GetComponent<PickUpItem>()?.Type == PickUpItem.PickUp.Heart)
            {
                if (pStat.Hp != pStat.MaxHp)
                {
                    Heal(collision.collider.GetComponent<PickUpItem>().GetHpAmount());            
                }
            }
            if (collision.collider.GetComponent<PickUpItem>()?.Type == PickUpItem.PickUp.Bomb)
            {
                //폭탄을 먹는 구문
                int _bombCnt = collision.collider.GetComponent<PickUpItem>().GetBombAmount();
                
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            newItem = collision.gameObject;
            itemGain = true;
            _audioSource.PlayOneShot(_itemGainSound);
            collision.transform.position = new Vector2(transform.position.x, transform.position.y + 0.2f); //아이템 머리위로
            gameObject.GetComponent<Animator>().SetBool("ItemGain",true);
            itemIdx.Add(collision.GetComponent<ItemIdx>().Idx); //리스트에 아이템 인덱스저장
            im.AddItem(pStat, collision.GetComponent<ItemIdx>().Idx); //아이템 정보전달
            GenericSingleton<UIBase>.Instance.InvenDraw(collision.GetComponent<ItemIdx>().Idx);

            //UI에 먹은아이템 표시

             //눈물갯수와 hp정보 전달
            GenericSingleton<AttackCon>.Instance.Init();
            GenericSingleton<UIBase>.Instance.HpUpdate();
            Invoke("ItemGainEnd", 1.5f);  //애니메이션끄기
            _player.GetComponent<SpriteRenderer>().enabled = false;
        }
       
    }
    void DamageEnd()
    {
        inDamage = false;
        gameObject.GetComponent<Animator>().SetBool("OnDamage", false);
        _player.GetComponent<SpriteRenderer>().color= new Color(255,255,255,255);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);

    }
    public Vector2 GetAxis()
    {
        Vector2 axis = new Vector2(axisH, axisV);
        return (axis);
    }
    void ItemGainEnd()
    {
        newItem.SetActive(false);
        GenericSingleton<StageManager>.Instance.CurrentRoom.TreasureItemClear();
       itemGain = false;
        gameObject.GetComponent<Animator>().SetBool("ItemGain", false);
        _player.GetComponent<SpriteRenderer>().enabled = true;
    }

   
}

