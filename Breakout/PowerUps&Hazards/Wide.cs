using Breakout;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout.PowerUps {
    public class Wide : Effect {
        /// <summary>
        /// Makes the board the player controls twice as wide.
        /// </summary>
        public Wide(DynamicShape shape, IBaseImage image) 
            : base(EffectType.Wide, shape, image) {
                HasDuration = true;
        }

        /// <summary>
        /// Doubles the width of the player's board.
        /// </summary>
        public override void ActivatePlayer(Player player) {
            if (player.Shape.Extent.X < 0.38f) {
                player.Shape.Extent.X *= 2;
                IsActive = true;
            }
        }

        /// <summary>
        /// Halves the width of the player's board.
        /// </summary>
        public override void DeactivatePlayer(Player player) {
            if (player.Shape.Extent.X > 0.22f) {
                player.Shape.Extent.X /= 2;
                IsActive = false;
            }
        }
    }
}


