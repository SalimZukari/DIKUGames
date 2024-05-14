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
    }

    [Test]
    public void TestTransformStateToString() {
        var result = StateTransformer.TransformStateToString(GameStateType.GameRunning);
        var expected = "GAME_RUNNING";
        Assert.AreEqual(expected, result);
    }
}