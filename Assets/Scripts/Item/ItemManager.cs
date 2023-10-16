

public class ItemManger
{
    public void AddItem(PlayerStat stat, int idx)
    {
        switch (idx)
        {
            case 0:
                {
                    if (stat.BulletCnt < 2)
                    {
                        stat.SetBulletCnt(2);  
                    }
                    break;
                }
            case 1:
                {
                    if (stat.BulletCnt < 3)
                    {
                        stat.SetBulletCnt(3);
                        stat.SetAttackSpeed(stat.AttackSpeed * 1.5f);
                    }
                    break;
                }
            case 2:
                {
                    if (stat.BulletCnt < 4)
                    {
                        stat.SetBulletCnt(4);
                        if (stat.BulletCnt != 3)
                        {
                            stat.SetAttackSpeed(stat.AttackSpeed * 2f);
                        }
                    }
                    break;
                }
            case 3:
                {
                    stat.SetMaxHp(stat.MaxHp + 2);
                    stat.SetHp(stat.Hp + 2);
                    break;
                }
            case 4:
                {
                    stat.SetPower(stat.Power + 2);
                    break;
                }
            case 5:
                {
                    stat.SetAttackSpeed(stat.AttackSpeed * 0.6f);
                    break;
                }
            case 6:
                {
                    stat.SetSpeed(stat.Speed + 1);
                    break;
                }
        }
    }
}
 
