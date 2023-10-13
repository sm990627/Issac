using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItemManager : GenericSingleton<PickUpItemManager>
{
    [SerializeField] GameObject _pickUpItem;
    [SerializeField] Sprite[] _heartImg;
    [SerializeField] Sprite[] _bombImg;
     void Start()
     {
        _pickUpItem.GetComponent<PickUpItem>()?.Init(_heartImg, _bombImg);
        UpdatePickUpItem();
     }

    public void UpdatePickUpItem()
    {
        _pickUpItem.GetComponent<PickUpItem>()?.UpdateItemState();
    }

    
}
