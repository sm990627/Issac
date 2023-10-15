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
    
        gameObject.SetActive(true);
        gameObject.transform.position = Vector3.zero;
        switch (GenericSingleton<StageManager>.Instance.CurrentRoom._pickUpItem)
        {
            
            case PickUpItems.Heart0:
                {
                    _ren.sprite = _heartImg[0];
                    _hpAmount = 0.5f;
                    _type = PickUp.Heart;
                }
                break;

            case PickUpItems.Heart1:
                {
                    _ren.sprite = _heartImg[1];
                    _hpAmount = 1;
                    _type = PickUp.Heart;
                }
                break;


            case PickUpItems.Heart2:
                {
                    _ren.sprite = _heartImg[2];
                    _hpAmount = 2;
                    _type = PickUp.Heart;
                }
                break;

            case PickUpItems.Bomb1:
                {
                    _ren.sprite = _bombImg[0];
                    _bombAmount = 1;
                    _type = PickUp.Bomb;
                }
                break;

            case PickUpItems.Bomb2:
                {
                    _ren.sprite = _bombImg[1];
                    _bombAmount = 2;
                    _type = PickUp.Bomb;

                }
                break;
            default:
                {
                    _type = PickUp.None;
                    gameObject.SetActive(false);
                }
                break;


        }
     }
}
