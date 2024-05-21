using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Timers;
using DIKUArcade.Math;
using Breakout.BreakoutStates;
using Breakout;

namespace Breakout.PowerUps {
    public class Slowness : Effect {
        private float originalSpeed;
        public Slowness(DynamicShape shape, IBaseImage image) 
            : base(EffectType.ExtraLife, shape, image) {
        }

        public override void ActivatePlayer(Player player) {
            originalSpeed = player.MovementSpeed;
            var currentTime = StaticTimer.GetElapsedSeconds();
            while (StaticTimer.GetElapsedSeconds() <= currentTime + duration) {
                player.MovementSpeed /= 2;
            }
            Deactivate();
        }
    }
}

