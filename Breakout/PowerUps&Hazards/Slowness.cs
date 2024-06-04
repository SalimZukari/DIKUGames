using Breakout;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using Breakout.BreakoutStates;

namespace Breakout.PowerUps {
    public class Slowness : Effect {
        /// <summary>
        /// Decreases the player's speed
        /// </summary>
        public Slowness(DynamicShape shape, IBaseImage image) 
            : base(EffectType.ExtraLife, shape, image) {
                HasDuration = true;
        }

        /// <summary>
        /// Halves the player's speed
        /// </summary>
        public override void ActivatePlayer(Player player) {
            if (player.MovementSpeed > 0.005f) {
                player.MovementSpeed /= 2;
                IsActive = true;
            }
        }

        /// <summary>
        /// Doubles the player's speed
        /// </summary>
        public override void DeactivatePlayer(Player player) {
            if (player.MovementSpeed < 0.009) {
                player.MovementSpeed *= 2;
                IsActive = false;
            }
        }
    }
}

