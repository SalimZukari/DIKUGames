using System.IO;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Math;
using DIKUArcade.Input;
using DIKUArcade.State;
using DIKUArcade.Events;
using DIKUArcade.Physics;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using Breakout.BreakoutStates;
using System.Collections.Generic;

namespace Breakout.BreakoutStates {
    public class GameRunning : IGameState {
        private static GameRunning? instance = null;
        private Entity backGroundImage;
        private Player player;
        private LevelSetUp blocks;
        private IBaseImage ballsImage;
        private EntityContainer<Ball> balls;
        private string levelFile = "../Assets/Levels/level1.txt";

        public static GameRunning GetInstance() {
            if (GameRunning.instance == null) {
                GameRunning.instance = new GameRunning();
                GameRunning.instance.ResetState();

                }
            return GameRunning.instance;
            }


        public GameRunning() {
            backGroundImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f), 
                new Image(Path.Combine("..", "Assets", "Images", "SpaceBackground.png")));
            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.2f, 0.04f)),
                new Image(Path.Combine("..", "Assets", "Images", "player.png")));
            blocks = new LevelSetUp(levelFile);

            balls = new EntityContainer<Ball>();
            ballsImage = new Image(Path.Combine("..", "Assets", "Images", "ball.png"));
            balls.AddEntity(new Ball(new Vec2F(0.45f, 0.3f), ballsImage));

            BreakoutBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);
        }

        public void HandleKeyEvent (KeyboardAction action, KeyboardKey key) {
            if (action == KeyboardAction.KeyPress) {
                KeyPress(key);
            } else {
                KeyRelease(key);
            }
        }

        public void KeyPress(KeyboardKey key) {
            switch (key) {
                case KeyboardKey.Escape:
                    BreakoutBus.GetBus().RegisterEvent(new GameEvent { 
                        EventType = GameEventType.WindowEvent, 
                        Message = "Quit_Game", 
                    });
                    break;
                case KeyboardKey.Left:
                    BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                        EventType = GameEventType.PlayerEvent,
                        Message = "Move_left",
                    });
                    break;
                case KeyboardKey.Right:
                    BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                        EventType = GameEventType.PlayerEvent,
                        Message = "Move_right",
                    });
                    break;
            }
        }

        public void KeyRelease(KeyboardKey key) {
            switch (key) {
                case KeyboardKey.Left:
                    BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                        EventType = GameEventType.PlayerEvent,
                        Message = "No_move_left",
                    });
                    break;
                case KeyboardKey.Right:
                    BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                        EventType = GameEventType.PlayerEvent,
                        Message = "No_move_right",
                    });
                    break; 
            }
        }

        private void CheckCollisions() {
            balls.Iterate(ball => {
                ball.Movement();
                if (ball.Shape.Position.Y < 0.0f) {
                    ball.DeleteEntity();
                } else {
                    blocks.GetBlocks().Iterate(block => {
                        var collideBlock = CollisionDetection.Aabb((DynamicShape) ball.Shape, block.Shape);
                        var collidePlayer = CollisionDetection.Aabb((DynamicShape) ball.Shape, player.Shape);
                        if (collideBlock.Collision) {
                            block.Damage();
                            ball.ChangeDirection();
                        } else if (collidePlayer.Collision) {
                            ball.ChangeDirection();
                        }
                    
                    });
                }
            });
        }

        public void RenderState() {
            backGroundImage.RenderEntity();
            player.Render();
            blocks.GetBlocks().RenderEntities();
            balls.RenderEntities();
        }

        public void ResetState() {

        }

        public void UpdateState() {
            BreakoutBus.GetBus().ProcessEventsSequentially();
            player.Move();
            CheckCollisions();
        }

        public void NullInstance() {
            instance = null;
        }

    }
}