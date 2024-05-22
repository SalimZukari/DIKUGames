using System;
using System.Reflection;
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
        private static EntityContainer<Lives> livesImage;
        private Player player;
        private LevelSetUp level;
        private IBaseImage ballsImage;
        private EntityContainer<Ball> balls;
        private BlockObserver blockObserver;
        private bool timeOut = false;
        private IDictionary<string, string> timeData;
        private static int timeInSec;
        private Text timeLeftText;
        private static EntityContainer<Effect> effects;
        private static EntityContainer<Effect> collidedEffects;

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
        public static EntityContainer<Effect> Effects {
            get { return effects; }
        }
        public static EntityContainer<Lives> LivesImage {
            get {return livesImage;}
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
            livesImage = new EntityContainer<Lives>();
            for (int i = 0; i < player.Lives; i++) {
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

            effects = new EntityContainer<Effect>(); 
            effects.ClearContainer();
            collidedEffects = new EntityContainer<Effect>();
        }

        public void SpawnPowerUp(Effect powerUp) {
            effects.AddEntity(powerUp);
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

        private void CheckEffectCollisions() {
            effects.Iterate(effect => {
                effect.Update();
                /*if (CollisionDetection.Aabb((DynamicShape)effect.Shape, player.Shape).Collision) {*/
                if (player.Shape.Position.X < effect.Shape.Position.X 
                    && effect.Shape.Position.X < player.Shape.Position.X + player.Shape.Extent.X
                    && player.Shape.Position.Y < effect.Shape.Position.Y 
                    && effect.Shape.Position.Y < player.Shape.Position.Y + player.Shape.Extent.Y) {
                        ApplyActivate(effect, player, balls);
                        collidedEffects.AddEntity(effect);
                        effect.DeleteEntity();
                } else if (effect.Shape.Position.Y < 0.0f) {
                    effect.DeleteEntity();
                }
            });
        }

        public static void ApplyActivate(Effect effect, Player player, EntityContainer<Ball> balls1) {
            MethodInfo? methodPlayer = effect.GetType().GetMethod("ActivatePlayer");
            MethodInfo? methodBall = effect.GetType().GetMethod("ActivateBall");
            if (methodPlayer != null) {
                methodPlayer.Invoke(effect, new object[] { player });
            }

            balls1.Iterate(ball => {
                if (methodBall != null) {
                    methodBall.Invoke(effect, new object[] { ball });
                }
            });
        }

        public void ApplyEffect(Effect effect, Player player, EntityContainer<Ball> balls1) {
            
        }

        public void EffectTime(Player player, EntityContainer<Ball> balls1) {
            var currentTime = StaticTimer.GetElapsedSeconds();
            foreach (Effect effect in collidedEffects) {
                if (!effect.IsDeactivated && currentTime > effect.ActivationTime + 5) {
                    MethodInfo? methodPlayer = effect.GetType().GetMethod("DeactivatePlayer");
                    MethodInfo? methodBall = effect.GetType().GetMethod("DeactivateBall");

                    if (methodPlayer != null) {
                        methodPlayer.Invoke(effect, new object[] { player });
                    }

                    balls1.Iterate(ball => {
                        if (methodBall != null) {
                            methodBall.Invoke(effect, new object[] { ball });
                        }
                    });

                    effect.IsDeactivated = true; 
                    effect.DeleteEntity();
                }
            }
        }

        public void DetractLife() {
            if (balls.CountEntities() == 0 && player.Lives > 0) {
                livesImage.Iterate(life => {
                    if (life.LifeNumber == player.Lives && life.IsFull) {
                        life.MakeEmtpy();
                        life.IsFull = false;
                    }
                });
                player.Lives--;
                if (player.Lives > 0) {
                    balls.AddEntity(new Ball(new Vec2F(0.45f, 0.3f), ballsImage));
                    foreach (Ball ball in balls) {
                        ball.Direction.Y = -0.01f;
                        ball.Direction.X = 0.0f;
                    }
                }
            }
        }

        public void SetStopWatch() {
            if (timeData.ContainsKey("Time")) {
                Int32.TryParse(timeData["Time"], out int t);
                timeInSec = t;
            }


            if (StaticTimer.GetElapsedSeconds() >= timeInSec) {
                TimeOut = true;
            }
        }

        public static void AddTime() {
            timeInSec += 5;
        }
        
        public bool IsGameOver () {
            if (player.Lives == 0 || TimeOut) {
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
                foreach (Ball ball in balls) {
                        ball.Direction.Y = -0.01f;
                        ball.Direction.X = 0.0f;
                    }
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
            effects.RenderEntities();
        }

        public void ResetState() {
            StaticTimer.RestartTimer();
            balls.Iterate(ball => {
                ball.SetExtent();
            });
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
            CheckEffectCollisions();
            EffectTime(player, balls);
        }

        public void NullInstance() {
            instance = null;
        }
    }
}