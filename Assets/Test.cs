using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Vector2 start = Vector2.zero;
    Vector2 end = new Vector2(3,5);
    private void Start()
    {
        foreach (var temp in FindSimplePath(start,end))
        {
            Debug.Log(temp);
        }
        ;
    }
    List<Vector2> FindSimplePath(Vector2 start, Vector2 end)
    {
        List<Vector2> path = new List<Vector2>();

        int deltaX = Mathf.Abs((int)end.x - (int)start.x);
        int deltaY = Mathf.Abs((int)end.y - (int)start.y);

        int x = (int)start.x;
        int y = (int)start.y;

        for (int i = 0; i <= Mathf.Max(deltaX, deltaY); i++)
        {
            path.Add(new Vector2Int(x, y));

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
        }

        return path;
    }

}
