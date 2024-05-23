using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using Breakout;
using Breakout.BreakoutStates;
using Breakout.PowerUps;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System;

namespace BreakoutTests {
    public class PowerUpTests {
        string testFilePath = Path.Combine(
            "..", "..", "..", "..", "Assets", "Levels", "level3.txt"
        );
        Player player;    
        Ball ball;
        DynamicShape baseShape;

        [SetUp]
        public void SetUp() {
            DIKUArcade.GUI.Window.CreateOpenGLContext();
            player = new Player(baseShape, null, 3);
            ball = new Ball(new Vec2F(0.45f, 0.3f), new NoImage());
            baseShape = new DynamicShape(0.0f, 0.0f, 0.2f, 0.0f);
        }

        [Test]
        public void TestDoubleSize() {
            DoubleSize doubleSize = new DoubleSize(baseShape, new NoImage());
            doubleSize.ActivateBall(ball);
            float postExtent = 0.07f;
            Assert.AreEqual(postExtent, ball.Shape.Extent.X);
            Assert.AreEqual(postExtent, ball.Shape.Extent.Y);

            doubleSize.DeactivateBall(ball);
            float preExtent = 0.035f;
            Assert.AreEqual(preExtent, ball.Shape.Extent.X);
            Assert.AreEqual(preExtent, ball.Shape.Extent.Y);
        }

        [Test]
        public void TestDoubleSizeLimit() {
            DoubleSize doubleSize = new DoubleSize(baseShape, new NoImage());

            float postExtent = 0.07f;
            doubleSize.ActivateBall(ball);
            doubleSize.ActivateBall(ball);
            Assert.AreEqual(postExtent, ball.Shape.Extent.X);
            Assert.AreEqual(postExtent, ball.Shape.Extent.Y);

            float preExtent = 0.035f;
            doubleSize.DeactivateBall(ball);
            doubleSize.DeactivateBall(ball);
            Assert.AreEqual(preExtent, ball.Shape.Extent.X);
            Assert.AreEqual(preExtent, ball.Shape.Extent.Y);
        }

        [Test]
        public void TestLoseLife() {
            LoseLife loseLife = new LoseLife(baseShape, new NoImage());
            loseLife.ActivatePlayer(player);
            Assert.AreEqual(player.Lives, 2);
        }

        [Test]
        public void TestExtraLife() {
            ExtraLife extraLife = new ExtraLife(baseShape, new NoImage());

            extraLife.ActivatePlayer(player);
            Assert.AreEqual(player.Lives, 3);

            extraLife.ActivatePlayer(player);
            Assert.AreEqual(player.Lives, 3);
        }

        [Test]
        public void TestPlayerSpeed() {
            PlayerSpeed playerSpeed = new PlayerSpeed(baseShape, new NoImage());

            playerSpeed.ActivatePlayer(player);
            Assert.AreEqual(player.MovementSpeed, 0.02f);

            playerSpeed.ActivatePlayer(player);
            Assert.AreEqual(player.MovementSpeed, 0.02f);

            playerSpeed.DeactivatePlayer(player);
            Assert.AreEqual(player.MovementSpeed, 0.01f);

            playerSpeed.DeactivatePlayer(player);
            Assert.AreEqual(player.MovementSpeed, 0.01f);
        }

        [Test]
        public void TestSlowness() {
            Slowness slowness = new Slowness(baseShape, new NoImage());

            slowness.ActivatePlayer(player);
            Assert.AreEqual(player.MovementSpeed, 0.005f);

            slowness.ActivatePlayer(player);
            Assert.AreEqual(player.MovementSpeed, 0.005f);

            slowness.DeactivatePlayer(player);
            Assert.AreEqual(player.MovementSpeed, 0.01f);

            slowness.DeactivatePlayer(player);
            Assert.AreEqual(player.MovementSpeed, 0.01f);
        }

        [Test]
        public void TestWide() {
            Wide wide = new Wide(baseShape, new NoImage());

            wide.ActivatePlayer(player);
            Assert.AreEqual(player.Shape.Extent.X, 0.4f, 0.0005f);

            wide.ActivatePlayer(player);
            Assert.AreEqual(player.Shape.Extent.X, 0.4f, 0.0005f);

            wide.DeactivatePlayer(player);
            Assert.AreEqual(player.Shape.Extent.X, 0.2f, 0.0005f);

            wide.DeactivatePlayer(player);
            Assert.AreEqual(player.Shape.Extent.X, 0.2f, 0.0005f);
        }

        [Test]
        public void TestMoreTime() {
            MoreTime moreTime = new MoreTime(baseShape, new NoImage());
            GameRunning gameRunning = new GameRunning(testFilePath);

            gameRunning.TimeRender();

            Assert.AreEqual(GameRunning.TimeInSec, 180);

            moreTime.ActivateBall(ball);
            Assert.AreEqual(GameRunning.TimeInSec, 185);
        }
    }
}