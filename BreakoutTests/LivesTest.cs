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
using Breakout.IBlock;


namespace BreakoutTests {
    
    [TestFixture]
    public class LivesTest {
    [OneTimeSetUp]
    public void Init() {
        DIKUArcade.GUI.Window.CreateOpenGLContext();
    }
        
        private Lives lives;
        private Player player;
        private Image fullImage;
        private Image emptyImage;
        private EntityContainer<Ball> balls;
        private EntityContainer<Lives> livesImage;
        private EntityContainer<Lives> emptylives;
        private EntityContainer<Lives> fulllives;
        

        [SetUp]
        public void Setup() {
            DynamicShape shape = new DynamicShape(new Vec2F(0.5f, 0.1f), new Vec2F(0.1f, 0.1f));
            player = new Player(shape, null,3);
            balls = new EntityContainer<Ball>();
            fullImage = new Image(Path.Combine("..","Assets", "Images", "heart_filled.png"));
            emptyImage = new Image(Path.Combine("..","Assets", "Images", "heart_empty.png"));
            lives = new Lives(new Vec2F(0.5f, 0.5f), fullImage, emptyImage, 3);
            livesImage = new EntityContainer<Lives>();
            for (int i = 0; i < 2; i++) {
                livesImage.AddEntity(new Lives(new Vec2F(0.5f, 0.5f), fullImage, emptyImage, i + 1));
            }
        }

        [Test]
        public void LivesTest1() {
            Assert.That(lives.Position.X, Is.EqualTo(0.5f).Within(0.0001f));
            Assert.That(lives.Position.Y, Is.EqualTo(0.5f).Within(0.0001f));
            Assert.That(lives.Extent.X, Is.EqualTo(0.04f).Within(0.0001f));
            Assert.That(lives.Extent.Y, Is.EqualTo(0.04f).Within(0.0001f));
            Assert.AreEqual(fullImage, lives.Image);
            Assert.IsTrue(lives.IsFull);
            Assert.IsFalse(lives.IsEmpty);
            Assert.AreEqual(3, lives.LifeNumber);
        }

        [Test]
        public void MakeEmpty() {
            lives.MakeEmpty();
            Assert.AreEqual(emptyImage, lives.Image);
            Assert.IsFalse(lives.IsFull);
            Assert.IsTrue(lives.IsEmpty);
        }

        [Test]
        public void MakeFull() {
            lives.MakeEmpty();
            lives.MakeFull();
            Assert.AreEqual(fullImage, lives.Image);
            Assert.IsTrue(lives.IsFull);
            Assert.IsFalse(lives.IsEmpty);
        }
        [Test]
        public void DetractLife() { 
            // Arrange
            GameRunning gameRunning = new GameRunning(Path.Combine(
                    "..", "..", "..", "..", "Assets", "Levels", "level1.txt"
                ));
            EntityContainer<Block> blocks = gameRunning.LevelSetUp.GetBlocks();
            EntityContainer<Ball> balls = gameRunning.Ball;
            balls.ClearContainer();

            Player player1 = gameRunning.Player;
            player1.Lives = 2; // Set player's lives to 2 initially

            GameRunning.LivesImage.ClearContainer();
            for (int i = 0; i < player1.Lives; i++) {
                var life = new Lives(new Vec2F(0.01f + i * 0.05f, 0.95f), 
                    new Image(Path.Combine("..", "Assets", "Images", "heart_filled.png")), 
                    new Image(Path.Combine("..", "Assets", "Images", "heart_empty.png")), i + 1);
                GameRunning.LivesImage.AddEntity(life);
            }

            // Act
            gameRunning.DetractLife();
            gameRunning.UpdateState();

            // Assert
            var emptyLives = new EntityContainer<Lives>();
            foreach (Lives life in GameRunning.LivesImage) {
                if (life.IsEmpty) {
                    emptyLives.AddEntity(life);
                }
            }
            Assert.AreEqual(1, emptyLives.CountEntities(), "Expected one empty life entity after detracting a life.");

            var fullLives = new EntityContainer<Lives>();
            foreach (Lives life in GameRunning.LivesImage) {
                if (life.IsFull) {
                    fullLives.AddEntity(life);
                }
            }
            Assert.AreEqual(1, fullLives.CountEntities(), "Expected one full life entity remaining after detracting a life.");
            
            Assert.AreEqual(1, player1.Lives, "Player lives should be decremented by 1.");
        }

        [Test]
        public void DetractLife1() {
            // Arrange
            GameRunning gameRunning = new GameRunning(Path.Combine(
                    "..", "..", "..", "..", "Assets", "Levels", "level1.txt"
                ));

            // Act

            // Assert
            Assert.AreEqual(3, player.Lives);
            emptylives = new EntityContainer<Lives>();
            foreach (Lives life in livesImage) {
                if (life.IsEmpty) {
                    emptylives.AddEntity(life);
                }
            }
            Assert.AreEqual(0, emptylives.CountEntities());
            fulllives = new EntityContainer<Lives>();
            foreach (Lives life in livesImage) {
                if (life.IsFull) {
                    fulllives.AddEntity(life);
                }
            }
            Assert.AreEqual(2, fulllives.CountEntities());
        }
    }
}