using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Breakout.BreakoutStates;
using Breakout;

namespace Breakout.PowerUps {
    public class ExtraLife : PowerUp {
        public ExtraLife(DynamicShape shape, IBaseImage image, float duration) 
            : base(PowerUpType.ExtraLife, shape, image, duration) {
        }

        public override void Activate(Player player) {
            player.Lives++;
            Deactivate();
        }
    }
}

