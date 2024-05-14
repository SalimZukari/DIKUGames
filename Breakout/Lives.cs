using System;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout {
    public class Lives : Entity {
        private Vec2F position;
        private Vec2F extent;
        public bool isFull = true;
        public bool IsFull {
            get { return isFull; }
            set { isFull = value; }
        }
        private Image empty;
        public Image Empty {
            get {return empty; }
        }
        public int LifeNumber { get; private set; }

            
        public Lives (Vec2F position, Image full, Image empty, int lifeNumber) : 
            base(new StationaryShape(position, new Vec2F(0.04f, 0.04f)), full) {
                this.position = position;
                this.extent = new Vec2F(0.04f, 0.04f);
                this.Image = full;
                this.empty = empty;
                this.LifeNumber = lifeNumber;
        }

        public void ChangeImage() {
            this.Image = empty;
            isFull = false;
        }
    }
}