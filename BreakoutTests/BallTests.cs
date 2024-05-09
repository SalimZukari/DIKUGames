using NUnit.Framework;
using DIKUArcade.Math;
using Breakout;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade;
using DIKUArcade.Events;
using DIKUArcade.Input;
using System.Collections.Generic;
using DIKUArcade.GUI;
using DIKUArcade.Physics;
using Breakout.BreakoutStates;
using System;

namespace BreakoutTests; 
public class BallTest {
    private Random rnd = new Random();
    private Vec2F position;
    private Ball ball;
    private EntityContainer<Ball> BallContainer;


    private static Vec2F extent = new Vec2F(0.035f, 0.035f);
    public static Vec2F direction = new Vec2F(0.0f, 0.01f);
    
    [OneTimeSetUp]
    public void Init() {
        DIKUArcade.GUI.Window.CreateOpenGLContext();
    }

    [SetUp]
    public void Setup() {
        position = new Vec2F(0.5f, 0.5f);
        ball = new Ball(position, new Image(Path.Combine("..","Assets", "Images", "ball.png")));
        BallContainer = new EntityContainer<Ball>();
        BallContainer.AddEntity(ball);
    }

    [Test]
    public void TestEnsureOppositeXDir() {
        ball.Direction.X = 0.05f; // positive
        ball.EnsureOppositeXDir();
        Assert.AreNotEqual(0.05f, ball.Direction.X, "Direction X should invert on hit");

        ball.Direction.X = -0.05f; // negative
        ball.EnsureOppositeXDir();
        Assert.AreNotEqual(-0.05f, ball.Direction.X, "Direction X should invert on hit");
    }

    [Test]
    public void TestHitsBlockMove() {
        ball.Direction.Y = 0.01f; // initially positive
        ball.HitsBlockMove();
        Assert.AreEqual(-0.01f, ball.Direction.Y, "Direction Y should be negative when initially positive");

        ball.Direction.Y = -0.01f; // now negative
        ball.HitsBlockMove();
        Assert.AreEqual(0.01f, ball.Direction.Y, "Direction Y should be positive when initially negative");
    }


    [Test]
    public void TestMovement() {
        ball.Shape.Position.Y = 2.0f; // First condition 
        ball.Movement();
        Assert.AreEqual(-0.01f, ball.Direction.Y, "Direction Y should be negative when near upper boundary");

        ball.Shape.Position.X = 3.0f; // Second condition
        ball.Movement();
        Assert.Less(ball.Direction.X, 0.0f, "Direction X should be negative when near right boundary");

        ball.Shape.Position.X = -0.3f; // Third condition
        ball.Movement();
        Assert.Greater(ball.Direction.X, -1, "Direction X should be positive when near left boundary");
    }
}