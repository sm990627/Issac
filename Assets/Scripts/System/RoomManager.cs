using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : GenericSingleton<RoomManager>
{
    Dictionary<Vector2 ,Room> _rooms;
    public Dictionary<Vector2, Room> Rooms { get { return _rooms; } }
    string[] basements;
    Vector2 _bossPos;
    Vector2 _treasurePos;
    List<Vector2> _basePos;

    

    int nearCount(Vector2 pos)
    {
        
        int count = 0;
        if (_rooms.ContainsKey(pos + Vector2.up)) count++;

        if (_rooms.ContainsKey(pos + Vector2.right)) count++;


        if (_rooms.ContainsKey(pos + Vector2.down)) count++;


        if (_rooms.ContainsKey(pos + Vector2.left)) count++;

        return count;
    }
    public Dictionary<Vector2, Room> Init()
    {
        do
        {
            _basePos = new List<Vector2> { Vector2.up, Vector2.down, Vector2.right, Vector2.left };
            _rooms = new Dictionary<Vector2, Room>();
            bool isVertical = Random.Range(0, 2) == 0 ? true : false;
            if (isVertical)
            {
                _bossPos.x = RandomOne() * 3;
                _bossPos.y = Random.Range(-3, 4);
            }
            else
            {
                _bossPos.x = Random.Range(-3, 4);
                _bossPos.y = RandomOne() * 3;
            }
            do
            {
                isVertical = Random.Range(0, 2) == 0 ? true : false;
                if (isVertical)
                {
                    _treasurePos.x = RandomOne() * 3;
                    _treasurePos.y = Random.Range(-3, 4);
                }
                else
                {
                    _treasurePos.x = Random.Range(-3, 4);
                    _treasurePos.y = RandomOne() * 3;
                }
            } while (Vector2.Distance(_bossPos, _treasurePos) <= 3);
            _rooms.Add(Vector2.zero, new Room("Basement0", RoomType.Start));
            _rooms.Add(_bossPos, new Room("BossStage", RoomType.Boss));
            _rooms.Add(_treasurePos, new Room("Treasure", RoomType.Treasure));
            foreach (var pos in _basePos)
            {
                _rooms.Add(pos, new Room("Basement" + Random.Range(1, 5).ToString(), RoomType.Normal));
            }
            NormalRoom();
            Debug.Log($"황금방{nearCount(_treasurePos)},보스방{nearCount(_bossPos)}, 방 최대{_rooms.Count}");
        } while (nearCount(_bossPos) > 2 || nearCount(_treasurePos) > 2 || _rooms.Count >12);
        return _rooms;
        
    }
    int RandomOne()
    {
        int i = Random.Range(0, 2);
        i = i == 0 ? -1 : 1;
        return i;
    }
    public void NormalRoom()
    {
        Vector2 bossdir = _basePos[Random.Range(0, 3)];
        _basePos.Remove(bossdir);
        foreach (var pos in FindSimplePath(bossdir, _bossPos))
        {
            if (!_rooms.ContainsKey(pos))
            {
                _rooms.Add(pos, new Room("Basement" + Random.Range(1, 5).ToString(), RoomType.Normal));
            }
        }
        Vector2 tresuredir = _basePos[Random.Range(0, 2)];
        foreach (var pos in FindSimplePath(tresuredir, _treasurePos))
        {
            if (!_rooms.ContainsKey(pos))
            {
                _rooms.Add(pos, new Room("Basement" + Random.Range(1, 5).ToString(), RoomType.Normal));
            }
        }
    }
    List<Vector2> FindSimplePath(Vector2 start, Vector2 end)
    {
        List<Vector2> path = new List<Vector2>();

        int deltaX = Mathf.Abs((int)end.x - (int)start.x);
        int deltaY = Mathf.Abs((int)end.y - (int)start.y);

        int x = (int)start.x;
        int y = (int)start.y;

        for (int i = 0; i <= deltaX + deltaY -2; i++)
        {


            if (x < end.x)
            {
                x++;
            }
            else if (x > end.x)
            {
                x--;
            }

            else if (y < end.y)
            {
                y++;
            }
            else if (y > end.y)
            {
                y--;
            }
            path.Add(new Vector2Int(x, y));
        }

        return path;
    }
}
public class Room
{
    public enum PickUpItems
    {
        Default,
        None,
        Heart0,
        Heart1,
        Heart2,
        Bomb1,
        Bomb2,
        IsUsed,
    }
    public PickUpItems _pickUpItem = PickUpItems.Default;
    string _sceneName;
    RoomType _type;
    GameObject[] _doors;
    bool _isClear;
    public bool IsClear { get { return _isClear; } }    
    public GameObject[] Doors { get { return _doors; } }
    public RoomType Type { get { return _type; } }
    public Room(string sceneName, RoomType type)
    {
        _sceneName = sceneName;
        _type = type;
    }

    public void Load()
    {
        SceneManager.LoadScene(_sceneName);
        if (_type == RoomType.Boss && !_isClear) 
        {
            Time.timeScale = 0f;
            GenericSingleton<UIBase>.Instance.ShowBossIntro(true);

        }
        else
        {
            GenericSingleton<UIBase>.Instance.ShowBossIntro(false);          
            GenericSingleton<UIBase>.Instance.FadeEffect();
            if (_isClear)GenericSingleton<GameManager>.Instance.DelayRoomClear();
        }
    }
    void SetPickUpItem()
    {
        _pickUpItem = (PickUpItems)Random.Range(1, 5);
        GenericSingleton<PickUpItemManager>.Instance.UpdatePickUpItem();
    }
    public void PickUpItemUsed()
    {
        _pickUpItem = PickUpItems.IsUsed;
    }

    public void SetClear()
    {
        _isClear = true;
        if (_type != RoomType.Start && _type != RoomType.Treasure)
        {
            SetPickUpItem();
        }
        
    }

}
public enum RoomType
{
    Start,
    Boss,
    Treasure,
    Normal,
}