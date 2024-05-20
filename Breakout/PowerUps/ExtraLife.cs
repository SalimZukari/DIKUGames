using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Breakout.BreakoutStates;
using Breakout;

namespace Breakout.PowerUps {
    public class ExtraLife : PowerUp {
        public ExtraLife(DynamicShape shape, IBaseImage image) 
            : base(PowerUpType.ExtraLife, shape, image) {
        }

        public override void Activate(Player player) {
            player.Lives++;
            Deactivate();
        }
    }
}

