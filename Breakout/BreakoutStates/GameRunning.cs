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
using DIKUArcade.Utilities;
using Breakout.PowerUps;
using Breakout;

namespace Breakout.BreakoutStates {
    public class GameRunning : IGameState {
        private static GameRunning? instance = null;
        private Entity backGroundImage;
        private EntityContainer<Lives> livesImage;
        private Player player;
        private LevelSetUp level;
        private IBaseImage ballsImage;
        private EntityContainer<Ball> balls;
        private BlockObserver blockObserver;
        private int lives;
        private int livesLost;
        private bool timeOut = false;
        private IDictionary<string, string> timeData;
        private int timeInSec;
        private Text timeLeftText;
        private static EntityContainer<PowerUp> powerUps;

        public bool TimeOut {
            get {return timeOut;}
            private set {timeOut = value;}
        }
        public EntityContainer<Ball> Ball {
            get { return balls; }
        }
        public Text TimeLeftText {
            get {return timeLeftText;}
            private set {timeLeftText = value;}
        }
        public LevelSetUp LevelSetUp {
            get { return level; }
        }
        public Player Player {
            get { return player; }
        }
        public static EntityContainer<PowerUp> PowerUps {
            get { return powerUps; }
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
                new Image(Path.Combine("..", "Assets", "Images", "player.png")), 3);
            level = new LevelSetUp(levelFile);

            balls = new EntityContainer<Ball>();
            ballsImage = new Image(Path.Combine("..", "Assets", "Images", "ball.png"));
            balls.AddEntity(new Ball(new Vec2F(0.45f, 0.3f), ballsImage));

            BreakoutBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);

            blockObserver = new BlockObserver();
            lives = player.Lives;
            livesImage = new EntityContainer<Lives>();
            for (int i = 0; i < lives; i++) {
                var Life = new Lives(new Vec2F(0.01f + i * 0.05f, 0.95f), 
                    new Image(Path.Combine("..", "Assets", "Images", "heart_filled.png")), 
                    new Image(Path.Combine("..", "Assets", "Images", "heart_empty.png")), i + 1);
                livesImage.AddEntity(Life);
            }

            timeData = level.Layout.GetMetaOrganized();
            timeInSec = -1;
            if (timeData.ContainsKey("Time")) {
                Int32.TryParse(timeData["Time"], out int t);
                timeInSec = t;
            }          

            TimeLeftText = new Text("", new Vec2F(0.8f, 0.65f), new Vec2F(0.35f, 0.35f));
            TimeLeftText.SetColor(System.Drawing.Color.White);

            powerUps = new EntityContainer<PowerUp>(); 
            powerUps.ClearContainer();
        }

        public void SpawnPowerUp(PowerUp powerUp) {
            powerUps.AddEntity(powerUp);
        }

        private void CheckPowerUpCollisions() {
            powerUps.Iterate(powerUp => {
                powerUp.Update();
                if (CollisionDetection.Aabb((DynamicShape)powerUp.Shape, player.Shape).Collision) {
                    powerUp.Activate(player);
                    powerUp.DeleteEntity();
                } else if (powerUp.Shape.Position.Y < 0.0f) {
                    powerUp.DeleteEntity();
                }
            });
        }

        public void TimeRender() {
            int timeLeft = (int)(timeInSec - StaticTimer.GetElapsedSeconds());
            string timeLeftString = timeLeft.ToString();
            TimeLeftText.SetText(timeLeftString);
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
                    var collideBlock = CollisionDetection.Aabb((DynamicShape)ball.Shape, block.Shape);
                    var collidePlayer = CollisionDetection.Aabb((DynamicShape)ball.Shape, player.Shape);
                    if (collideBlock.Collision) {
                        if (block is PowerUpBlock powerUpBlock) {
                            powerUpBlock.Damage();
                        } else {
                            block.Damage();
                        }
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
            if (balls.CountEntities() == 0 && lives > 0) {
                livesImage.Iterate(life => {
                    if (life.LifeNumber == lives && life.IsFull) {
                        life.ChangeImage();
                        life.IsFull = false;
                    }
                });
                lives--;
                if (lives > 0) {
                    balls.AddEntity(new Ball(new Vec2F(0.45f, 0.3f), ballsImage));
                }
            }
        }

        public void SetStopWatch() {
            int timeInSec = -1;
            if (timeData.ContainsKey("Time")) {
                Int32.TryParse(timeData["Time"], out int t);
                timeInSec = t;
            }


            if (StaticTimer.GetElapsedSeconds() >= timeInSec) {
                TimeOut = true;
            }
        }
        
        public bool IsGameOver () {
            if (lives == 0 || TimeOut) {
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
            TimeLeftText.RenderText();
            livesImage.RenderEntities();
            powerUps.RenderEntities();
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
            TimeRender();
            CheckPowerUpCollisions();
        }

        public void NullInstance() {
            instance = null;
        }
    }
}