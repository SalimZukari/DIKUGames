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
            if (player.Lives < 3) {
                GameRunning.LivesImage.Iterate(life => {
                    if (life.LifeNumber == (player.Lives + 1) && life.IsEmpty) {
                        life.MakeFull();
                        life.IsFull = true;
                        player.Lives++;
                    }
                });
            }
            Deactivate();
        }
    }
}

