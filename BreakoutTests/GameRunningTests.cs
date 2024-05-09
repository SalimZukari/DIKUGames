using System.IO;
using DIKUArcade;
using DIKUArcade.GUI;
using NUnit.Framework;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using Breakout.BreakoutStates;
using Breakout.IBlock;
using Breakout;
using System.Collections.Generic;
using System;


namespace BreakoutTests;
public class CheckCollisionTest {

    [OneTimeSetUp]
    public void Init() {
        DIKUArcade.GUI.Window.CreateOpenGLContext();
    }

    [SetUp]
    public void Setup() {
    }

    [Test]
    public void TestBlockDestroyed() {
        GameRunning gameRunning = new GameRunning(Path.Combine(
                    "..", "..", "..", "..", "Assets", "Levels", "level1.txt"
                ));
        EntityContainer<Block> block = gameRunning.LevelSetUp.GetBlocks();
        EntityContainer<Ball> ball = gameRunning.Ball;
        int beforeCheckBlocks = block.CountEntities();

        for (int i = 0; i < 100; i++) {
            gameRunning.UpdateState();
        }

        int afterCheckBlocks = block.CountEntities();
        Assert.Less(afterCheckBlocks, beforeCheckBlocks);
    }

    [Test]
    public void TestPlayerHitLeft() {
        GameRunning gameRunning = new GameRunning(Path.Combine(
                    "..", "..", "..", "..", "Assets", "Levels", "level1.txt"
                ));
        EntityContainer<Block> block = gameRunning.LevelSetUp.GetBlocks();
        EntityContainer<Ball> ball = gameRunning.Ball;
        ball.ClearContainer();
        Ball newBall = new Ball(new Vec2F(0.47f, 0.2f),
                                new Image("../Assets/Images/ball.png"));
        newBall.SetDirectionY(-0.01f);
        ball.AddEntity(newBall);

        for (int i = 0; i < 20; i++) {
            gameRunning.UpdateState();
        }
        Assert.Less(newBall.Direction.X, 0);
    }

    [Test]
    public void TestPlayerHitRight() {
        GameRunning gameRunning = new GameRunning(Path.Combine(
                    "..", "..", "..", "..", "Assets", "Levels", "level1.txt"
                ));
        EntityContainer<Block> block = gameRunning.LevelSetUp.GetBlocks();
        EntityContainer<Ball> ball = gameRunning.Ball;
        ball.ClearContainer();
        Ball newBall = new Ball(new Vec2F(0.57f, 0.2f),
                                new Image("../Assets/Images/ball.png"));
        newBall.SetDirectionY(-0.01f);
        ball.AddEntity(newBall);

        for (int i = 0; i < 20; i++) {
            gameRunning.UpdateState();
        }
        Assert.Less(0, newBall.Direction.X);
    }

    [Test]
    public void TestIsGameOver() {
        var gameRunning = new GameRunning(Path.Combine("..", "..", "..", "..", "Assets", "Levels", "level1.txt"));
    
        Assert.IsTrue(gameRunning.Ball.CountEntities() > 0);

        gameRunning.Ball.ClearContainer();

        Assert.IsTrue(gameRunning.IsGameOver());
    }

    [Test]
    public void TestIsGameWon() {
        var gameRunning = new GameRunning(Path.Combine("..", "..", "..", "..", "Assets", "Levels", "level1.txt"));
        Assert.IsTrue(gameRunning.LevelSetUp.GetBlocks().CountEntities() > 0);

        gameRunning.LevelSetUp.GetBlocks().ClearContainer();
        
        Assert.IsTrue(gameRunning.IsGameWon());
    }
}