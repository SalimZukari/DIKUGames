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
using Breakout.PowerUps;
using System;
using System.Diagnostics;
using DIKUArcade.Timers;



namespace BreakoutTests;
public class CheckCollisionTest {
    DynamicShape baseShape;
    private static EntityContainer<Effect>? effects;
    private static EntityContainer<Effect>? collidedEffects;
    private Player player;
    private GameRunning gameRunning;
    private Stopwatch stopwatch;

    [OneTimeSetUp]
    public void Init() {
        DIKUArcade.GUI.Window.CreateOpenGLContext();
    }

    [SetUp]
    public void Setup() {
        baseShape = new DynamicShape(0.0f, 0.0f, 0.2f, 0.0f);
        gameRunning = new GameRunning(Path.Combine(
            "..", "..", "..", "..", "Assets", "Levels", "level1.txt"
        ));
        player = gameRunning.Player;
        effects = GameRunning.Effects;
        collidedEffects = new EntityContainer<Effect>();
        stopwatch = new Stopwatch();
    }

    [Test]
    public void TestBlockDestroyed() {
        GameRunning gameRunning = new GameRunning(Path.Combine(
                    "..", "..", "..", "..", "Assets", "Levels", "level1.txt"
                ));
        player = new Player(baseShape, null, 3);
        EntityContainer<Block> block = gameRunning.LevelSetUp.GetBlocks();
        EntityContainer<Ball> ball = gameRunning.Ball;
        ball.ClearContainer();
        Ball newBall = new Ball(new Vec2F(0.47f, 0.2f),
                                new Image("../Assets/Images/ball.png"));
        newBall.SetDirectionY(0.01f);
        ball.AddEntity(newBall);
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
        gameRunning.DetractLife();
        gameRunning.Ball.ClearContainer();
        gameRunning.DetractLife();
        gameRunning.Ball.ClearContainer();
        gameRunning.DetractLife();

        Assert.IsTrue(gameRunning.IsGameOver());
    }

    [Test]
    public void TestIsGameWon() {
        var gameRunning = new GameRunning(Path.Combine("..", "..", "..", "..", "Assets", "Levels", "level1.txt"));
        Assert.IsTrue(gameRunning.LevelSetUp.GetBlocks().CountEntities() > 0);

        gameRunning.LevelSetUp.GetBlocks().ClearContainer();
        
        Assert.IsTrue(gameRunning.IsGameWon());
    }
    [Test]
    public void SetStopWatch_TimeNotExpired() {
        // Arrange
        var gameRunning = new GameRunning(Path.Combine("..", "..", "..", "..", "Assets", "Levels", "level1.txt"));

        // Act
        gameRunning.SetStopWatch();

        // Assert
        Assert.IsFalse(gameRunning.TimeOut);
    }
    [Test]
    public void SetStopWatch_TimeExpired() {
        // Arrange
        var gameRunning = new GameRunning(Path.Combine("..", "..", "..", "..", "Assets", "Levels", "levelTest.txt"));

        // Act
        gameRunning.SetStopWatch();
        gameRunning.SubtractTime();
        gameRunning.SubtractTime();
        gameRunning.SubtractTime();
        gameRunning.UpdateState();
        // Assert
        Assert.IsTrue(gameRunning.TimeOut);
    }
    
    [Test]
    public void TestEffectCollidesWithPlayer() {
        var effect = new DoubleSize(baseShape, new Image(Path.Combine("..", "Assets", "Images", "playerStride.png")));
        gameRunning.SpawnPowerUp(effect);

        // Position the effect to collide with the player
        effect.Shape.Position = new Vec2F(player.Shape.Position.X + 0.1f, player.Shape.Position.Y + 0.1f);
        for (int i = 0; i < 50; i++) {
            effect.Shape.MoveY(-0.01f);
            gameRunning.CheckEffectCollisions();
        }

        Assert.IsTrue(effect.IsDeleted(), "Effect should be deleted after collision with player.");
    }

    [Test]
    public void TestEffectFallsOffScreen() {
        var effect = new DoubleSize(baseShape, new Image(Path.Combine("..", "Assets", "Images", "playerStride.png")));
        gameRunning.SpawnPowerUp(effect);

        effect.Shape.Position = new Vec2F(player.Shape.Position.X, -0.05f);
        gameRunning.CheckEffectCollisions();

        Assert.IsTrue(effect.IsDeleted(), "Effect should be deleted after falling off screen.");
    }

    [Test]
    public void TestEffectNoCollision() {
        var effect = new DoubleSize(baseShape, new Image(Path.Combine("..", "Assets", "Images", "playerStride.png")));
        gameRunning.SpawnPowerUp(effect);

        effect.Shape.Position = new Vec2F(player.Shape.Position.X + 0.5f, player.Shape.Position.Y + 0.5f);
        gameRunning.CheckEffectCollisions();

        Assert.IsFalse(effect.IsDeleted(), "Effect should not be deleted if no collision occurs.");
        Assert.AreEqual(1, GameRunning.Effects.CountEntities(), "Effect should remain in the effects container.");
    }

    [Test]
    public void TestEffectOutOfBounds() {
        var effect = new DoubleSize(baseShape, new Image(Path.Combine("..", "Assets", "Images", "playerStride.png")));
        gameRunning.SpawnPowerUp(effect);

        effect.Shape.Position = new Vec2F(-0.1f, -0.1f);
        gameRunning.CheckEffectCollisions();

        Assert.IsTrue(effect.IsDeleted(), "Effect should be deleted if out of bounds.");
    }

