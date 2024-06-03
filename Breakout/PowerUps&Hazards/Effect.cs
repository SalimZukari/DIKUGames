using Breakout;
using DIKUArcade.Math;
using DIKUArcade.Timers;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using Breakout.BreakoutStates;

namespace Breakout.PowerUps {
    public class Effect : Entity {
        /// <summary>
        /// The base effect, no special attributes
        /// </summary>
        protected int duration;
        private static readonly float speed = 0.01f;
        public bool HasDuration { get; protected set; }
        public EffectType Type { get; private set; }
        public bool IsActive { get; protected set; }
        public double ActivationTime { get;  set; } 
        public bool IsDeactivated { get;  set; } 

        public Effect(EffectType type, DynamicShape shape, IBaseImage image)
            : base(shape, image) {
            this.Type = type;
            IsActive = true;
            this.duration = 5;
            HasDuration = false;
            ActivationTime = StaticTimer.GetElapsedSeconds(); // Gemmer activation time
            IsDeactivated = false; 
        }

        public virtual void ActivatePlayer(Player player) {
            IsActive = true;
        }

        public virtual void ActivateBall(Ball ball) {
            IsActive = true;
        }

        public virtual void DeactivatePlayer(Player player) {
            DeleteEntity();
            IsActive = false;
        }

        public virtual void DeactivateBall(Ball ball) {
            DeleteEntity();
            IsActive = false;
        }

        public void Update() {
            if (IsActive) {
                Shape.MoveY(-speed);
            }
        }
    }
}
