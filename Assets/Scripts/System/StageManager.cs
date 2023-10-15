using System.Collections.Generic;
using UnityEngine;


public class StageManager : GenericSingleton<StageManager>
{
    Dictionary<Vector2, Room> _rooms = new Dictionary<Vector2,Room>();
    [SerializeField] Sprite[] _doorSprites;
    [SerializeField] GameObject _upClose;
    [SerializeField] GameObject _rightClose;
    [SerializeField] GameObject _downClose;
    [SerializeField] GameObject _leftClose;

    GameObject _upDoor;
    GameObject _leftDoor;
    GameObject _rightDoor;
    GameObject _downDoor;

    Vector2 _currentPos = Vector2.zero;
    public Vector2 CurrentPos { get { return _currentPos; } }
    Room _currentRoom;
    public Room CurrentRoom { get { return _currentRoom; } }
    public void Init()
    {
        _rooms = GenericSingleton<RoomManager>.Instance.Rooms;
        _upDoor =  GenericSingleton<Doors>.Instance._doors[0];
        _rightDoor = GenericSingleton<Doors>.Instance._doors[1];
        _downDoor = GenericSingleton<Doors>.Instance._doors[2];
        _leftDoor = GenericSingleton<Doors>.Instance._doors[3];
        ResetCurrentPos();
        _currentRoom = GetRoom(_currentPos);
        GenericSingleton<PickUpItemManager>.Instance.UpdatePickUpItem();
        DoorInit();
        GenericSingleton<Doors>.Instance.GetComponent<Doors>().DoorOpen();
    }
    public void LoadInit()
    {
        _upDoor = GenericSingleton<Doors>.Instance._doors[0];
        _rightDoor = GenericSingleton<Doors>.Instance._doors[1];
        _downDoor = GenericSingleton<Doors>.Instance._doors[2];
        _leftDoor = GenericSingleton<Doors>.Instance._doors[3];
    }
    public void ResetCurrentPos()
    {
        _currentPos = Vector2.zero;
        GetRoom(_currentPos).Load();
    }
    public void DoorInit()
    {
        RoomType currentRoomType = GetRoom(_currentPos).Type;      
        switch (currentRoomType)
        {
            case RoomType.Normal:
            case RoomType.Start:
                {
                    SetDoorState(_currentPos + Vector2.up, _upDoor, _upClose,doorSpriteInit(_currentPos + Vector2.up));
                    SetDoorState(_currentPos + Vector2.right, _rightDoor, _rightClose, doorSpriteInit(_currentPos + Vector2.right));
                    SetDoorState(_currentPos + Vector2.down, _downDoor, _downClose, doorSpriteInit(_currentPos + Vector2.down));
                    SetDoorState(_currentPos + Vector2.left, _leftDoor, _leftClose, doorSpriteInit(_currentPos + Vector2.left));
                    break;
                }
            case RoomType.Boss:
                {
                    SetDoorState(_currentPos + Vector2.up, _upDoor, _upClose, _doorSprites[1]);
                    SetDoorState(_currentPos + Vector2.right, _rightDoor, _rightClose, _doorSprites[1]);
                    SetDoorState(_currentPos + Vector2.down, _downDoor, _downClose, _doorSprites[1]);
                    SetDoorState(_currentPos + Vector2.left, _leftDoor, _leftClose, _doorSprites[1]);
                    break;
                }
            case RoomType.Treasure:
                {
                    SetDoorState(_currentPos + Vector2.up, _upDoor, _upClose, _doorSprites[2]);
                    SetDoorState(_currentPos + Vector2.right, _rightDoor, _rightClose, _doorSprites[2]);
                    SetDoorState(_currentPos + Vector2.down, _downDoor, _downClose, _doorSprites[2]);
                    SetDoorState(_currentPos + Vector2.left, _leftDoor, _leftClose, _doorSprites[2]);
                    break;
                }
        }
        
    }
  
    void SetDoorState(Vector2 targetPos, GameObject door, GameObject close, Sprite doorSprite)
    {

        if (_rooms.ContainsKey(targetPos))
        {
            door.SetActive(true);
            close.SetActive(false);
            door.GetComponent<SpriteRenderer>().sprite = doorSprite;
        }
        else
        {
            door.SetActive(false);
            close.SetActive(true);
        }
    }
    Sprite doorSpriteInit(Vector3 nextpos)
    {
        Room nextRoom = GetRoom(nextpos);
        if (nextRoom.Type == RoomType.Boss)
        {
            return _doorSprites[1];
        }
        else if (nextRoom.Type == RoomType.Treasure)
        {
            return _doorSprites[2];
        }
        else
        {
            return _doorSprites[0];
            
        }
        

    }
    Room GetRoom(Vector2 pos)
    {
        if ( _rooms.TryGetValue(pos, out Room targetRoom))
        {
            return targetRoom;
        }
       return new Room("",RoomType.Start);
    } 
    public void ChangeScene(ExitDirection dir)
    {
        switch (dir)
        {
            case ExitDirection.up:
                {                    
                    GetRoom(_currentPos + Vector2.up).Load();                        
                    _currentPos += Vector2.up;
                    GenericSingleton<PlayerCon>.Instance.transform.position = new Vector2(0f, -1.9f);
                    break;
                }
            case ExitDirection.right:
                {
                    GetRoom(_currentPos + Vector2.right).Load();
                    _currentPos += Vector2.right;
                    GenericSingleton<PlayerCon>.Instance.transform.position = new Vector2(-2.7f, 0);
                     break;
                }
            case ExitDirection.down:
                {
                    GetRoom(_currentPos + Vector2.down).Load();
                    _currentPos += Vector2.down;
                    GenericSingleton<PlayerCon>.Instance.transform.position = new Vector2(0f, 1.9f);
                    
                    break;
                }
            case ExitDirection.left:
                {
                    GetRoom(_currentPos + Vector2.left).Load();
                    _currentPos += Vector2.left;
                    GenericSingleton<PlayerCon>.Instance.transform.position = new Vector2(2.7f, 0);
                    break;
                }

        }
        DoorInit();
        if (_rooms.TryGetValue(_currentPos, out Room room)) _currentRoom = room;
        GenericSingleton<PickUpItemManager>.Instance.UpdatePickUpItem();
        GenericSingleton<UIBase>.Instance.UpdateMiniMap();
        GenericSingleton<DataManager>.Instance.SaveData();
    }
    public void LoadCurrentPos(Vector2 pos,Vector2 Ppos)
    {
        _rooms = GenericSingleton<RoomManager>.Instance.Rooms;
        _currentPos = pos;
        _currentRoom = GetRoom(_currentPos);
        GenericSingleton<PlayerCon>.Instance.SetPosition(Ppos);
        _currentRoom.Load();
    }

}

