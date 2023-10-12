using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GameManager : GenericSingleton<GameManager>
{
    [SerializeField] Image blackScreen;
    [SerializeField] float fadeDuration = 1.0f;
    [SerializeField] float delayBeforeFadeOut = 1.0f;
    [SerializeField] GameObject[] _pickUpItems;

    Dictionary<Vector2, Room> _rooms;
    float _restartTimer;
    bool _escUI;
    GameState _currentState;
    public GameState CurrentState { get { return _currentState; } }
 


    public enum GameState
    {
        Loading,
        Playing,
        NoEnemies,
        Pause,
        GameOver,
        GameClear
    }

    public void SetGameState(GameState newState)
    {
        _currentState = newState;

        switch (newState)
        {
            case GameState.Loading:
                
                break;

            case GameState.Playing:
                Time.timeScale = 1;
                CheckState();
                
                break;

            case GameState.NoEnemies:
                Time.timeScale = 1;
                SetClear();
                GenericSingleton<Doors>.Instance.DoorOpen();
                break;

            case GameState.Pause:
                Time.timeScale = 0;

                break;

            case GameState.GameOver:
                Time.timeScale = 0;


                break;

            case GameState.GameClear:
                Time.timeScale = 1;

                break;

        }
        Debug.Log(_currentState);
    }
    protected override void OnAwake()                                      
    {
        _rooms = GenericSingleton<RoomManager>.Instance.Init();                              
        GenericSingleton<StageManager>.Instance.Init();                                                
        GenericSingleton<Doors>.Instance.DoorOpen();                       
        GenericSingleton<PlayerCon>.Instance.Init();                        
        GenericSingleton<AttackCon>.Instance.Init();      
        GenericSingleton<UIBase>.Instance.Init();
        GenericSingleton<UIBase>.Instance.HpBarInit();
        _currentState = GameState.Loading;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_escUI)
            {
                SetGameState(GameState.Pause);
                GenericSingleton<UIBase>.Instance.ShowEscUI(true);
                _escUI = true;
            }
            else
            {
                SetGameState(GameState.Playing);
                GenericSingleton<UIBase>.Instance.ShowEscUI(false);
                _escUI = false;
            }
        }
        if (Input.GetKey(KeyCode.R))
        {
            _restartTimer += Time.deltaTime;
            if (_restartTimer > 2)
            {
                Debug.Log("재시작");
                _restartTimer = 0;
                Restart();
            }
        }
        if (Input.GetKey(KeyCode.L))
        {
            RoomClear();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            _restartTimer = 0;
        }

        
     
    }
    
    
    void Restart() 
    {                             
        GenericSingleton<RoomManager>.Instance.Init();                     
        _rooms = GenericSingleton<RoomManager>.Instance.Rooms;                                    
        RoomClear();
        GenericSingleton<StageManager>.Instance.ResetCurrentPos();                   
        GenericSingleton<StageManager>.Instance.DoorInit();
        GenericSingleton<PlayerCon>.Instance.Init();                       
        GenericSingleton<AttackCon>.Instance.Init();                                         
        GenericSingleton<UIBase>.Instance.Init();
        GenericSingleton<UIBase>.Instance.HpUpdate();
        GenericSingleton<Doors>.Instance.DoorOpen();                      
    }
    public void CheckState()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") == null && GameObject.FindGameObjectWithTag("Boss") == null)
        {
            SetGameState(GameState.NoEnemies);
        }
        else
        {
            Debug.Log("문닫기");
            GenericSingleton<Doors>.Instance.DoorClose();
        }
    }
    void SetClear()   //방 클리어로 만들기
    {
        GenericSingleton<StageManager>.Instance.CurrentRoom.SetClear();
    }

    void RoomClear()
    {
        SetGameState(GameState.NoEnemies);
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemys)
        {
            enemy.SetActive(false);
        }
    }
    public void FadeEffect()
    {
        StartCoroutine(FadeGameEffect(0.1f,0.3f));
    }
    public void DelayRoomClear()
    {   
        Invoke("RoomClear", 0.01f);
    }


    private IEnumerator FadeGameEffect(float delaytime, float fadeDuration)
    {
        // Display black screen
        SetGameState(GameState.Loading);
        blackScreen.gameObject.SetActive(true);
        blackScreen.color = Color.black;

        // Wait for a brief moment
        yield return new WaitForSeconds(delaytime);

        // Fade out the black screen
        float elapsedTime = 0;
        Color startColor = blackScreen.color;
        Color endColor = new Color(0, 0, 0, 0); // Fully transparent
        while (elapsedTime < fadeDuration)
        {
            blackScreen.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
  
        blackScreen.gameObject.SetActive(false); // Deactivate the black screen
        SetGameState(GameState.Playing);
    }
}