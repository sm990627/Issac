using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    Sprite[] _heartImg;
    Sprite[] _bombImg;

    float _hpAmount;
    int _bombAmount;
    SpriteRenderer _ren;
    PickUp _type;
    public PickUp Type { get { return _type; } }

    public enum PickUp
    {
        None,
        Heart,
        Bomb,
    }

    private void Awake()
    {
        Debug.Log("pickup item");
    }

    public void Init(Sprite[] heart, Sprite[] bomb)
     {
        _ren = GetComponent<SpriteRenderer>();
        _heartImg = heart;
        _bombImg = bomb;
     }    
     public float GetHpAmount()
     {
        float resultAmout = _hpAmount;
        GenericSingleton<StageManager>.Instance.CurrentRoom.PickUpItemUsed();
        UpdateItemState();
        return resultAmout;
     } 
     public int GetBombAmount()
     {
        int resultAmout = _bombAmount;
        GenericSingleton<StageManager>.Instance.CurrentRoom.PickUpItemUsed();
        UpdateItemState();
        return resultAmout;
     }
     public void UpdateItemState()
     {
        Debug.Log(GenericSingleton<StageManager>.Instance.CurrentRoom._pickUpItem);
        gameObject.SetActive(true);
        switch (GenericSingleton<StageManager>.Instance.CurrentRoom._pickUpItem)
        {
            
            case Room.PickUpItems.Heart0:
                {
                    Debug.Log("ÇÏÆ® ¹Ý°³");
                    _ren.sprite = _heartImg[0];
                    _hpAmount = 0.5f;
                    _type = PickUp.Heart;
                    break;
                }

            case Room.PickUpItems.Heart1:
                {
                    Debug.Log("ÇÏÆ® ÇÑ°³");
                    _ren.sprite = _heartImg[1];
                    _hpAmount = 1;
                    _type = PickUp.Heart;
                    break;
                }
                

            case Room.PickUpItems.Heart2:
                {
                    Debug.Log("ÇÏÆ® µÎ°³");
                    _ren.sprite = _heartImg[2];
                    _hpAmount = 2;
                    _type = PickUp.Heart;
                    break;
                }
                
            case Room.PickUpItems.Bomb1:
                {
                    Debug.Log("ÆøÅº ÇÑ°³");
                    _ren.sprite = _bombImg[0];
                    _bombAmount = 1;
                    _type = PickUp.Bomb;
                    break;
                }
                
            case Room.PickUpItems.Bomb2:
                {
                    Debug.Log("ÆøÅº µÎ°³");
                    _ren.sprite = _bombImg[1];
                    _bombAmount = 2;
                    _type = PickUp.Bomb;
                    break;

                }
            default:
                {
                    _type = PickUp.None;
                    Debug.Log("²¨Áü");
                    gameObject.SetActive(false);
                    Debug.Log("²¨Áü2");
                    break;
                }
               

        }
     }
}
