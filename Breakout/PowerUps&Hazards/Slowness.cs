using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Timers;
using DIKUArcade.Math;
using Breakout.BreakoutStates;
using Breakout;

namespace Breakout.PowerUps {
    public class Slowness : Effect {
        public Slowness(DynamicShape shape, IBaseImage image) 
            : base(EffectType.ExtraLife, shape, image) {
                HasDuration = true;
        }

        public override void ActivatePlayer(Player player) {
            if (player.MovementSpeed > 0.005f) {
                player.MovementSpeed /= 2;
                IsActive = true;
            }
        }

        public override void DeactivatePlayer(Player player) {
            if (player.MovementSpeed < 0.009) {
                player.MovementSpeed *= 2;
                IsActive = false;
            }
        }
    }
}

