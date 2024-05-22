using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Timers;
using DIKUArcade.Math;
using Breakout.BreakoutStates;
using Breakout;

namespace Breakout.PowerUps {
    public class MoreTime : Effect {
        public MoreTime(DynamicShape shape, IBaseImage image) 
            : base(EffectType.MoreTime, shape, image) {
                HasDuration = true;
        }

        public override void ActivateBall(Ball ball) {
            GameRunning.AddTime();
        }
    }
}


