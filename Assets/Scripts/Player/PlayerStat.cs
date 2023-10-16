using JetBrains.Annotations;
using UnityEngine;

public  class PlayerStat
{
     float _maxHp = 3;
     public float MaxHp { get { return _maxHp; } }
     float _maxTotalHp;
     public float MaxTotalHp { get { return _maxTotalHp; } }
     float _hp = 3;
     public float Hp { get { return _hp; }  }

     float _speed = 4.0f;
     public float Speed { get { return _speed; } }

     float _power = 1.0f;
     public float Power { get { return _power; } }

     float _attackSpeed = 0.4f;
     public float AttackSpeed { get { return _attackSpeed; } }

     int _bulletCnt = 1;
     public int BulletCnt { get { return _bulletCnt; } }

     float _range = 8.0f;
     public float Range { get { return _range; } }
     int _luck = 1;
     public int Luck { get { return _luck; } }  

     float _bulletSpeed = 6.0f;
     public float BulletSpeed { get { return _bulletSpeed; } set { _bulletSpeed = value; } }
     public PlayerStat(float maxHp, float maxTotalHp, float hp, float speed, float power, float attackSpeed, int bulletCnt, float range, float bulletSpeed, int luck)
     {
        _maxHp = maxHp;
        _maxTotalHp = maxTotalHp;
        _hp = hp;
        _speed = speed;
        _power = power;
        _attackSpeed = attackSpeed;
        _bulletCnt = bulletCnt;
        _range = range;
        _bulletSpeed = bulletSpeed;
        _luck = luck;
     }
     public void LoadPlayerStat(PlayerStatData data)
     {
        _maxHp = data.MaxHp;
        _hp = data.Hp;
        _speed = data.Speed;
        _power = data.Power;
        _attackSpeed = data.AttackSpeed;
        _bulletCnt = data.BulletCnt;
        _range = data.Range;
        _bulletSpeed = data.BulletSpeed;
     }
    public void SetMaxHp(float maxHp)
    {
         _maxHp = Mathf.Clamp(maxHp, 0, _maxTotalHp); 
    }
    public void SetHp(float Hp)
    {
        _hp = Mathf.Clamp(Hp, 0, _maxHp);
    }
    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
    public void SetPower(float power)
    {
        _power = power;
    }
    public void SetAttackSpeed(float attackSpeed)
    {
        _attackSpeed = attackSpeed;
    }
    public void SetBulletCnt(int cnt)
    {
        _bulletCnt = cnt;
    }
    public void SetRange(float range)
    {
        _range = range;
    }
    public void SetLuck(int luck)
    {
        _luck = luck;
    }
}

