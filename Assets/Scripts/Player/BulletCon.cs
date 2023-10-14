using UnityEngine;

public class BulletCon : MonoBehaviour
{
    float _deleteTime;
    Rigidbody2D _rb;
    bool _gravity;
    Animator _animator;
    AudioSource _audioSource;
    GameObject _player;
    AttackCon _attcnt;
    [SerializeField] AudioClip[] _tearPop;
    bool _flag;
    string[] _animes = { "DestroyA", "DestroyB" };

    void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _player = GameObject.Find("PlayerHead");
        _attcnt = _player.GetComponent<AttackCon>();
        GenericSingleton<UIBase>.Instance.EffectVolume += EffectSound;
        GenericSingleton<UIBase>.Instance.SoundInit();
        _deleteTime = _attcnt.GetRange() /_attcnt.GetBulletSpeed();
        Invoke("GravityOn",_deleteTime - 0.5f);
        Invoke("BulletDestroy", _deleteTime);
        _rb = GetComponent<Rigidbody2D>();
    }

   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        BulletDestroy();
    }
    void GravityOn()
    {
        _gravity = true;
    }
    void BulletDestroy()
    {
        if (!_flag)
        {
            _rb.velocity = Vector3.zero;
            _flag = true;
            _gravity = false;
            _audioSource.PlayOneShot(_tearPop[Random.Range(0,_tearPop.Length)]);
            _animator.Play(_animes[Random.Range(0,2)]);
            GenericSingleton<UIBase>.Instance.EffectVolume -= EffectSound;
            Invoke("BulletOff",0.3f);
            Invoke("FlagOff", 0.5f);
        }
    }
    void FlagOff()
    {
        _flag = false;
    }
    void BulletOff()
    {
        gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (_gravity)
        {
            _rb.velocity += Physics2D.gravity * Time.fixedDeltaTime * 0.3f;
        }
    }
    void EffectSound(float value)
    {
        _audioSource.volume = value;
    }


}
