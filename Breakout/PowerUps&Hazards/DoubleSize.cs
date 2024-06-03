using Breakout;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using Breakout.BreakoutStates;

namespace Breakout.PowerUps {
    public class DoubleSize : Effect {
        public DoubleSize(DynamicShape shape, IBaseImage image) 
            : base(EffectType.DoubleSize, shape, image) {
                HasDuration = true;
        }

        public override void ActivateBall(Ball ball) {
            if (ball.Shape.Extent.X < 0.07f && ball.Shape.Extent.Y < 0.07f) {
                ball.Shape.Extent.X *= 2;
                ball.Shape.Extent.Y *= 2;
                IsActive = true;
            }
        }

        public override void DeactivateBall(Ball ball) {
            if (ball.Shape.Extent.X > 0.035f && ball.Shape.Extent.Y > 0.035f) {
                ball.Shape.Extent.X /= 2;
                ball.Shape.Extent.Y /= 2;
                IsActive = false;
            }
        }
    }
}


