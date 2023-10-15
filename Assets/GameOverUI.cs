
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] Sprite[] _enemyImages;
    [SerializeField] Image _enemyImage;


    public void SetEnemyImage(string enemy)
    {
        switch (enemy)
        {
            case "Horf":
                {
                    _enemyImage.sprite = _enemyImages[0];
                }
                break;
            case "Red Maw":
                {
                    _enemyImage.sprite = _enemyImages[1];
                }
                break;
            case "Mask":
                {
                    _enemyImage.sprite = _enemyImages[2];
                }
                break;
            case "Heart":
                {
                    _enemyImage.sprite = _enemyImages[3];
                }
                break;
            case "Monstro":
                {
                    _enemyImage.sprite = _enemyImages[4];
                }
                break;
        }
    }
}
