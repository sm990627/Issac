using UnityEngine;

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
    GameObject _bulletParent;
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
        if (_power >= 3) _bullet.GetComponent<SpriteRenderer>().color = Color.red;
        else _bullet.GetComponent<SpriteRenderer>().color = Color.white;

    }

    void Start()
    {
        _bulletPool = new GameObject[50];
        _bulletParent = GameObject.FindWithTag("Pool");
        for (int i = 0; i < _bulletPool.Length; i++)
        {
            GameObject gameObject = Instantiate(_bullet, _bulletParent.transform);
            _bulletPool[i] = gameObject;
            gameObject.SetActive(false);
        }
        GetComponent<Animator>().SetBool("isIdle", true);

    }


    void Update()
    {
        if(GenericSingleton<GameManager>.Instance.CurrentState == GameManager.GameState.EnemiesOn || GenericSingleton<GameManager>.Instance.CurrentState == GameManager.GameState.EnemiesOff)
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
                    //Quaternion r = Quaternion.Euler(0, 0, angleA);
                    float x = Mathf.Cos(angleA * Mathf.Deg2Rad);
                    float y = Mathf.Sin(angleA * Mathf.Deg2Rad);
                    //���� ��ġ ����
                    Vector3 VI = new Vector3(transform.position.x + x * 0.3f + Random.RandomRange(-0.2f,0.2f) * y, transform.position.y +y * 0.2f + Random.RandomRange(-0.2f, 0.2f) * x, transform.position.z);

                    //GameObject bulletprefab1 = Instantiate(_bullet, VI, r);   ������Ʈ Ǯ������� ����
                    //Rigidbody2D rb = bulletprefab1.GetComponent<Rigidbody2D>(); ������Ʈ Ǯ������� ����

                    _bulletPool[_poolIndex].SetActive(true);      //������Ʈ Ǯ���� ������ �Ѿ� �ѱ�
                    _bulletPool[_poolIndex].transform.position = VI;
                    Rigidbody2D rb = _bulletPool[_poolIndex++].GetComponent<Rigidbody2D>();
                    IndexReset();

                   

                    //�߻� ����   //clamp������ -���ذ�ȵ� ���� ũ�� �з� ���
                    Vector3 VD = new Vector3(x + axisH * 0.2f, y + axisV * 0.2f, 0) * _bulletSpeed;

                    rb.AddForce(VD, ForceMode2D.Impulse);
                    break;
                }
            case 2: //�ι��� ��쿣 ���� ������ġ���� ���ݾ��������� ����߻� 
                {
                    float x = Mathf.Cos(angleA * Mathf.Deg2Rad);
                    float y = Mathf.Sin(angleA * Mathf.Deg2Rad);

                    //Quaternion r = Quaternion.Euler(0, 0, angleA);

                    //���� ��ġ ����
                    //angleA �� ������� �غ��� x�� sin��Ÿ y�� -cos��Ÿ������  ��Ī���� �ϳ�������
                    Vector3 VI1 = new Vector3(transform.position.x - 0.15f * y + FireX * 0.3f, transform.position.y + 0.15f * x + FireY * 0.5f, transform.position.z);
                    Vector3 VI2 = new Vector3(transform.position.x + 0.15f * y + FireX * 0.3f, transform.position.y - 0.15f * x + FireY * 0.5f, transform.position.z);

                    //GameObject bulletprefab1 = Instantiate(_bullet, VI1, r);
                    //Rigidbody2D rb1 = bulletprefab1.GetComponent<Rigidbody2D>();

                    //GameObject bulletprefab2 = Instantiate(_bullet, VI2, r);
                    //Rigidbody2D rb2 = bulletprefab2.GetComponent<Rigidbody2D>();


                    _bulletPool[_poolIndex].SetActive(true);
                    _bulletPool[_poolIndex].transform.position = VI1;
                    Rigidbody2D rb1 = _bulletPool[_poolIndex++].GetComponent<Rigidbody2D>();
                    IndexReset();
                    _bulletPool[_poolIndex].SetActive(true);
                    _bulletPool[_poolIndex].transform.position = VI2;
                    Rigidbody2D rb2 = _bulletPool[_poolIndex++].GetComponent<Rigidbody2D>();
                    IndexReset();



                    //�߻� ����
                    Vector3 VD = new Vector3(x + axisH * 0.2f, y + axisV * 0.2f, 0) * _bulletSpeed;
                    rb1.AddForce(VD, ForceMode2D.Impulse);
                    rb2.AddForce(VD, ForceMode2D.Impulse);
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

                    //Quaternion r = Quaternion.Euler(0, 0, angleA);

                    //���� ��ġ ���� ��� �Ѿ��� ���� ������ ��ġ
                    Vector3 VI1 = new Vector3(transform.position.x - 0.2f * y2 + FireX * 0.5f, transform.position.y + 0.2f * x1 + FireY * 0.5f, transform.position.z);
                    Vector3 VI2 = new Vector3(transform.position.x + FireX * 0.6f, transform.position.y + FireY * 0.6f, transform.position.z);
                    Vector3 VI3 = new Vector3(transform.position.x + 0.2f * y2 + FireX * 0.5f, transform.position.y - 0.2f * x3 + FireY * 0.5f, transform.position.z);

                    // bulletprefab1 = Instantiate(_bullet, VI1, r);
                    //Rigidbody2D rb1 = bulletprefab1.GetComponent<Rigidbody2D>();
                    //GameObject bulletprefab2 = Instantiate(_bullet, VI2, r);
                    //Rigidbody2D rb2 = bulletprefab2.GetComponent<Rigidbody2D>();
                    //GameObject bulletprefab3 = Instantiate(_bullet, VI3, r);
                    //Rigidbody2D rb3 = bulletprefab3.GetComponent<Rigidbody2D>();

                    _bulletPool[_poolIndex].SetActive(true);
                    _bulletPool[_poolIndex].transform.position = VI1;
                    Rigidbody2D rb1 = _bulletPool[_poolIndex++].GetComponent<Rigidbody2D>();
                    IndexReset();
                    _bulletPool[_poolIndex].SetActive(true);
                    _bulletPool[_poolIndex].transform.position = VI2;
                    Rigidbody2D rb2 = _bulletPool[_poolIndex++].GetComponent<Rigidbody2D>();
                    IndexReset();
                    _bulletPool[_poolIndex].SetActive(true);
                    _bulletPool[_poolIndex].transform.position = VI3;
                    Rigidbody2D rb3 = _bulletPool[_poolIndex++].GetComponent<Rigidbody2D>();
                    IndexReset();

                    //�߻� ����
                    Vector3 VD1 = new Vector3(x1 + axisH * 0.2f, y1 + axisV * 0.2f, 0) * _bulletSpeed;
                    Vector3 VD2 = new Vector3(x2 + axisH * 0.2f, y2 + axisV * 0.2f, 0) * _bulletSpeed;
                    Vector3 VD3 = new Vector3(x3 + axisH * 0.2f, y3 + axisV * 0.2f, 0) * _bulletSpeed;

                    rb1.AddForce(VD1, ForceMode2D.Impulse);
                    rb2.AddForce(VD2, ForceMode2D.Impulse);
                    rb3.AddForce(VD3, ForceMode2D.Impulse);
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

                    //Quaternion r = Quaternion.Euler(0, 0, angleA);

                    //���� ��ġ ����  // y x ������������  fireX,Y �����߻� ����
                    Vector3 VI1 = new Vector3(transform.position.x - y * 0.3f + FireX * 0.4f, transform.position.y + x * 0.3f + FireY * 0.4f, transform.position.z);
                    Vector3 VI2 = new Vector3(transform.position.x - y * 0.15f + FireX * 0.6f, transform.position.y + x * 0.15f + FireY * 0.6f, transform.position.z);
                    Vector3 VI3 = new Vector3(transform.position.x + y * 0.15f + FireX * 0.6f, transform.position.y - x * 0.15f + FireY * 0.6f, transform.position.z);
                    Vector3 VI4 = new Vector3(transform.position.x + y * 0.3f + FireX * 0.4f, transform.position.y - x * 0.3f + FireY * 0.4f, transform.position.z);

                    //GameObject bulletprefab1 = Instantiate(_bullet, VI1, r);
                    //Rigidbody2D rb1 = bulletprefab1.GetComponent<Rigidbody2D>();
                    //GameObject bulletprefab2 = Instantiate(_bullet, VI2, r);
                    //Rigidbody2D rb2 = bulletprefab2.GetComponent<Rigidbody2D>();
                    //GameObject bulletprefab3 = Instantiate(_bullet, VI3, r);
                    //Rigidbody2D rb3 = bulletprefab3.GetComponent<Rigidbody2D>();
                    //GameObject bulletprefab4 = Instantiate(_bullet, VI4, r);
                    //Rigidbody2D rb4 = bulletprefab4.GetComponent<Rigidbody2D>();

                    _bulletPool[_poolIndex].SetActive(true);
                    _bulletPool[_poolIndex].transform.position = VI1;
                    Rigidbody2D rb1 = _bulletPool[_poolIndex++].GetComponent<Rigidbody2D>();
                    IndexReset();
                    _bulletPool[_poolIndex].SetActive(true);
                    _bulletPool[_poolIndex].transform.position = VI2;
                    Rigidbody2D rb2 = _bulletPool[_poolIndex++].GetComponent<Rigidbody2D>();
                    IndexReset();
                    _bulletPool[_poolIndex].SetActive(true);
                    _bulletPool[_poolIndex].transform.position = VI3;
                    Rigidbody2D rb3 = _bulletPool[_poolIndex++].GetComponent<Rigidbody2D>();
                    IndexReset();
                    _bulletPool[_poolIndex].SetActive(true);
                    _bulletPool[_poolIndex].transform.position = VI4;
                    Rigidbody2D rb4 = _bulletPool[_poolIndex++].GetComponent<Rigidbody2D>();
                    IndexReset();
                    //�߻� ����
                    Vector3 VD1 = new Vector3(x1 + axisH * 0.2f, y1 + axisV * 0.2f, 0) * _bulletSpeed;
                    Vector3 VD2 = new Vector3(x2 + axisH * 0.2f, y2 + axisV * 0.2f, 0) * _bulletSpeed;
                    Vector3 VD3 = new Vector3(x3 + axisH * 0.2f, y3 + axisV * 0.2f, 0) * _bulletSpeed;
                    Vector3 VD4 = new Vector3(x4 + axisH * 0.2f, y4 + axisV * 0.2f, 0) * _bulletSpeed;

                    rb1.AddForce(VD1, ForceMode2D.Impulse);
                    rb2.AddForce(VD2, ForceMode2D.Impulse);
                    rb3.AddForce(VD3, ForceMode2D.Impulse);
                    rb4.AddForce(VD4, ForceMode2D.Impulse);
                    break;
                }

        }
        inAttack = true;

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
