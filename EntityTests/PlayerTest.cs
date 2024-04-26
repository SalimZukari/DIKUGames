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

namespace BreakoutTests {
    public class PlayerTest  {
        private Player player;
        private GameEventBus eventBus;
        
        [SetUp]
        public void Setup() {
            DynamicShape shape = new DynamicShape(new Vec2F(0.5f, 0.1f), new Vec2F(0.1f, 0.1f));
            player = new Player(shape, null);
            eventBus = new GameEventBus();
            eventBus.InitializeEventBus(new List<GameEventType> {GameEventType.PlayerEvent});
            eventBus.Subscribe(GameEventType.PlayerEvent, player);
        }
        
        [Test]
        public void TestPlayerStartPosition()  {
            var position = player.GetPosition();
            var startPos = new Vec2F(0.5f, 0.1f);

            Assert.AreEqual(startPos.X, position.X);
            Assert.AreEqual(startPos.Y, position.Y);
        }

        [Test]
        //Arrange
        public void TestMoveRightSide() {
        var expectedPosition = player.GetPosition().X;
        var tolerance = 0.01f;
        GameEvent gameEventPress = new GameEvent {
            EventType = GameEventType.PlayerEvent, 
            From = this, 
            To = player, 
            Message = "Move_right", 
        };
        //Act
        eventBus.RegisterEvent (gameEventPress);
        player.ProcessEvent(gameEventPress);
        player.Move(); 
        var positionAferMove = player.Shape.Position.X;
        //Assert
        Assert.AreEqual(expectedPosition, positionAferMove, tolerance);
        }
        [Test]
        public void TestMoveLeftSide() {
            // Arrange
            var expectedPosition = player.GetPosition().X;
            var tolerance = 0.01f;
            GameEvent gameEventPress = new GameEvent {
                EventType = GameEventType.PlayerEvent, 
                From = this, 
                To = player, 
                Message = "Move_left", 
            };
            
            // Act
            eventBus.RegisterEvent (gameEventPress);
            player.ProcessEvent(gameEventPress);
            player.Move(); 
            var positionAferMove = player.Shape.Position.X;

            // Assert
            Assert.AreEqual(expectedPosition, positionAferMove, tolerance);
        }

        [Test]
        public void TestPlayerBoundary() {
            // Arrange
            player.Shape.Position = new Vec2F(0.0f, 0.1f);
            var expectedPosition = player.GetPosition().X - 0.01f;
            var tolerance = 0.01f;
            GameEvent gameEventPress = new GameEvent {
                EventType = GameEventType.PlayerEvent, 
                From = this, 
                To = player, 
                Message = "Move_left", 
            };
            
            // Act
            eventBus.RegisterEvent(gameEventPress);
            player.ProcessEvent(gameEventPress);
            player.Move(); 
            var positionAfterMove = player.Shape.Position.X;

            // Assert
            Assert.AreEqual(expectedPosition, positionAfterMove, tolerance);
        }
    }
}
