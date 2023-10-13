using System.Collections.Generic;
using UnityEngine;

public class MiniMapUI : MonoBehaviour
{
    [SerializeField] GameObject[] roomIconPrefab; 
    [SerializeField] RectTransform content;      
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
                Transform.anchoredPosition = new Vector2(roomPosition.x * 33, roomPosition.y * 33);
            }
            else
            {
                GameObject roomIcon = Instantiate(roomIconPrefab[0], content);
                RectTransform Transform = roomIcon.GetComponent<RectTransform>();
                Transform.anchoredPosition = new Vector2(roomPosition.x * 33, roomPosition.y * 33);
            }
            if(room.Type == RoomType.Boss)
            {
                GameObject roomIcon = Instantiate(roomIconPrefab[2], content);
                RectTransform Transform = roomIcon.GetComponent<RectTransform>();
                Transform.anchoredPosition = new Vector2(roomPosition.x * 33, roomPosition.y * 33);
            }
            else if (room.Type == RoomType.Treasure)
            {
                GameObject roomIcon = Instantiate(roomIconPrefab[3], content);
                RectTransform Transform = roomIcon.GetComponent<RectTransform>();
                Transform.anchoredPosition = new Vector2(roomPosition.x * 33, roomPosition.y * 33);
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
                Transform.anchoredPosition = new Vector2((roomPosition.x - currentPos.x) * 33, (roomPosition.y -currentPos.y) * 33);
            }
            else
            {
                GameObject roomIcon = Instantiate(roomIconPrefab[0], content);
                RectTransform Transform = roomIcon.GetComponent<RectTransform>();
                Transform.anchoredPosition = new Vector2((roomPosition.x - currentPos.x) * 33, (roomPosition.y - currentPos.y) * 33);
            }
            if (room.Type == RoomType.Boss)
            {
                GameObject roomIcon = Instantiate(roomIconPrefab[2], content);
                RectTransform Transform = roomIcon.GetComponent<RectTransform>();
                Transform.anchoredPosition = new Vector2((roomPosition.x - currentPos.x) * 33, (roomPosition.y - currentPos.y) * 33);
            }
            else if (room.Type == RoomType.Treasure)
            {
                GameObject roomIcon = Instantiate(roomIconPrefab[3], content);
                RectTransform Transform = roomIcon.GetComponent<RectTransform>();
                Transform.anchoredPosition = new Vector2((roomPosition.x - currentPos.x) * 33, (roomPosition.y - currentPos.y) * 33);
            }
            if ((int)room._pickUpItem >= 2 && (int)room._pickUpItem <= 4)
            {
                GameObject roomIcon = Instantiate(roomIconPrefab[4], content);
                RectTransform Transform = roomIcon.GetComponent<RectTransform>();
                Transform.anchoredPosition = new Vector2((roomPosition.x - currentPos.x) * 33, (roomPosition.y - currentPos.y) * 33);
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
