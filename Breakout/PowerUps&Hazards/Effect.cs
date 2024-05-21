using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;

namespace Breakout.PowerUps {
    public class Effect : Entity {
        public EffectType Type { get; private set; }
        protected int duration; 
        public bool IsActive { get; private set; }
        private static readonly float speed = 0.01f; 

        public Effect(EffectType type, DynamicShape shape, IBaseImage image)
            : base(shape, image) {
            this.Type = type;
            IsActive = true;
            this.duration = 5;
        }

        public virtual void ActivatePlayer(Player player) {
            IsActive = true;
        }

        public virtual void ActivateBall(Ball ball) {
            IsActive = true;
        }

        public void Deactivate() {
            DeleteEntity();
            IsActive = false;
        }

        public void Update() {
            if (IsActive) {
                Shape.MoveY(-speed); 
                if (Shape.Position.Y < 0.0f) {
                    Deactivate();
                }
            }
        }
    }
}
