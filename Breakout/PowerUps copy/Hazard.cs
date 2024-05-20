using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Breakout.PowerUps {
    public class Hazard : Entity {
        public PowerUpType Type { get; private set; }
        protected float duration; 
        public bool IsActive { get; private set; }
        private static readonly float speed = 0.01f; 

        public Hazard(HazardType type, DynamicShape shape, IBaseImage image)
            : base(shape, image) {
            this.Type = type;
            IsActive = true;
        }

        public virtual void Activate(Player player) {
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
