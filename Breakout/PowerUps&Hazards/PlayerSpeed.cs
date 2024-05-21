using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Breakout.BreakoutStates;
using DIKUArcade.Timers;
using Breakout;

namespace Breakout.PowerUps {
    public class PlayerSpeed : Effect {
        private float originalSpeed;

        public PlayerSpeed(DynamicShape shape, IBaseImage image) 
            : base(EffectType.PlayerSpeed, shape, image) {
        }

        public override void ActivatePlayer(Player player) {
            originalSpeed = player.MovementSpeed;
            var currentTime = StaticTimer.GetElapsedSeconds();
            player.MovementSpeed = 0.02f;
            player.MovementSpeed = 0.01f;
            Deactivate();
        }
    }
}


