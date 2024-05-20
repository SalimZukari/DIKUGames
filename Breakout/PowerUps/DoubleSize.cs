using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Breakout.BreakoutStates;
using Breakout;

namespace Breakout.PowerUps {
    public class DoubleSize : PowerUp {
        public DoubleSize(DynamicShape shape, IBaseImage image) 
            : base(PowerUpType.DoubleSize, shape, image) {
        }

        public override void Activate(Ball ball) {
            ball.Shape.Extent *= 2;
            Deactivate(); 
        }
    }
}


