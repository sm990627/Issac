
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GameManager : GenericSingleton<GameManager>
{

    Dictionary<Vector2, Room> _rooms;
    float _restartTimer;
    bool _escUI;
    [SerializeField]GameState _currentState;
    public GameState CurrentState { get { return _currentState; } }
 


    public enum GameState
    {
        Title,
        Loading,
        GameStart,
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
            case GameState.Title:
                break;

            case GameState.Loading:
                
                break;

            case GameState.GameStart:
                GameStart();
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
        SetGameState(_currentState);

    }
    public void GameStart()
    {
        //nstantiate(_roomManager);
        _rooms = GenericSingleton<RoomManager>.Instance.Init();
        //Instantiate(_stageManager);
        GenericSingleton<StageManager>.Instance.Init();
        //Instantiate(_doors);
        GenericSingleton<Doors>.Instance.DoorOpen();
        //Instantiate(_player);
        GenericSingleton<PlayerCon>.Instance.Init();
        GenericSingleton<AttackCon>.Instance.Init();
        //Instantiate(_uiBase);
        GenericSingleton<UIBase>.Instance.Init();
        GenericSingleton<UIBase>.Instance.HpBarInit();
        _currentState = GameState.Loading;
    }

    private void Update()
    {
        if (_currentState == GameState.Playing || _currentState == GameState.NoEnemies)
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
    public void DelayRoomClear()
    {   
        Invoke("RoomClear", 0.01f);
    }


    
}