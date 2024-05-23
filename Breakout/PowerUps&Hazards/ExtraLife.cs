using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Breakout.BreakoutStates;
using Breakout;

namespace Breakout.PowerUps {
    public class ExtraLife : Effect {
        public ExtraLife(DynamicShape shape, IBaseImage image) 
            : base(EffectType.ExtraLife, shape, image) {
        }

        public override void ActivatePlayer(Player player) {
            int oneLife = 0;
            if (player.Lives < 3 && GameRunning.LivesImage != null) {
                GameRunning.LivesImage.Iterate(life => {
                    if (life.LifeNumber == (player.Lives + 1) && life.IsEmpty && oneLife == 0) {
                        life.MakeFull();
                        life.IsFull = true;
                        player.Lives++;
                        oneLife = 1;
                    }
                });
            }
            oneLife = 0;
        }
    }
}

