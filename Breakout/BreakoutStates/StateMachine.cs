using DIKUArcade.State;
using DIKUArcade.Events;
using Breakout.PowerUps;
using Breakout.BreakoutStates;

namespace Breakout.BreakoutStates {
    public class StateMachine : IGameEventProcessor {
        /// <summary>
        /// Keeps track of the current state and any state changes
        /// </summary>
        public IGameState ActiveState { get; private set; }
        
        public StateMachine() {
            BreakoutBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            BreakoutBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            ActiveState = MainMenu.GetInstance();
            GameRunning.GetInstance();
            GamePaused.GetInstance();
            GameLost.GetInstance();
            GameWon.GetInstance();
        }

        /// <summary>
        /// Activates the appropriate state.
        /// </summary>
        public void SwitchState(GameStateType state) {
            switch (state) {
                case GameStateType.GAME_RUNNING:
                    ActiveState = GameRunning.GetInstance();
                    break;
                case GameStateType.GAME_PAUSED:
                    ActiveState = GamePaused.GetInstance();
                    break;
                case GameStateType.MAIN_MENU:
                    ActiveState = MainMenu.GetInstance();
                    break;
                case GameStateType.GAME_LOST:
                    ActiveState = GameLost.GetInstance();
                    break;
                case GameStateType.GAME_WON:
                    ActiveState = GameWon.GetInstance();
                    break;
                default:
                    throw new ArgumentException("Cannot switch to unknown state");
            }
        }

        /// <summary>
        /// Processes events so that the state can be changed.
        /// </summary>
        public void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.GameStateEvent) {
                if (gameEvent.Message == "CHANGE_STATE") {
                    SwitchState(StateTransformer.TransformStringToState(gameEvent.StringArg1));
                } else if (gameEvent.Message == "SPAWN_POWERUP") {
                    if (ActiveState is GameRunning gameRunning) {
                        var powerUp = gameEvent.ObjectArg1 as Effect;
                        if (powerUp != null) {
                            gameRunning.SpawnPowerUp(powerUp);
                        }
                    }
                }
            }
        }
    }
}


