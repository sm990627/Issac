
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


    GameState _lastState;
    public enum GameState
    {
        Title ,
        Loading,
        GameStart,
        Playing,
        GameOver,
        GameClear,
        EnemiesOff = 100,
        EnemiesOn  = 101,
        Pause      = 102,
    }

    public void SetGameState(GameState newState)
    {
        if (_currentState != newState) _currentState = newState;

        switch (newState)
        {
            case GameState.Title:
                break;

            case GameState.Loading:
                
                break;

            case GameState.GameStart:
                {
                    GameStart();
                }
                break;

            case GameState.Playing:
                {
                    Time.timeScale = 1;
                    CheckState();
                }
                break;
            case GameState.EnemiesOn:
                {
                    Time.timeScale = 1;
                    GenericSingleton<Doors>.Instance.DoorClose();
                }
                break;

            case GameState.EnemiesOff:
                {
                    Time.timeScale = 1;
                    GenericSingleton<StageManager>.Instance.CurrentRoom.SetClear();
                    GenericSingleton<Doors>.Instance.DoorOpen();
                }
                
                break;

            case GameState.Pause:
                {
                    Time.timeScale = 0;
                }
                break;

            case GameState.GameOver:
                {
                    Time.timeScale = 0; 
                }
                break;

            case GameState.GameClear:
                {
                    Time.timeScale = 1;
                }
                break;

        }
        Debug.Log(_currentState);
    }
    public void Title()
    {
        SetGameState(GameState.Title);
        GenericSingleton<PlayerCon>.Instance.gameObject.SetActive(false);
        GenericSingleton<UIBase>.Instance.Title();
    }
    protected override void OnAwake()                                      
    {
        Title();

    }
    public void GameStart()
    {
        _rooms = GenericSingleton<RoomManager>.Instance.Init();
        GenericSingleton<StageManager>.Instance.Init();
        GenericSingleton<Doors>.Instance.DoorOpen();
        GenericSingleton<PlayerCon>.Instance.gameObject.SetActive(true);
        GenericSingleton<PlayerCon>.Instance.Init();
        GenericSingleton<AttackCon>.Instance.Init();
        GenericSingleton<UIBase>.Instance.Init();
        GenericSingleton<UIBase>.Instance.HpBarInit();
        GenericSingleton<SoundManager>.Instance.SetBasement();
        _currentState = GameState.Loading;
    }

    private void Update()
    {
        if (((int)_currentState) >= 100)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!_escUI)
                {
                    _lastState = _currentState;
                    SetGameState(GameState.Pause);
                    GenericSingleton<UIBase>.Instance.ShowEscUI(true);
                    _escUI = true;
                }
                else if(!GenericSingleton<UIBase>.Instance.OptionUI.activeSelf)
                {
                    ResumeGame();
                }
            }
            if (_currentState != GameState.Pause)
            {
                if (Input.GetKey(KeyCode.R))
                {
                    _restartTimer += Time.deltaTime;
                    if (_restartTimer > 2)
                    {
                        Debug.Log("¿ÁΩ√¿€");
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
        

    }
    public void ResumeGame()
    {
        SetGameState(_lastState);
        GenericSingleton<UIBase>.Instance.ShowEscUI(false);
        _escUI = false;
    }
    
    void Restart() 
    {                             
        GenericSingleton<RoomManager>.Instance.Init();                     
        _rooms = GenericSingleton<RoomManager>.Instance.Rooms;                                    
        RoomClear();
        GenericSingleton<StageManager>.Instance.Init();
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
            SetGameState(GameState.EnemiesOff);
        }
        else
        {
            SetGameState(GameState.EnemiesOn);
        }

    }


    void RoomClear()
    {
        SetGameState(GameState.EnemiesOff);
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemys)
        {
            enemy.SetActive(false);
        }
    }
    public void DelayRoomClear()
    {   
        Invoke("RoomClear", 0.2f);
    }


    
}