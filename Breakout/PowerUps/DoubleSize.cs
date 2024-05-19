using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Breakout.BreakoutStates;
using Breakout;

namespace Breakout.PowerUps {
    public class DoubleSize : PowerUp {
        public DoubleSize(DynamicShape shape, IBaseImage image, float duration) 
            : base(PowerUpType.DoubleSize, shape, image, duration) {
        }

        public override void Activate(Ball ball) {
            ball.Shape.Extent *= 2;
            Deactivate(); 
        }
    }
}


