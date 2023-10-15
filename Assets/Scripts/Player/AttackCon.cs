using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackCon : GenericSingleton<AttackCon>
{
    //�÷��̾� ���Ⱥ���
    float _power = 1.0f;
    float _attackSpeed = 0.4f;
    int _bulletCnt = 1;
    float _range = 8.0f;
    float _bulletSpeed = 6.0f;
    float FireX;
    float FireY;
    float angleA;

    bool inAttack = false;

    //���ݾִϸ��̼� ��������
    string upAttack = "AttackUp";
    string downAttack = "AttackDown";
    string leftAttack = "AttackLeft";
    string rightAttack = "AttackRight";

    //�Ѿ� ������ƮǮ��������
    GameObject _bullet;
    [SerializeField ]GameObject _bulletParent;
    GameObject[] _bulletPool;
    int _poolIndex;
    public void Init()
    {
        _power = GenericSingleton<PlayerCon>.Instance.Pstat.Power;
        _attackSpeed = GenericSingleton<PlayerCon>.Instance.Pstat.AttackSpeed;
        _bulletCnt = GenericSingleton<PlayerCon>.Instance.Pstat.BulletCnt;
        _range = GenericSingleton<PlayerCon>.Instance.Pstat.Range;
        _bulletSpeed = GenericSingleton<PlayerCon>.Instance.Pstat.BulletSpeed;
        _bullet = GenericSingleton<PlayerCon>.Instance.Bullet;
        InstantiateBullet();
        GetComponent<Animator>().SetBool("isIdle", true);

    }

    void InstantiateBullet()
    {
        foreach (Transform child in _bulletParent.transform)
        {
            Destroy(child.gameObject);
        }
        _bulletPool = new GameObject[50];
        for (int i = 0; i < _bulletPool.Length; i++)
        {
            GameObject gameObject = Instantiate(_bullet,_bulletParent.transform);
            _bulletPool[i] = gameObject;
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(GenericSingleton<GameManager>.Instance.CurrentState == GameState.EnemiesOn || GenericSingleton<GameManager>.Instance.CurrentState == GameState.EnemiesOff)
        {
            FireX = Input.GetAxisRaw("FireX");
            FireY = Input.GetAxisRaw("FireY");
            Vector2 fromPt2 = transform.position;
            Vector2 toPt2 = new Vector2(fromPt2.x + FireX, fromPt2.y + FireY);
            angleA = GetAngleA(fromPt2, toPt2);
            if ((FireX != 0 || FireY != 0) && inAttack == false)
            {
                Attack();
                AttackAnime(angleA);
                Invoke("StopAttack", _attackSpeed);

            }
            else if (FireX != 0 || FireY != 0)
            {
                GetComponent<Animator>().SetBool("isIdle", true);
            }
        }

    }
    

    //���� �ΰ��� �޾� ���ݰ� ���
    float GetAngleA(Vector2 v1, Vector2 v2)
    {
        float angle;
        if (FireX != 0 || FireY != 0)
        {
            float dx = v2.x - v1.x;
            float dy = v2.y - v1.y;
            float rad = Mathf.Atan2(dy, dx);
            angle = Mathf.Rad2Deg * rad;
        }
        else
        {
            angle = angleA;
        }
        return angle;
    }
    public float GetRange()
    {
        return _range;
    }
    public float GetBulletSpeed()
    {
        return _bulletSpeed;
    }
    //���ݼӵ� ������ �Լ�
    void StopAttack()
    {
        inAttack = false;
    }
    void Attack()
    {
        float axisH = GenericSingleton<PlayerCon>.Instance.GetComponent<PlayerCon>().GetAxis().x;
        float axisV = GenericSingleton<PlayerCon>.Instance.GetComponent<PlayerCon>().GetAxis().y;
        switch (_bulletCnt)
        {
            case 1: //�ѹ� �߻� �÷��̾� ���� ��� ������ġ ����   ���ݰ� ���Ѱɷ� cos, sin�� �������� (�߻翡 �̿�)
                {
                    float x = Mathf.Cos(angleA * Mathf.Deg2Rad);
                    float y = Mathf.Sin(angleA * Mathf.Deg2Rad);
                    //���� ��ġ ����
                    Vector3 VI = new Vector3(transform.position.x + x * 0.3f + Random.Range(-0.2f,0.2f) * y, transform.position.y + y * 0.2f + Random.Range(-0.2f, 0.2f) * x, transform.position.z);

                    //�߻� ����  
                    Vector3 VD = new Vector3(x + axisH * 0.2f, y + axisV * 0.2f, 0) * _bulletSpeed;

                    BulletSetting(_poolIndex++, VI, VD);
                    IndexReset();

                    break;
                }
            case 2: //�ι��� ��쿣 ���� ������ġ���� ���ݾ��������� ����߻� 
                {
                    float x = Mathf.Cos(angleA * Mathf.Deg2Rad);
                    float y = Mathf.Sin(angleA * Mathf.Deg2Rad);


                    //���� ��ġ ����
                    //angleA �� ������� �غ��� x�� sin��Ÿ y�� -cos��Ÿ������  ��Ī���� �ϳ�������
                    Vector3 VI1 = new Vector3(transform.position.x - 0.15f * y + FireX * 0.3f, transform.position.y + 0.15f * x + FireY * 0.5f, transform.position.z);
                    Vector3 VI2 = new Vector3(transform.position.x + 0.15f * y + FireX * 0.3f, transform.position.y - 0.15f * x + FireY * 0.5f, transform.position.z);

                    Vector3 VD = new Vector3(x + axisH * 0.2f, y + axisV * 0.2f, 0) * _bulletSpeed;

                    BulletSetting(_poolIndex++, VI1, VD);
                    IndexReset();

                    BulletSetting(_poolIndex++, VI2, VD);
                    IndexReset();

                    break;
                }
            case 3: // 3��, 4���� ��쿣 ��ź�� �߻� ���ݾ� ������ Ʋ� ���� �� �߻�
                {
                    float x1 = Mathf.Cos((angleA + 3.5f) * Mathf.Deg2Rad);
                    float y1 = Mathf.Sin((angleA + 3.5f) * Mathf.Deg2Rad);
                    float x2 = Mathf.Cos(angleA * Mathf.Deg2Rad);
                    float y2 = Mathf.Sin(angleA * Mathf.Deg2Rad);
                    float x3 = Mathf.Cos((angleA - 3.5f) * Mathf.Deg2Rad);
                    float y3 = Mathf.Sin((angleA - 3.5f) * Mathf.Deg2Rad);


                    //���� ��ġ ���� ��� �Ѿ��� ���� ������ ��ġ
                    Vector3 VI1 = new Vector3(transform.position.x - 0.2f * y2 + FireX * 0.5f, transform.position.y + 0.2f * x1 + FireY * 0.5f, transform.position.z);
                    Vector3 VI2 = new Vector3(transform.position.x + FireX * 0.6f, transform.position.y + FireY * 0.6f, transform.position.z);
                    Vector3 VI3 = new Vector3(transform.position.x + 0.2f * y2 + FireX * 0.5f, transform.position.y - 0.2f * x3 + FireY * 0.5f, transform.position.z);
                    //�߻� ����
                    Vector3 VD1 = new Vector3(x1 + axisH * 0.2f, y1 + axisV * 0.2f, 0) * _bulletSpeed;
                    Vector3 VD2 = new Vector3(x2 + axisH * 0.2f, y2 + axisV * 0.2f, 0) * _bulletSpeed;
                    Vector3 VD3 = new Vector3(x3 + axisH * 0.2f, y3 + axisV * 0.2f, 0) * _bulletSpeed;

                    BulletSetting(_poolIndex++, VI1, VD1);
                    IndexReset();

                    BulletSetting(_poolIndex++, VI2, VD2);
                    IndexReset();

                    BulletSetting(_poolIndex++, VI3, VD3);
                    IndexReset();

                    break;
                }
            case 4:
                { //������ �߻� ����
                    float x1 = Mathf.Cos((angleA + 3.5f) * Mathf.Deg2Rad);
                    float y1 = Mathf.Sin((angleA + 3.5f) * Mathf.Deg2Rad);
                    float x2 = Mathf.Cos((angleA + 1.5f) * Mathf.Deg2Rad);
                    float y2 = Mathf.Sin((angleA + 1.5f) * Mathf.Deg2Rad);
                    float x3 = Mathf.Cos((angleA - 1.5f) * Mathf.Deg2Rad);
                    float y3 = Mathf.Sin((angleA - 1.5f) * Mathf.Deg2Rad);
                    float x4 = Mathf.Cos((angleA - 3.5f) * Mathf.Deg2Rad);
                    float y4 = Mathf.Sin((angleA - 3.5f) * Mathf.Deg2Rad);
                    float x = Mathf.Cos(angleA * Mathf.Deg2Rad);
                    float y = Mathf.Sin(angleA * Mathf.Deg2Rad);


                    //���� ��ġ ����  // y x ������������  fireX,Y �����߻� ����
                    Vector3 VI1 = new Vector3(transform.position.x - y * 0.3f + FireX * 0.4f, transform.position.y + x * 0.3f + FireY * 0.4f, transform.position.z);
                    Vector3 VI2 = new Vector3(transform.position.x - y * 0.15f + FireX * 0.6f, transform.position.y + x * 0.15f + FireY * 0.6f, transform.position.z);
                    Vector3 VI3 = new Vector3(transform.position.x + y * 0.15f + FireX * 0.6f, transform.position.y - x * 0.15f + FireY * 0.6f, transform.position.z);
                    Vector3 VI4 = new Vector3(transform.position.x + y * 0.3f + FireX * 0.4f, transform.position.y - x * 0.3f + FireY * 0.4f, transform.position.z);

                    Vector3 VD1 = new Vector3(x1 + axisH * 0.2f, y1 + axisV * 0.2f, 0) * _bulletSpeed;
                    Vector3 VD2 = new Vector3(x2 + axisH * 0.2f, y2 + axisV * 0.2f, 0) * _bulletSpeed;
                    Vector3 VD3 = new Vector3(x3 + axisH * 0.2f, y3 + axisV * 0.2f, 0) * _bulletSpeed;
                    Vector3 VD4 = new Vector3(x4 + axisH * 0.2f, y4 + axisV * 0.2f, 0) * _bulletSpeed;


                    BulletSetting(_poolIndex++, VI1,VD1);
                    IndexReset();

                    BulletSetting(_poolIndex++, VI2, VD2);
                    IndexReset();

                    BulletSetting(_poolIndex++, VI3, VD3);
                    IndexReset();

                    BulletSetting(_poolIndex++, VI4, VD4);
                    IndexReset();

                    break;
                }

        }
        inAttack = true;

    }
    void BulletSetting(int idx,Vector3 VI, Vector3 VD)
    {
        _bulletPool[idx].SetActive(true);
        _bulletPool[idx].transform.position = VI;
        if (_power >= 3) _bulletPool[idx].GetComponent<SpriteRenderer>().color = Color.red;
        else _bulletPool[idx].GetComponent<SpriteRenderer>().color = Color.white;
        Rigidbody2D rb = _bulletPool[idx].GetComponent<Rigidbody2D>();
        rb.AddForce(VD, ForceMode2D.Impulse);
    }
    void AttackAnime(float angle)
    {
        if (angle >= -45 && angle < 45)
        {
            GetComponent<Animator>().Play(rightAttack);
        }
        else if (angle >= 45 && angle <= 135)
        {
            GetComponent<Animator>().Play(upAttack);
        }
        else if (angle >= -135 && angle <= -45)
        {
            GetComponent<Animator>().Play(downAttack);
        }
        else
        {
            GetComponent<Animator>().Play(leftAttack);
        }

    }
    public float GetPower()
    {
        return _power;
    }
    
    void IndexReset()
    {
        if (_poolIndex == 50) _poolIndex = 0;
    }
}
