using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Timers;
using DIKUArcade.Math;
using Breakout.BreakoutStates;
using Breakout;

namespace Breakout.PowerUps {
    public class Wide : Effect {
        public Wide(DynamicShape shape, IBaseImage image) 
            : base(EffectType.Wide, shape, image) {
                HasDuration = true;
        }

        public override void ActivatePlayer(Player player) {
            if (player.Shape.Extent.X < 0.38f) {
                player.Shape.Extent.X *= 2;
                IsActive = true;
            }
        }

        public override void DeactivatePlayer(Player player) {
            if (player.Shape.Extent.X > 0.22f) {
                player.Shape.Extent.X /= 2;
                IsActive = false;
            }
        }
    }
}


