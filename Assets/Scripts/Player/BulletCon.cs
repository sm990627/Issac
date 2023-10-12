using UnityEngine;

public class BulletCon : MonoBehaviour
{
    float deleteTime;
    Rigidbody2D rb;
    bool gravity;
    Animator animator;
    AudioSource audioSource;
    [SerializeField] AudioClip[] _tearPop;
    bool flag;
    string[] animes = { "DestroyA", "DestroyB" };
    void OnEnable()
    {
        audioSource =GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        GameObject player = GameObject.Find("PlayerHead");
        AttackCon attcnt = player.GetComponent<AttackCon>();
        deleteTime = attcnt.GetRange() / attcnt.GetBulletSpeed();
        Invoke("GravityOn",deleteTime - 0.5f);
        Invoke("BulletDestroy", deleteTime);
        rb = GetComponent<Rigidbody2D>();

    }

   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        BulletDestroy();
    }
    void GravityOn()
    {
        gravity = true;
    }
    void BulletDestroy()
    {
        if (!flag)
        {
            rb.velocity = Vector3.zero;
            flag = true;
            gravity = false;
            audioSource.PlayOneShot(_tearPop[Random.RandomRange(0,_tearPop.Length)]);
            animator.Play(animes[Random.RandomRange(0,2)]);
            Invoke("BulletOff",0.3f);
            Invoke("FlagOff", 0.5f);
        }
    }
    void FlagOff()
    {
        flag = false;
    }
    void BulletOff()
    {
        gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (gravity)
        {
            rb.velocity += Physics2D.gravity * Time.fixedDeltaTime * 0.3f;
        }
    }
}
