using Breakout;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using Breakout.BreakoutStates;

namespace Breakout.PowerUps {
    public class LoseLife : Effect {
        public LoseLife(DynamicShape shape, IBaseImage image) 
            : base(EffectType.PlayerSpeed, shape, image) {
        }

        public override void ActivatePlayer(Player player) {
            int oneLife = 0;
            if (GameRunning.LivesImage != null) {
                GameRunning.LivesImage.Iterate(life => {
                        if (life.LifeNumber == player.Lives && life.IsFull && oneLife == 0) {
                            life.MakeEmpty();
                            life.IsFull = false;
                            player.Lives--;
                            oneLife = 1;
                        }
                });
                oneLife = 0;
            }
        }
    }
}
