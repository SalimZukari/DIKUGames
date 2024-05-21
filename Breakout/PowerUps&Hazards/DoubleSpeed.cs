using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Timers;
using DIKUArcade.Math;
using Breakout.BreakoutStates;
using Breakout;

namespace Breakout.PowerUps {
    public class DoubleSpeed : Effect {
        public DoubleSpeed(DynamicShape shape, IBaseImage image) 
            : base(EffectType.DoubleSpeed, shape, image) {
                HasDuration = true;
        }

        public override void ActivateBall(Ball ball) {
            ball.Direction.X *= 2;
            ball.Direction.Y *= 2;
        }
    }
}


