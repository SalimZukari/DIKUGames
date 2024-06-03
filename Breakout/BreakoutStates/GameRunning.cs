using System;
using Breakout;
using Breakout.IBlock;
using DIKUArcade.Math;
using DIKUArcade.Input;
using DIKUArcade.State;
using System.Reflection;
using DIKUArcade.Events;
using DIKUArcade.Timers;
using Breakout.PowerUps;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;

namespace Breakout.BreakoutStates {
    public class GameRunning : IGameState {
        /// <summary>
        /// The main chunk of the game
        /// This is where the game mechanics are managed while the game runs
        /// </summary>
        private static GameRunning? instance = null;
        private Entity backGroundImage;
        private Entity overLayImage;
        private static EntityContainer<Lives>? livesImage;
        private Player player;
        private LevelSetUp level;
        private IBaseImage ballsImage;
        private EntityContainer<Ball> balls;
        private BlockObserver blockObserver;
        private bool timeOut = false;
        private IDictionary<string, string> timeData;
        private static int timeInSec;
        private Text? timeLeftText;
        private static EntityContainer<Effect>? effects;
        private static EntityContainer<Effect>? collidedEffects;
        private int timeLeft;
        private string? timeLeftString;
        private Dictionary<Type, double> LastActivationTimes = new Dictionary<Type, double>();
        private static int score;
        private static int prevScore;
        private Text scoreText;
        private CollisionHandler collisionHandler;

        /// <summary>
        /// Property that says if time has run out
        /// </summary>
        public bool TimeOut {
            get {return timeOut;}
            private set {timeOut = value;}
        }
        public EntityContainer<Ball> Ball {
            get { return balls; }
        }
        public Text? TimeLeftText {
            get {return timeLeftText;}
            private set {timeLeftText = value;}
        }
        /// <summary>
        /// The information for the level set up
        /// </summary>
        public LevelSetUp LevelSetUp {
            get { return level; }
        }
        public Player Player {
            get { return player; }
        }
        public static EntityContainer<Effect>? Effects {
            get { return effects; }
        }
        /// <summary>
        /// Lives as entities to be rendered
        /// </summary>
        public static EntityContainer<Lives>? LivesImage {
            get {return livesImage;}
        }
        public static int TimeInSec {
            get {return timeInSec;}
        }
        /// <summary>
        /// Gives data on effects and activation times
        /// </summary>
        public Dictionary<Type, double> lastActivationTimes {
            get { return LastActivationTimes; }
        }   
        public static EntityContainer<Effect>? CollidedEffects {
            get { return collidedEffects; }
        }
        public static int Score {
            get {return score;}
        }


        public static GameRunning GetInstance() {
            if (GameRunning.instance == null) {
                GameRunning.instance = new GameRunning("../Assets/Levels/level1.txt");
                GameRunning.instance.ResetState();

                }
            return GameRunning.instance;
            }

