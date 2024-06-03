using System;
using DIKUArcade.Events;


namespace Breakout;
public static class BreakoutBus {
    /// <summary>
    /// Manages new events
    /// </summary>
    private static GameEventBus? eventBus;
    
    public static GameEventBus GetBus() {
        return BreakoutBus.eventBus ?? (BreakoutBus.eventBus =
                                    new GameEventBus());
    }
}