    [Test]
    public void EffectTime_CollidedEffectsIsNull() {
        // Arrange
        collidedEffects = null;
        // Act
        gameRunning.EffectTime(player, gameRunning.Ball);
        // Assert
        // No exception should be thrown
    }

    [Test]
    public void EffectTime_CollidedEffectsIsEmpty() {
        // Arrange
        collidedEffects = new EntityContainer<Effect>();
        // Act
        gameRunning.EffectTime(player, gameRunning.Ball);
        // Assert
        // No exception should be thrown
    }

    [Test]
    public void EffectNotDeactivated_CurrentTimeLessThanActivationTimePlusFive() {
        // Arrange
        var effect = new DoubleSize(baseShape, new Image(Path.Combine("..", "Assets", "Images", "playerStride.png")));
        effect.ActivationTime = StaticTimer.GetElapsedSeconds();
        collidedEffects.AddEntity(effect);
        // Act
        gameRunning.EffectTime(player, gameRunning.Ball);
        // Assert
        Assert.IsFalse(effect.IsDeactivated);
        Assert.IsFalse(effect.IsDeleted());
    }

    [Test]
    public void EffectTime_LastActivationTimesDictionaryEmpty() {
        // Arrange
        gameRunning.lastActivationTimes.Clear();
        var effect = new DoubleSize(baseShape, new Image(Path.Combine("..", "Assets", "Images", "playerStride.png")));
        effect.ActivationTime = StaticTimer.GetElapsedSeconds(); // Set activation time to current time
        collidedEffects.AddEntity(effect);
        
        // Act
        gameRunning.EffectTime(player, gameRunning.Ball);
        
        // Assert
        // No assertion needed since the test should not throw any exceptions
    }

    [Test]
    public void EffectTypeNotInLastActivationTimes() {
        // Arrange
        gameRunning.lastActivationTimes.Clear();
        var effect = new DoubleSize(baseShape, new Image(Path.Combine("..", "Assets", "Images", "playerStride.png")));
        effect.ActivationTime = StaticTimer.GetElapsedSeconds() - 10; 
        collidedEffects.AddEntity(effect);
        gameRunning.lastActivationTimes.Add(typeof(DoubleSize), StaticTimer.GetElapsedSeconds() - 20); 
        
        // Act
        gameRunning.EffectTime(player, gameRunning.Ball);
        
        // Assert
        // No assertion needed since the test should not throw any exceptions
    }

    [Test]
    public void EffectTypeInLastActivationTimes_CurrentTimeGreaterThanActivationTime() {
        // Arrange
        var effect = new DoubleSize(baseShape, new Image(Path.Combine("..", "Assets", "Images", "playerStride.png")));
        effect.ActivationTime = StaticTimer.GetElapsedSeconds() - 10; 
        collidedEffects.AddEntity(effect);
        gameRunning.lastActivationTimes.Add(typeof(DoubleSize), StaticTimer.GetElapsedSeconds() - 20); 
        
        // Act
        gameRunning.EffectTime(player, gameRunning.Ball);
        
        // Assert
        Assert.IsFalse(effect.IsDeactivated, "Effect should not be deactivated if current time is not greater than activation time.");
        Assert.IsFalse(effect.IsDeleted(), "Effect should not be deleted if not deactivated.");
    }

    [Test]
    public void EffectTypeInLastActivationTimes_CurrentTimeLessThanActivationTime() {
        // Arrange
        var effect = new DoubleSize(baseShape, new Image(Path.Combine("..", "Assets", "Images", "playerStride.png")));
        effect.ActivationTime = StaticTimer.GetElapsedSeconds() - 10; 
        collidedEffects.AddEntity(effect);
        gameRunning.lastActivationTimes.Add(typeof(DoubleSize), StaticTimer.GetElapsedSeconds() - 5); 
        stopwatch.Start();

        // Act
        gameRunning.EffectTime(player, gameRunning.Ball);
        stopwatch.Stop();

        // Assert
        Assert.IsFalse(effect.IsDeactivated, "Effect should not be deactivated if current time is less than activation time.");
        Assert.IsFalse(effect.IsDeleted(), "Effect should not be deleted if not deactivated.");
    }

    [Test]
    public void EffectTypeInLastActivationTimes_CurrentTimeGreaterThanActivationTimePlusFive() {
        // Arrange
        stopwatch.Start();
        var effect = new DoubleSize(baseShape, new Image(Path.Combine("..", "Assets", "Images", "playerStride.png")));
        //effect.ActivationTime = StaticTimer.GetElapsedSeconds() - 10; 
        GameRunning.CollidedEffects.AddEntity(effect);
        gameRunning.lastActivationTimes[typeof(DoubleSize)] =  effect.ActivationTime; 

        // Act
        while (stopwatch.ElapsedMilliseconds/1000 <= effect.ActivationTime + 6) {
            gameRunning.EffectTime(player, gameRunning.Ball);
        }
        stopwatch.Stop();

        // Assert
        Assert.IsTrue(effect.IsDeactivated, "Effect should be deactivated if current time is greater than activation time plus five.");
        Assert.IsTrue(effect.IsDeleted(), "Effect should be deleted if deactivated.");
    }
}