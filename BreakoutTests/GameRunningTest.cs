using System;
using Breakout.IBlock;
using DIKUArcade.Math;
using DIKUArcade.Input;
using DIKUArcade.State;
using DIKUArcade.Events;
using DIKUArcade.Physics;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using Breakout.BreakoutStates;
using NUnit.Framework;
using Breakout;
using Breakout.BreakoutStates;
using DIKUArcade;

namespace BreakoutTests;

public class GameRunningTests {
    private GameRunning gameRunning;
    private LevelSetUp blocks;
    private IBaseImage ballsImage;
    private EntityContainer<Ball> balls;
    private string levelFile = "../Assets/Levels/level1.txt";

    [OneTimeSetUp]
    public void Init() {
        DIKUArcade.GUI.Window.CreateOpenGLContext();
    }

    [SetUp]
    public void Setup() {
        blocks = new LevelSetUp(levelFile);
        balls = new EntityContainer<Ball>();
        ballsImage = new Image(Path.Combine("..", "Assets", "Images", "ball.png"));
        balls.AddEntity(new Ball(new Vec2F(0.45f, 0.3f), ballsImage));
        gameRunning = new GameRunning();
    }

    [Test]
    public void IsGameWonTest() {
 
    }
}