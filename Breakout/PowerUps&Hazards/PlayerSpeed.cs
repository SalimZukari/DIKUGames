using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Breakout.BreakoutStates;
using DIKUArcade.Timers;
using Breakout;

namespace Breakout.PowerUps {
    public class PlayerSpeed : Effect {

        public PlayerSpeed(DynamicShape shape, IBaseImage image) 
            : base(EffectType.PlayerSpeed, shape, image) {
                HasDuration = true;
        }

        public override void ActivatePlayer(Player player) {
            if (player.MovementSpeed < 0.02f) {
                player.MovementSpeed *= 2;
                IsActive = true;
            }
        }

        public override void DeactivatePlayer(Player player) {
            if (player.MovementSpeed > 0.01f) {
                player.MovementSpeed /= 2;
                IsActive = false;
            }
        }
    }
}


