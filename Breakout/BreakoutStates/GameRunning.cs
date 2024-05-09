using System;
using Breakout.IBlock;
using DIKUArcade.Math;
using DIKUArcade.Input;
using DIKUArcade.State;
using DIKUArcade.Events;
using DIKUArcade.Physics;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout.BreakoutStates {
    public class GameRunning : IGameState {
        private static GameRunning? instance = null;
        private Entity backGroundImage;
        private Player player;
        private LevelSetUp blocks;
        private IBaseImage ballsImage;
        private EntityContainer<Ball> balls;
        private BlockObserver blockObserver;
        public EntityContainer<Ball> Ball {
            get { return balls; }
        }

        public LevelSetUp LevelSetUp {
            get { return blocks; }
        }

        public Player Player {
            get { return player; }
        }

        public static GameRunning GetInstance() {
            if (GameRunning.instance == null) {
                GameRunning.instance = new GameRunning("../Assets/Levels/level1.txt");
                GameRunning.instance.ResetState();

                }
            return GameRunning.instance;
            }

        public GameRunning(string levelFile) {
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

            blockObserver = new BlockObserver();
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
                    BreakoutBus.GetBus().RegisterEvent ( new GameEvent {
                        EventType = GameEventType.GameStateEvent,
                        Message = "CHANGE_STATE",
                        StringArg1 = "GAME_PAUSED",
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
            float playerLeft = player.Shape.Position.X;
            float playerRight = player.Shape.Position.X + player.Shape.Extent.X;
            float playerMid = (playerRight + playerLeft) / 2.0f;
            balls.Iterate(ball => {
                float ballPos = ball.Shape.Position.X + (ball.Shape.Extent.X / 2.0f);
                ball.Movement();
                ball.CheckDeleteBall();
                blocks.GetBlocks().Iterate(block => {
                    var collideBlock = CollisionDetection.Aabb((
                                                DynamicShape) ball.Shape, block.Shape);
                    var collidePlayer = CollisionDetection.Aabb((
                                                DynamicShape) ball.Shape, player.Shape);
                    if (collideBlock.Collision) {
                        block.Damage();
                        ball.HitsBlockMove();
                    } else if (collidePlayer.Collision) {
                        if (playerLeft <= ballPos && ballPos < playerMid) {
                            ball.GoLeft();
                        } else if (playerMid <= ballPos && ballPos < playerRight) {
                            ball.GoRight();
                        }
                    }
                });
            });
        }
        
        public bool IsGameOver () {
            if (balls.CountEntities() == 0) {
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.GameStateEvent,
                    Message = "CHANGE_STATE",
                    StringArg1 = "GAME_LOST",
                }); 
                MainMenu.GetInstance().ResetState();
                GameRunning.GetInstance().NullInstance();
                return true;
            }
            return false;
        }

        public bool SwitchLevelIfWon() {
            string nextLevelFile = blocks.GetNextLevelFile();

            if (File.Exists(nextLevelFile)) {
                blocks.LoadLevel(nextLevelFile);
                
                player.ResetPosition();
                balls.ClearContainer();
                balls.AddEntity(new Ball(new Vec2F(0.45f, 0.3f), ballsImage));
                return true;
            }
            return false;
        }

        public bool IsGameWon() {
            if (blocks.GetBlocks().CountEntities() == 0) {
                if (!SwitchLevelIfWon()) {
                    BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                        EventType = GameEventType.GameStateEvent,
                        Message = "CHANGE_STATE",
                        StringArg1 = "GAME_WON",
                    });
                    MainMenu.GetInstance().ResetState();
                    GameRunning.GetInstance().NullInstance();
                    return true;
                }
            }
            return false;
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
            blockObserver.CheckBlocks(blocks.GetBlocks());
            IsGameOver();
            IsGameWon();
        }

        public void NullInstance() {
            instance = null;
        }
    }
}