using System;
using System.Collections.Generic;
using UnityEngine;


public class GameProcessManager : GenericSingleton<GameProcessManager>
{
    [SerializeField] GameObject UI;
    Dictionary<GameState, FSMState> _state = new Dictionary<GameState, FSMState>();
    Action _callback;
    FSM fsm = null;
    private void Awake()
    {
        _state.Add(GameState.Playing, new PlayingState(fsm));
        _state.Add(GameState.Loading, new LoadingState(fsm));
        _state.Add(GameState.Pause, new PauseState(fsm));
        _state.Add(GameState.BaseState, new BaseState(fsm));
        _state.Add(GameState.GameOver, new GameOverState(fsm));
        _state.Add(GameState.GameClear, new GameClearState(fsm));
        _state.Add(GameState.Lobby, new LobbyState(fsm));
    }

    protected override void OnAwake()
    {
        base.OnAwake();
    }
    void Start()
    {
        //로딩상태로 전환
        ChangeStateByEnum(GameState.Loading);
    }

    void Update()
    {
        fsm.DoLoop(); //반복실행을 매프라임마다
    }
    public void ChangeStateByEnum(GameState type)
    {
        fsm.ChangeState(_state[type]);
    }
}
public enum GameState
{
    Playing,
    Loading,
    Pause,
    BaseState,
    GameOver,
    GameClear,
    Lobby,
}
public class FSM
{
    protected FSMState _currentState;
    public void ChangeState(FSMState state)
    {
        _currentState?.OnExit();
        _currentState = state;
        _currentState.OnEnter();
    }

    public void DoLoop() => _currentState.DoLoop();

}
public abstract class FSMState
{
    protected FSM _baseFSM;
    public FSMState(FSM fsm) { _baseFSM = fsm; }
    public abstract void OnEnter();
    public abstract void DoLoop();
    public abstract void OnExit();
}
public class PlayingState : FSMState
{
    public PlayingState(FSM fsm) : base(fsm) { }
    public override void OnEnter()
    {

    }
    public override void DoLoop()
    {


    }
    public override void OnExit()
    {

    }

}
public class LoadingState : FSMState
{
    public LoadingState(FSM fsm) : base(fsm) { }
    public override void OnEnter()
    {

    }
    public override void DoLoop()
    {


    }
    public override void OnExit()
    {

    }

}
public class PauseState : FSMState
{
    public PauseState(FSM fsm) : base(fsm) { }
    public override void OnEnter()
    {
        Time.timeScale = 0;
    }
    public override void DoLoop()
    {


    }
    public override void OnExit()
    {
        Time.timeScale = 1;
    }

}
public class BaseState : FSMState
{
    public BaseState(FSM fsm) : base(fsm) { }
    public override void OnEnter()
    {

    }
    public override void DoLoop()
    {


    }
    public override void OnExit()
    {

    }

}
public class GameOverState : FSMState
{
    public GameOverState(FSM fsm) : base(fsm) { }
    public override void OnEnter()
    {

    }
    public override void DoLoop()
    {


    }
    public override void OnExit()
    {

    }

}
public class GameClearState : FSMState
{
    public GameClearState(FSM fsm) : base(fsm) { }
    public override void OnEnter()
    {

    }
    public override void DoLoop()
    {


    }
    public override void OnExit()
    {

    }

}
public class LobbyState : FSMState
{
    public LobbyState(FSM fsm) : base(fsm) { }
    public override void OnEnter()
    {

    }
    public override void DoLoop()
    {


    }
    public override void OnExit()
    {

    }

}
