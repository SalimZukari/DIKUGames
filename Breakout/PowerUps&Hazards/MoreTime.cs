using Breakout;
using DIKUArcade.Math;
using DIKUArcade.Timers;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using Breakout.BreakoutStates;

namespace Breakout.PowerUps {
    public class MoreTime : Effect {
        /// <summary>
        /// Adds extra time to the game
        /// </summary>
        public MoreTime(DynamicShape shape, IBaseImage image) 
            : base(EffectType.MoreTime, shape, image) {
                HasDuration = true;
        }

        public override void ActivateBall(Ball ball) {
            GameRunning.AddTime();
        }
    }
}


