using System;
using DIKUArcade.State;
using DIKUArcade.Events;


namespace Breakout.BreakoutStates;
public class StateMachine : IGameEventProcessor {
    public IGameState ActiveState { get; private set; }
    public StateMachine() {
        BreakoutBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
        BreakoutBus.GetBus().Subscribe(GameEventType.InputEvent, this);
        ActiveState = MainMenu.GetInstance();
        GameRunning.GetInstance();
        GamePaused.GetInstance();
        GameLost.GetInstance();
        GameWon.GetInstance();
        }

    public void SwitchState(GameStateType state) {
        switch (state) {
            case GameStateType.GameRunning:
                ActiveState = GameRunning.GetInstance();
                break;
            case GameStateType.GamePaused:
                ActiveState = GamePaused.GetInstance();
                break;
            case GameStateType.MainMenu:
                ActiveState = MainMenu.GetInstance();
                break;
            case GameStateType.GameLost:
                ActiveState = GameLost.GetInstance();
                break;
            case GameStateType.GameWon:
                ActiveState = GameWon.GetInstance();
                break;
            default:
                throw new ArgumentException("Cannot switch to unknown state");
        }
    }  

    public void ProcessEvent(GameEvent gameEvent) {
        if (gameEvent.EventType == GameEventType.GameStateEvent) {
            if (gameEvent.Message == "CHANGE_STATE") {
                SwitchState(StateTransformer.TransformStringToState(gameEvent.StringArg1));
            } 
        }
    }
}

