using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;
using Breakout.BreakoutStates;
using Breakout;

namespace Breakout.PowerUps {
    public class DoubleSize : Effect {
        public DoubleSize(DynamicShape shape, IBaseImage image) 
            : base(EffectType.DoubleSize, shape, image) {
        }

        public override void Activate(Ball ball) {
            var currentTime = StaticTimer.GetElapsedSeconds();
            while (StaticTimer.GetElapsedSeconds() <= currentTime + duration) {
                ball.Shape.Extent *= 2;
            }
            Deactivate(); 
        }
    }
}


