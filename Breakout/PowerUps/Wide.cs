using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Breakout.BreakoutStates;
using Breakout;

namespace Breakout.PowerUps {
    public class Wide : PowerUp {
        public Wide(DynamicShape shape, IBaseImage image, float duration) 
            : base(PowerUpType.Wide, shape, image, duration) {
        }

        public override void Activate(Player player) {
            player.Shape.Extent.X *= 2;
            Deactivate(); 
        }
    }
}


