
using JetBrains.Annotations;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using Unity.VisualScripting.FullSerializer;
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
    [SerializeField] int _luck = 1;
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
    List<int> _itemIdx = new List<int>();
    public List<int> ItemIdx { get { return _itemIdx; } }
    

    //사용할 컴포넌트
    Rigidbody2D _rbody;
    [SerializeField] GameObject _player;
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

    string _lastEnemy;
    public  void Init()
    {
        gameObject.SetActive(true);
        gameObject.transform.position = Vector3.zero;
        GetComponent<Animator>().Play(idleAnime);
        gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        pStat = new PlayerStat(_maxHp,_maxTotalHp, _maxHp, _speed,_power,_attackSpeed,_bulletCnt,_range,_bulletSpeed,_luck);   
        _itemIdx.Clear();
    }

    public void LoadStart()
    {
        gameObject.SetActive(true);
        GetComponent<Animator>().Play(idleAnime);
        gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        pStat = new PlayerStat(_maxHp, _maxTotalHp, _maxHp, _speed, _power, _attackSpeed, _bulletCnt, _range, _bulletSpeed, _luck);
    }
    public void Hitted(GameObject Enemy,float dmg)
    {
        OnDamage(Enemy, dmg,"Monstro");
    }
    void Start()
    {
        _audioSource =GetComponent<AudioSource>();
        GenericSingleton<UIBase>.Instance.EffectVolume += EffectSound;
        GenericSingleton<UIBase>.Instance.SoundInit();
        _rbody = GetComponent<Rigidbody2D>();
        _rend = GetComponent<SpriteRenderer>();       
    }
    
    void Update()
    {
        if ((GenericSingleton<GameManager>.Instance.CurrentState == GameState.EnemiesOn || GenericSingleton<GameManager>.Instance.CurrentState == GameState.EnemiesOff))
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


        }
       
        
    }
    void FixedUpdate()
    {
        if ((GenericSingleton<GameManager>.Instance.CurrentState == GameState.EnemiesOn || GenericSingleton<GameManager>.Instance.CurrentState == GameState.EnemiesOff))
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
    public void SetPosition(Vector2 pos)
    {
        gameObject.transform.position = pos;
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
        //GameObject bombprefab = Instantiate(_bomb, transform.position, Quaternion.identity);
    }
    void OnDamage(GameObject enemy,float damage,string enemyname)
    {
        if (!inDamage)
        {
            pStat.SetHp(pStat.Hp - damage);
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
                GenericSingleton<GameManager>.Instance.SetGameState(GameState.GameOver);
                _player.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<Animator>().SetBool("GameOver",true);
                _audioSource.PlayOneShot(_dieSound);
                _lastEnemy = enemyname;
                gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                Invoke("GameOver",1f);
            }
        }
    }
   void GameOver()
    {
        Time.timeScale = 0;
        GenericSingleton<UIBase>.Instance.ShowGameOverUI(true);
        GenericSingleton<SoundManager>.Instance.SetGameOver();
        GenericSingleton<UIBase>.Instance.SetGameOverEnemy(_lastEnemy);
        _player.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.SetActive(false);
    }
    void Heal(float amount)
    {
        pStat.SetHp(pStat.Hp + amount);
        _audioSource.PlayOneShot(_healSound);
        if (pStat.Hp >= pStat.MaxHp)
        {
            pStat.SetHp(pStat.MaxHp);
        }
        GenericSingleton<UIBase>.Instance.HpUpdate();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            OnDamage(collision.gameObject,0.5f,collision.gameObject.name);
        }
        if (collision.collider.CompareTag("EnemyBullet"))
        {
            OnDamage(collision.gameObject, 0.5f,collision.collider.GetComponent<EnemyBulletCon>().GetName());
        }
        if (collision.collider.CompareTag("Boss"))
        {
            OnDamage(collision.gameObject,1,"Monstro");
        }
        if (collision.collider.CompareTag("PickUp"))
        {
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
            _itemIdx.Add(collision.GetComponent<ItemIdx>().Idx); //리스트에 아이템 인덱스저장
            im.AddItem(pStat, collision.GetComponent<ItemIdx>().Idx); //아이템 정보전달
            GenericSingleton<UIBase>.Instance.InvenDraw(collision.GetComponent<ItemIdx>().Idx);
            GenericSingleton<UIBase>.Instance.StatUIInit();
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


    void EffectSound(float value)
    {
        _audioSource.volume = value;
    }
    private void OnDestroy()
    {
        GenericSingleton<UIBase>.Instance.EffectVolume -= EffectSound;
    }
    public void LoadItemData(List<int> items)
    {
        _itemIdx = items;
    }
   

}

