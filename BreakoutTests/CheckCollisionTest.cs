using System.IO;
using DIKUArcade;
using DIKUArcade.GUI;
using NUnit.Framework;
using DIKUArcade.Math;
using DIKUArcade.Input;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using Breakout.BreakoutStates;
using Breakout.IBlock;
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
        /* GameRunning gameRunning = new GameRunning();
        Block block = gameRunning.GetBlocks()[0];
        Ball ball = gameRunning.GetBall()[0];

        float blockPosition = block.GetPosition();
        float ballPosition = ball.GetPosition();
        Vec2F ballDir = (blockPosition.X - ballPosition.X,
                        blockPosition.Y - ballPosition.Y);
        int lengthOfDirVec = Math.sqrt(
                ((double)ballDir.X ** 2.0) +
                ((double)ballDir.Y ** 2.0)
            );

        ball.SetDirection(ballDir);

        for (int i = 0; i <= lengthOfDirVec; i++) {
            ball.Movement();
            gameRunning.CheckCollisions();
        }

        Assert.AreEqual(ball, null); */
    }
}