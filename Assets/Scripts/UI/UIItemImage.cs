using UnityEngine;
using UnityEngine.UI;

public class UIItemImage : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;

    public void Init(int itemIdx)
    {
        GetComponentInChildren<Image>().sprite = sprites[itemIdx];
    } 
}
