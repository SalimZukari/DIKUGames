using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Breakout.BreakoutStates;
using Breakout;

namespace Breakout.PowerUps {
    public class PlayerSpeed : PowerUp {
        private float originalSpeed;

        public PlayerSpeed(DynamicShape shape, IBaseImage image) 
            : base(PowerUpType.PlayerSpeed, shape, image) {
        }

        public override void Activate(Player player) {
            originalSpeed = player.MovementSpeed;
            player.MovementSpeed *= 2;
            Deactivate();
        }
    }
}


