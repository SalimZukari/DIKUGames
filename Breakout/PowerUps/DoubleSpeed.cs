using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Breakout.BreakoutStates;
using Breakout;

namespace Breakout.PowerUps {
    public class DoubleSpeed : PowerUp {
        public DoubleSpeed(DynamicShape shape, IBaseImage image) 
            : base(PowerUpType.DoubleSpeed, shape, image) {
        }

        public override void Activate(Ball ball) {
            ball.Direction *= 2;
            Deactivate(); 
        }
    }
}


