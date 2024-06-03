using System;

namespace Breakout.BreakoutStates;
public enum GameStateType {
    GAME_RUNNING,
    GAME_PAUSED,
    MAIN_MENU,
    GAME_LOST,
    GAME_WON
}

public class StateTransformer {
    /// <summary>
    /// The event bus takes string arguments,
    /// so this converts enumerators to strings and
    /// strings to enumerators.
    /// </summary>

    /// <summary>
    /// Tries to parse a string to an enumerator.
    /// </summary>
    public static GameStateType TransformStringToState(string state) {
        if (Enum.TryParse(state, out GameStateType stateType)) {
            return stateType;
        } else {
            throw new ArgumentException("Invalid state type");
        }
    }

    /// <summary>
    /// Turns an enumerator into a string.
    /// </summary>
    public static string TransformStateToString(GameStateType state) {
        if (!Enum.IsDefined(typeof(GameStateType), state)) {
            throw new ArgumentException("Invalid state type");
        } return state.ToString();
    }
}

