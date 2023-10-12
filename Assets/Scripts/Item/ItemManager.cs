

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
                        stat.BulletCnt = 2;
                    }
                    break;
                }
            case 1:
                {
                    if (stat.BulletCnt < 3)
                    {
                        stat.BulletCnt = 3;
                    }
                    break;
                }
            case 2:
                {
                    if (stat.BulletCnt < 4)
                    {
                        stat.BulletCnt = 4;
                    }
                    break;
                }
            case 3:
                {
                    stat.MaxHp += 2;
                    stat.Hp += 2;
                    break;
                }
        }
    }
}
 
