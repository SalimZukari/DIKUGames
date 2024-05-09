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


namespace BreakoutTests;
public class StateTransformerTest {

    [OneTimeSetUp]
    public void Init() {
        DIKUArcade.GUI.Window.CreateOpenGLContext();
    }

    [SetUp]
    public void Setup() {
    }

    [Test]
    public void TestTransformStringToState() {
        var result = StateTransformer.TransformStringToState("GAME_RUNNING");
        var expected = GameStateType.GameRunning;
        Assert.AreEqual(expected, result);

        var result2 = StateTransformer.TransformStringToState("GAME_PAUSED");
        var expected2 = GameStateType.GamePaused;
        Assert.AreEqual(expected2, result2);

        var result3 = StateTransformer.TransformStringToState("GAME_LOST");
        var expected3 = GameStateType.GameLost;
        Assert.AreEqual(expected3, result3);

        var result4 = StateTransformer.TransformStringToState("MAIN_MENU");
        var expected4 = GameStateType.MainMenu;
        Assert.AreEqual(expected4, result4);
        
        var result5 = StateTransformer.TransformStringToState("GAME_WON");
        var expected5 = GameStateType.GameWon;
        Assert.AreEqual(expected5, result5);

    }

    [Test]
    public void TestTransformStateToString() {
        var result = StateTransformer.TransformStateToString(GameStateType.GameRunning);
        var expected = "GAME_RUNNING";
        Assert.AreEqual(expected, result);

        var result2 = StateTransformer.TransformStateToString(GameStateType.GamePaused);
        var expected2 = "GAME_PAUSED";
        Assert.AreEqual(expected2, result2);

        var result3 = StateTransformer.TransformStateToString(GameStateType.GameLost);
        var expected3 = "GAME_LOST";
        Assert.AreEqual(expected3, result3);

        var result4 = StateTransformer.TransformStateToString(GameStateType.MainMenu);
        var expected4 = "MAIN_MENU";
        Assert.AreEqual(expected4, result4);

        var result5 = StateTransformer.TransformStateToString(GameStateType.GameWon);
        var expected5 = "GAME_WON";
        Assert.AreEqual(expected5, result5);
    }
}