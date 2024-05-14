using System;
using Breakout.IBlock;
using DIKUArcade.Math;
using DIKUArcade.Input;
using DIKUArcade.State;
using DIKUArcade.Events;
using DIKUArcade.Physics;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Timers;

namespace Breakout.BreakoutStates {
    public class GameRunning : IGameState {
        private static GameRunning? instance = null;
        private Entity backGroundImage;
        private Entity livesImage;
        private Player player;
        private LevelSetUp level;
        private IBaseImage ballsImage;
        private EntityContainer<Ball> balls;
        private BlockObserver blockObserver;
        private int lives;
        private int livesLost;
        private bool timeOut = false;

        public bool TimeOut {
            get {return timeOut;}
            private set {timeOut = value;}
        }
        public EntityContainer<Ball> Ball {
            get { return balls; }
        }

        public LevelSetUp LevelSetUp {
            get { return level; }
        }
        public int Lives {
            get { return lives; }
            private set {lives = value;}
        }

        public Player Player {
            get { return player; }
        }

        public static GameRunning GetInstance() {
            if (GameRunning.instance == null) {
                GameRunning.instance = new GameRunning("../Assets/Levels/level3.txt");
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
            level = new LevelSetUp(levelFile);

            balls = new EntityContainer<Ball>();
            ballsImage = new Image(Path.Combine("..", "Assets", "Images", "ball.png"));
            balls.AddEntity(new Ball(new Vec2F(0.45f, 0.3f), ballsImage));

            BreakoutBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);

            blockObserver = new BlockObserver();
            lives = 3;            
        }

        public void LivesImageDisplay() {
            for (int i = 0; i < Lives; i++) {
                livesImage = new Entity(new StationaryShape(0.01f + i * 0.05f, 0.95f, 0.04f, 0.04f),
                    new Image(Path.Combine("..", "Assets", "Images", "heart_filled.png")));
                livesImage.RenderEntity();
            } 
        }

        public void LivesLostDisplay() {
            livesLost = 3 - Lives;
            for (int i = 0; i < livesLost; i++) {
                livesImage = new Entity(new StationaryShape(0.01f + i * 0.05f, 0.95f, 0.04f, 0.04f),
                    new Image(Path.Combine("..", "Assets", "Images", "heart_empty.png")));
                livesImage.RenderEntity();
            } 
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
                    StaticTimer.PauseTimer();
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
                level.GetBlocks().Iterate(block => {
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

        public void DetractLife() {
            if (balls.CountEntities() == 0 && Lives > 1) {
                Lives--;
                balls.AddEntity(new Ball(new Vec2F(0.45f, 0.3f), ballsImage));

            } else if (balls.CountEntities() == 0 && Lives == 1) {
                Lives--;
            }
        }

        public void SetStopWatch() {
            var timeData = level.Layout.GetMetaOrganized();
            int timeInSec = -1;
            if (timeData.ContainsKey("Time")) {
                Int32.TryParse(timeData["Time"], out int t);
                timeInSec = t;
            } if (StaticTimer.GetElapsedSeconds() >= timeInSec) {
                TimeOut = true;
            }
        }
        
        public bool IsGameOver () {
            if (Lives == 0 || TimeOut) {
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
            string nextLevelFile = level.GetNextLevelFile();

            if (File.Exists(nextLevelFile)) {
                level.LoadLevel(nextLevelFile);
                
                player.ResetPosition();
                balls.ClearContainer();
                balls.AddEntity(new Ball(new Vec2F(0.45f, 0.3f), ballsImage));
                return true;
            }
            return false;
        }

        public bool IsGameWon() {
            if (level.GetBlocks().CountEntities() == 0) {
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
            level.GetBlocks().RenderEntities();
            balls.RenderEntities();
            LivesImageDisplay();
            LivesLostDisplay();
        }

        public void ResetState() {
            StaticTimer.RestartTimer();
        }

        public void UpdateState() {
            BreakoutBus.GetBus().ProcessEventsSequentially();
            player.Move();
            CheckCollisions();
            blockObserver.CheckBlocks(level.GetBlocks());
            IsGameOver();
            IsGameWon();
            DetractLife();
            SetStopWatch();
        }

        public void NullInstance() {
            instance = null;
        }
    }
}