        /// <summary>
        /// Constructor is in charge of a lot.
        /// Many properties are defined in the constructor.
        /// </summary>
        public GameRunning(string levelFile) {
            backGroundImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f), 
                new Image(Path.Combine("..", "Assets", "Images", "SpaceBackground.png")));
            overLayImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f), 
                new Image(Path.Combine("..","Assets", "Images", "Overlay.png")));
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
                var Life = new Lives(new Vec2F(0.01f + i * 0.05f, 0.22f), 
                    new Image(Path.Combine("..", "Assets", "Images", "heart_filled.png")), 
                    new Image(Path.Combine("..", "Assets", "Images", "heart_empty.png")), i + 1);
                livesImage.AddEntity(Life);
            }
            effects = new EntityContainer<Effect>(); 
            effects.ClearContainer();
            collidedEffects = new EntityContainer<Effect>();
            timeData = level.Layout.GetMetaOrganized();
            timeInSec = -1;
            if (timeData.ContainsKey("Time")) {
                Int32.TryParse(timeData["Time"], out int t);
                timeInSec = t;
            }
            TimeLeftText = new Text("", new Vec2F(0.8f, 0.65f), new Vec2F(0.35f, 0.35f));
            TimeLeftText.SetColor(System.Drawing.Color.White);

            score = 0;
            prevScore = score;
            scoreText = new Text("Score: 0", new Vec2F(0.01f, 0.01f), new Vec2F(0.2f, 0.2f));
            scoreText.SetColor(System.Drawing.Color.White);
            collisionHandler = new CollisionHandler(player, level, balls, effects, collidedEffects, score);

        }
        
        public void SpawnPowerUp(Effect powerUp) {
            if (effects != null) {
                effects.AddEntity(powerUp);
            }
        }

        public void TimeRender() {
            if (TimeLeftText != null) {
                timeLeft = (int)(timeInSec - StaticTimer.GetElapsedSeconds() + 0.04);
                if (timeLeft >  (timeInSec - StaticTimer.GetElapsedSeconds())) {
                    timeLeftString = timeLeft.ToString();
                    TimeLeftText.SetText(timeLeftString);
                }
            }
        }

        public void ScoreRender() {
            if (prevScore < score) {
                prevScore = score;
                scoreText.SetText($"Score: {prevScore}");
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

        /// <summary>
        /// For collisions between ball and player,
        /// as well as ball and blocks.
        /// </summary>
        private void CheckCollisions() {
            collisionHandler.CheckCollisions();
            score = collisionHandler.GetScore();
        }

        /// <summary>
        /// For collisions between the player and the effects.
        /// </summary>
        public void CheckEffectCollisions() {
            collisionHandler.CheckEffectCollisions();
        }

        /// <summary>
        /// For activating effects upon collision
        /// </summary>
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

        /// <summary>
        /// For managing how long an effect lasts
        /// </summary>
        public void EffectTime(Player player, EntityContainer<Ball> balls1) {
            var currentTime = StaticTimer.GetElapsedSeconds();
            if (collidedEffects != null) {
                foreach (Effect effect in collidedEffects) {
                    Type effectType = effect.GetType();
                    if (!effect.IsDeactivated) {
                        if (currentTime > effect.ActivationTime + 5) {
                            MethodInfo? methodPlayer = effectType.GetMethod("DeactivatePlayer");
                            MethodInfo? methodBall = effectType.GetMethod("DeactivateBall");
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
                        else if (lastActivationTimes.ContainsKey(effectType) && lastActivationTimes[effectType] > effect.ActivationTime) {
                            effect.DeleteEntity();
                        }
                        else {
                            lastActivationTimes[effectType] = effect.ActivationTime;
                        }
                    }
                }
            }
        }

        public void DetractLife() {
            if (balls.CountEntities() == 0 && player.Lives > 0 && livesImage != null) {
                livesImage.Iterate(life => {
                    if (life.LifeNumber == player.Lives && life.IsFull) {
                        life.MakeEmpty();
                        life.IsFull = false;
                    }
                });
                player.Lives--;
                if (player.Lives > 0) {
                    balls.AddEntity(new Ball(new Vec2F(0.45f, 0.3f), ballsImage));
                    foreach (Ball ball in balls) {
                        float playerLeft = player.Shape.Position.X;
                        float playerRight = player.Shape.Position.X + player.Shape.Extent.X;
                        float playerMid = (playerRight + playerLeft) / 2.0f;
                        ball.Position.Y = player.Shape.Position.Y + 0.16f;
                        ball.Position.X = playerMid ;
                        ball.Direction.Y = -0.01f;
                        ball.Direction.X = 0.0f;
                    }
                }
            }
        }

        public void SetStopWatch() {
            if (StaticTimer.GetElapsedSeconds() >= timeInSec) {
                TimeOut = true;
            }
        }

        public static void AddTime() {
            timeInSec += 5;
        }
        public void SubtractTime() {
            timeInSec -= 5;
        }
        
        /// <summary>
        /// Either if the player is out of lives, or if the 
        /// timer runs out.
        /// </summary>
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

        /// <summary>
        /// If the user beats a level, the game automatically
        /// moves onto the next level until there are no levels
        /// left.
        /// </summary>
        public bool SwitchLevelIfWon() {
            string nextLevelFile = level.GetNextLevelFile();

            if (File.Exists(nextLevelFile)) {
                StaticTimer.RestartTimer();
                level.LoadLevel(nextLevelFile);
                timeData = level.Layout.GetMetaOrganized();
                if (timeData.ContainsKey("Time")) {
                    Int32.TryParse(timeData["Time"], out int t);
                    timeInSec = t;
                }
                

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

        /// <summary>
        /// Game is won if all levels are beat, or if a certain score is reached
        /// </summary>
        public bool IsGameWon() {
            if (level.GetBlocks().CountEntities() == 0 || score == 10000) {
                if (!SwitchLevelIfWon() || score == 10000) {
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

        /// <summary>
        /// Everything from the ball, to player lives, to effects
        /// are rendered.
        /// </summary>
        public void RenderState() {
            backGroundImage.RenderEntity();
            overLayImage.RenderEntity();
            player.Render();
            level.GetBlocks().RenderEntities();
            balls.RenderEntities();
            if (TimeLeftText != null) {
                TimeLeftText.RenderText();
            }
            if (scoreText != null) {
                scoreText.RenderText();
            }
            if (livesImage != null) {
                livesImage.RenderEntities();
            }
            if (effects != null) {
                effects.RenderEntities();
            }
            level.GetBlocks().RenderEntities();
        }

        /// <summary>
        /// The timer also gets reset.
        /// </summary>
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
            ScoreRender();
            CheckEffectCollisions();
            EffectTime(player, balls);
        }

        public void NullInstance() {
            instance = null;
        }
    }
}