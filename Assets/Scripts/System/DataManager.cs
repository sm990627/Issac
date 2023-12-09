using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : GenericSingleton<DataManager>
{
    GameDataList _gameDatas = null;
    public void SaveData()
    {
        _gameDatas = new GameDataList();
        _gameDatas.Datas.Add(new GameData());
        GameData data = _gameDatas.Datas[0];
        data.SavePlayerStatData(GenericSingleton<PlayerCon>.Instance.Pstat);
        data.SaveRoomData(GenericSingleton<RoomManager>.Instance.Rooms);
        data.SaveCurrentPos(GenericSingleton<StageManager>.Instance.CurrentPos, GenericSingleton<PlayerCon>.Instance.gameObject.transform.position);
        data.SaveItemData(GenericSingleton<PlayerCon>.Instance.ItemIdx);
        string json = JsonUtility.ToJson(_gameDatas);
        string filePath = Path.Combine(Application.persistentDataPath, "GameData.json");
        File.WriteAllText(filePath, json);
    }

    public void LoadData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "GameData.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            _gameDatas = JsonUtility.FromJson<GameDataList>(json);
            GameData data = _gameDatas.Datas[0];
            GenericSingleton<RoomManager>.Instance.LoadRoomData(data.RoomDatas);
            GenericSingleton<StageManager>.Instance.LoadCurrentPos(data.CurrentPos,data.PlayerPos);
            GenericSingleton<PlayerCon>.Instance.Pstat.LoadPlayerStat(data.PlayerStatData);
            GenericSingleton<PlayerCon>.Instance.LoadItemData(data.Items);
        }

    }
   
}

[Serializable]
public class GameDataList
{
    public List<GameData> Datas = new List<GameData>();

}
[Serializable]
public class GameData
{

    public List<RoomData> RoomDatas = new List<RoomData>();
    public PlayerStatData PlayerStatData = new PlayerStatData();
    public Vector2 CurrentPos;
    public Vector2 PlayerPos;
    public List<int> Items;


    public void SavePlayerStatData(PlayerStat Pstat)
    {
        PlayerStatData.MaxHp = Pstat.MaxHp;
        PlayerStatData.Hp = Pstat.Hp;
        PlayerStatData.Speed = Pstat.Speed;
        PlayerStatData.Power = Pstat.Power;
        PlayerStatData.AttackSpeed = Pstat.AttackSpeed;
        PlayerStatData.BulletCnt = Pstat.BulletCnt;
        PlayerStatData.BulletSpeed = Pstat.BulletSpeed;
        PlayerStatData.Range = Pstat.Range;
    }
    public void SaveRoomData(Dictionary<Vector2, Room> Rooms)
    {
        foreach (var room in Rooms)
        {
            RoomData roomData = new RoomData();
            roomData =  room.Value.ConvertToData();
            roomData.Pos = room.Key;
            RoomDatas.Add(roomData);
        }
    }
    public void SaveCurrentPos(Vector2 pos,Vector2 Ppos)
    {
        CurrentPos = pos;
        PlayerPos = Ppos;
    }
    public void SaveItemData(List<int> items)
    {
        Items = items;
    }



}
[Serializable]
public class PlayerStatData
{
    public float MaxHp;
    public float Hp;
    public float Speed;
    public float Power;
    public float AttackSpeed;
    public int BulletCnt;
    public float BulletSpeed;
    public float Range;
}

[Serializable]
public class RoomData
{
    public Vector2 Pos;
    public string SceneName;
    public int TresureIdx;
    public bool IsClear;
    public RoomType Type;
    public PickUpItems PickUpItem;
}
