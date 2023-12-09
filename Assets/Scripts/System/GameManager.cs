
using System.Collections.Generic;
using System.IO;
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
    

    public void SetGameState(GameState newState)
    {
        if (_currentState != newState) _currentState = newState;

        switch (newState)
        {
            case GameState.Title:
                Time.timeScale = 1;
                break;

            case GameState.Loading:
                
                break;

            case GameState.GameStart:
                {
                    string filePath = Path.Combine(Application.persistentDataPath, "GameData.json");
                    if (File.Exists(filePath)) File.Delete(filePath);
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
                    Time.timeScale = 1;
                    string filePath = Path.Combine(Application.persistentDataPath, "GameData.json");
                    if (File.Exists(filePath)) File.Delete(filePath);
                }
                break;

            case GameState.GameClear:
                {
                    Time.timeScale = 1;
                    string filePath = Path.Combine(Application.persistentDataPath, "GameData.json");
                    if (File.Exists(filePath)) File.Delete(filePath);
                    GenericSingleton<UIBase>.Instance.GoTitle();
                }
                break;

        }
       
    }
    public void Title()
    {
        SetGameState(GameState.Title);
        GenericSingleton<SoundManager>.Instance.Setitle();
        GenericSingleton<PickUpItemManager>.Instance.gameObject.SetActive(false);
        GenericSingleton<Doors>.Instance.TrapDoor(false); 
        GenericSingleton<Doors>.Instance.gameObject.SetActive(false);
        GenericSingleton<PlayerCon>.Instance.gameObject.SetActive(false);
        GenericSingleton<UIBase>.Instance.Title();
        _escUI = false;
    }
    protected override void OnAwake()                                      
    {
        GenericSingleton<SoundManager>.Instance.Init();
        Title();

    }
    public void GameStart()
    {
        _rooms = GenericSingleton<RoomManager>.Instance.Init();
        GenericSingleton<Doors>.Instance.gameObject.SetActive(true);
        GenericSingleton<StageManager>.Instance.Init();
        GenericSingleton<PlayerCon>.Instance.gameObject.SetActive(true);
        GenericSingleton<PlayerCon>.Instance.Init();
        GenericSingleton<AttackCon>.Instance.Init();
        GenericSingleton<UIBase>.Instance.Init();
        GenericSingleton<SoundManager>.Instance.SetBasement();
        GenericSingleton<PickUpItemManager>.Instance.gameObject.SetActive(true);
        GenericSingleton<PickUpItemManager>.Instance.Init();
        _currentState = GameState.Loading;
    }
    public void LoadGame()
    {
        GenericSingleton<PlayerCon>.Instance.LoadStart();
        GenericSingleton<DataManager>.Instance.LoadData();     
        GenericSingleton<Doors>.Instance.gameObject.SetActive(true);
        GenericSingleton<StageManager>.Instance.LoadInit();
        GenericSingleton<StageManager>.Instance.DoorInit();
        GenericSingleton<AttackCon>.Instance.Init();
        GenericSingleton<UIBase>.Instance.Init();
        GenericSingleton<UIBase>.Instance.UpdateMiniMap();
        GenericSingleton<SoundManager>.Instance.SetBasement();
        GenericSingleton<PickUpItemManager>.Instance.gameObject.SetActive(true);
        GenericSingleton<PickUpItemManager>.Instance.Init();
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
                        _restartTimer = 0;
                        Restart();
                    }
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
        GenericSingleton<SoundManager>.Instance.SetBasement();
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
public enum GameState
{
    Title,
    Loading,
    GameStart,
    Playing,
    GameOver,
    GameClear,
    EnemiesOff = 100,
    EnemiesOn = 101,
    Pause = 102,
}