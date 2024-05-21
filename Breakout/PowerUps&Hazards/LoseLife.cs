using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Breakout.BreakoutStates;
using Breakout;

namespace Breakout.PowerUps {
    public class LoseLife : Effect {
        public LoseLife(DynamicShape shape, IBaseImage image) 
            : base(EffectType.PlayerSpeed, shape, image) {
        }

        public override void ActivatePlayer(Player player) {
            int oneLife = 0;
            GameRunning.LivesImage.Iterate(life => {
                    if (life.LifeNumber == player.Lives && life.IsFull && oneLife == 0) {
                        life.MakeEmtpy();
                        life.IsFull = false;
                        player.Lives--;
                        oneLife = 1;
                    }
            });
            oneLife = 0;
        }
    }
}
