using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Breakout.PowerUps {
    public class PowerUp : Entity {
        public PowerUpType Type { get; private set; }
        protected float duration; 
        public bool IsActive { get; private set; }
        private static readonly float speed = 0.01f; 

        public PowerUp(PowerUpType type, DynamicShape shape, IBaseImage image, float duration)
            : base(shape, image) {
            this.Type = type;
            this.duration = duration;
            IsActive = true;
        }

        public virtual void Activate(Player player) {
            IsActive = true;
        }

        public virtual void Activate(Ball ball) {
            IsActive = true;
        }

        public void Deactivate() {
            DeleteEntity();
            IsActive = false;
        }

        public void Update() {
            if (IsActive) {
                duration -= 1.0f;
                Shape.MoveY(-speed); 
                if (duration <= 0 || Shape.Position.Y < 0.0f) {
                    Deactivate();
                }
            }
        }
    }
}
