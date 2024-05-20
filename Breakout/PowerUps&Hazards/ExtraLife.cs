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

        public override void Activate(Player player) {
            GameRunning.LivesImage.Iterate(life => {
                    if (life.LifeNumber == player.Lives && !life.IsFull) {
                        life.MakeFull();
                        life.IsFull = false;
                    }
            });
            player.Lives++;
            Deactivate();
        }
    }
}

