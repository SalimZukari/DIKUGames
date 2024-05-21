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
        }

        public override void ActivateBall(Ball ball) {
            var currentTime = StaticTimer.GetElapsedSeconds();
            ball.Direction *= 2;
            Deactivate(); 
        }
    }
}


