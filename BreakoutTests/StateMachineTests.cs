using System.IO;
using DIKUArcade;
using DIKUArcade.GUI;
using NUnit.Framework;
using DIKUArcade.Math;
using DIKUArcade.Input;
using DIKUArcade.Events;
using DIKUArcade.Physics;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using Breakout.BreakoutStates;
using System.Collections.Generic;
using Breakout;
using System;


namespace BreakoutTests;
public class StateMachineTest {
    private StateMachine stateMachine;

    [SetUp]
    public void InitiateStateMachine() {
        DIKUArcade.GUI.Window.CreateOpenGLContext();

        try {
            BreakoutBus.GetBus().InitializeEventBus(new List<GameEventType> { 
                GameEventType.PlayerEvent, 
                GameEventType.WindowEvent,
                GameEventType.InputEvent,
                GameEventType.GameStateEvent
            });
        } catch {
            Console.WriteLine("Event bus already initialized");
        } 

        stateMachine = new StateMachine();

        BreakoutBus.GetBus().Subscribe(GameEventType.InputEvent, stateMachine);
        BreakoutBus.GetBus().Subscribe(GameEventType.WindowEvent, stateMachine);
        BreakoutBus.GetBus().Subscribe(GameEventType.GameStateEvent, stateMachine);    
    }

    [Test]
    public void TestInitialState() {
        Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
    }

    [Test]
    public void TestSwitchState() {
        stateMachine.SwitchState(GameStateType.GameRunning);
        Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameRunning>());

        stateMachine.SwitchState(GameStateType.MainMenu);
        Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());

        stateMachine.SwitchState(GameStateType.GamePaused);
        Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());

        stateMachine.SwitchState(GameStateType.GameLost);
        Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameLost>());

        stateMachine.SwitchState(GameStateType.GameWon);
        Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameWon>());
    }

    [Test]
    public void TestProcessEvent() {
        var gameEvent = new GameEvent {
            EventType = GameEventType.GameStateEvent,
            Message = "CHANGE_STATE",
            StringArg1 = "GAME_RUNNING"
        };

        stateMachine.ProcessEvent(gameEvent);
        Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameRunning>());

        var gameEvent2 = new GameEvent {
            EventType = GameEventType.GameStateEvent,
            Message = "CHANGE_STATE",
            StringArg1 = "MAIN_MENU"
        };
        stateMachine.ProcessEvent(gameEvent2);
        Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());

        var gameEvent3 = new GameEvent {
            EventType = GameEventType.GameStateEvent,
            Message = "CHANGE_STATE",
            StringArg1 = "GAME_PAUSED"
        };
        stateMachine.ProcessEvent(gameEvent3);
        Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());

        var gameEvent4 = new GameEvent {
            EventType = GameEventType.GameStateEvent,
            Message = "CHANGE_STATE",
            StringArg1 = "GAME_LOST"
        };
        stateMachine.ProcessEvent(gameEvent4);
        Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameLost>());

        var gameEvent5 = new GameEvent {
            EventType = GameEventType.GameStateEvent,
            Message = "CHANGE_STATE",
            StringArg1 = "GAME_WON"
        };
        stateMachine.ProcessEvent(gameEvent5);
        Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameWon>());
    }
}