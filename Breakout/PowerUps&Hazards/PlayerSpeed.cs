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
            var currentTime = StaticTimer.GetElapsedSeconds();
            player.MovementSpeed *= 2;
        }

        public override void DeactivatePlayer(Player player) {
            player.MovementSpeed /= 2;
        }
    }
}


