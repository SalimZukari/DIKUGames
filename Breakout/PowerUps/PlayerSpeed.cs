using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Breakout.BreakoutStates;
using Breakout;

namespace Breakout.PowerUps {
    public class PlayerSpeed : PowerUp {
        private float originalSpeed;

        public PlayerSpeed(DynamicShape shape, IBaseImage image, float duration) 
            : base(PowerUpType.PlayerSpeed, shape, image, duration) {
        }

        public override void Activate(Player player) {
            originalSpeed = player.MovementSpeed;
            player.MovementSpeed *= 2;
            Deactivate();
        }
    }
}


