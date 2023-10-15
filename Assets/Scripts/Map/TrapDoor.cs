using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    [SerializeField] GameObject _clear;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            _clear.SetActive(true);
            Invoke("GameClear", 0.7f);
        }
    }
    void GameClear()
    {
        GenericSingleton<GameManager>.Instance.SetGameState(GameState.GameClear);
    }
}
