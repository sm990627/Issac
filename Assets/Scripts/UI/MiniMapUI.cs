using System.Collections.Generic;
using UnityEngine;

public class MiniMapUI : MonoBehaviour
{
    [SerializeField] GameObject[] roomIconPrefab; // ���� ��Ÿ���� ������ ������
    [SerializeField] RectTransform content;      // ��ũ�Ѻ��� Content Transform
    Dictionary<Vector2, Room> rooms;


    public void GenerateMiniMap()
    {
        ClearMap();
        rooms = GenericSingleton<RoomManager>.Instance.GetComponent<RoomManager>().Rooms;
        foreach (var temp in rooms)
        {
            Vector2 roomPosition = temp.Key;
            Room room = temp.Value;
            if (room.Type == RoomType.Start)
            {
                GameObject roomIcon = Instantiate(roomIconPrefab[1], content);
                RectTransform Transform = roomIcon.GetComponent<RectTransform>();
                Transform.anchoredPosition = new Vector2(roomPosition.x * 45, roomPosition.y * 45);
            }
            else
            {
                GameObject roomIcon = Instantiate(roomIconPrefab[0], content);
                RectTransform Transform = roomIcon.GetComponent<RectTransform>();
                Transform.anchoredPosition = new Vector2(roomPosition.x * 45, roomPosition.y * 45);
            }
            if(room.Type == RoomType.Boss)
            {
                GameObject roomIcon = Instantiate(roomIconPrefab[2], content);
                RectTransform Transform = roomIcon.GetComponent<RectTransform>();
                Transform.anchoredPosition = new Vector2(roomPosition.x * 45, roomPosition.y * 45);
            }
            else if (room.Type == RoomType.Treasure)
            {
                GameObject roomIcon = Instantiate(roomIconPrefab[3], content);
                RectTransform Transform = roomIcon.GetComponent<RectTransform>();
                Transform.anchoredPosition = new Vector2(roomPosition.x * 45, roomPosition.y * 45);
            }
        }
    }
    public void UpdateMiniMap()
    {
        ClearMap();
        Vector2 currentPos = GenericSingleton<StageManager>.Instance.GetComponent<StageManager>().CurrentPos;
        foreach (var temp in rooms)
        {
            Vector2 roomPosition = temp.Key;
            Room room = temp.Value;
            if (roomPosition == currentPos || room.IsClear)
            {
                GameObject roomIcon = Instantiate(roomIconPrefab[1], content);
                RectTransform Transform = roomIcon.GetComponent<RectTransform>();
                Transform.anchoredPosition = new Vector2((roomPosition.x - currentPos.x) * 45, (roomPosition.y -currentPos.y) * 45);
            }
            else
            {
                GameObject roomIcon = Instantiate(roomIconPrefab[0], content);
                RectTransform Transform = roomIcon.GetComponent<RectTransform>();
                Transform.anchoredPosition = new Vector2((roomPosition.x - currentPos.x) * 45, (roomPosition.y - currentPos.y) * 45);
            }
            if (room.Type == RoomType.Boss)
            {
                GameObject roomIcon = Instantiate(roomIconPrefab[2], content);
                RectTransform Transform = roomIcon.GetComponent<RectTransform>();
                Transform.anchoredPosition = new Vector2((roomPosition.x - currentPos.x) * 45, (roomPosition.y - currentPos.y) * 45);
            }
            else if (room.Type == RoomType.Treasure)
            {
                GameObject roomIcon = Instantiate(roomIconPrefab[3], content);
                RectTransform Transform = roomIcon.GetComponent<RectTransform>();
                Transform.anchoredPosition = new Vector2((roomPosition.x - currentPos.x) * 45, (roomPosition.y - currentPos.y) * 45);
            }
           
        }
    }
    void ClearMap()
    {
        GameObject[] minimaps = GameObject.FindGameObjectsWithTag("MiniMap");
        foreach (GameObject minimap in minimaps)
        {
            Destroy(minimap);
        }
    }

}
