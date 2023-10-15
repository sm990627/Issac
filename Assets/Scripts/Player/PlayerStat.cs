using JetBrains.Annotations;
using UnityEngine;

public  class PlayerStat
{
     float _maxHp = 3;
     public float MaxHp { get { return _maxHp; } set { _maxHp = Mathf.Clamp(value, 0, _maxTotalHp); } }
     float _maxTotalHp;
     public float MaxTotalHp { get { return _maxTotalHp; } }
     float _hp = 3;
     public float Hp { get { return _hp; } set { _hp = Mathf.Clamp(value, 0, _maxHp); } }

     float _speed = 4.0f;
     public float Speed { get { return _speed; } set { _speed = value; } }

     float _power = 1.0f;
     public float Power { get { return _power; } set { _power = value; } }

     float _attackSpeed = 0.4f;
     public float AttackSpeed { get { return _attackSpeed; } set { _attackSpeed = value; } }

     int _bulletCnt = 1;
     public int BulletCnt { get { return _bulletCnt; } set { _bulletCnt = value; } }

     float _range = 8.0f;
     public float Range { get { return _range; } set { _range = value; } }

     float _bulletSpeed = 6.0f;
     public float BulletSpeed { get { return _bulletSpeed; } set { _bulletSpeed = value; } }
    public PlayerStat(float maxHp, float maxTotalHp, float hp, float speed, float power, float attackSpeed, int bulletCnt, float range, float bulletSpeed)
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
   
}
