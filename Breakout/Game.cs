using System.IO;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Math;
using DIKUArcade.Input;
using DIKUArcade.State;
using DIKUArcade.Timers;
using DIKUArcade.Events;
using DIKUArcade.Physics;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using Breakout.BreakoutStates;
using System.Collections.Generic;

namespace Breakout;
public class Game : DIKUGame, IGameEventProcessor {
    private StateMachine stateMachine;

    public Game(WindowArgs windowArgs) : base(windowArgs) {
        BreakoutBus.GetBus().InitializeEventBus(new List<GameEventType> { 
            GameEventType.PlayerEvent, 
            GameEventType.WindowEvent,
            GameEventType.InputEvent,
            GameEventType.GameStateEvent
        });
            stateMachine = new StateMachine();
            BreakoutBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            BreakoutBus.GetBus().Subscribe(GameEventType.WindowEvent, this);
            window.SetKeyEventHandler(KeyHandler);
    }

    public void ProcessEvent(GameEvent gameEvent) {
        switch (gameEvent.EventType) {
            case GameEventType.WindowEvent:
                switch (gameEvent.Message) {
                    case "Quit_Game":
                        window.CloseWindow();
                        break;
                }
                break;
        }
    }

    public override void Render() {
        stateMachine.ActiveState.RenderState();
    }

    public override void Update() {
        stateMachine.ActiveState.UpdateState();
        window.PollEvents();
    }

    public void KeyHandler(KeyboardAction action, KeyboardKey key) {
        stateMachine.ActiveState.HandleKeyEvent(action, key);
    }

}