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
        public bool isEmpty = false;
        private Image empty;
        private Image full;
        public int LifeNumber { get; private set; }
        public bool IsFull {
            get { return isFull; }
            set { isFull = value; }
        }
        public bool IsEmpty {
            get { return isEmpty; }
            set { isEmpty = value; }
        }
        public Image Empty {
            get {return empty; }
        }
        public Vec2F Position {
            get { return position; }
        }
        public Vec2F Extent {
            get { return extent; }
        }
        public Lives (Vec2F position, Image full, Image empty, int lifeNumber) : 
            base(new StationaryShape(position, new Vec2F(0.04f, 0.04f)), full) {
                this.position = position;
                this.extent = new Vec2F(0.04f, 0.04f);
                this.full = full;
                this.empty = empty;
                this.LifeNumber = lifeNumber;
        }

        public void MakeEmpty() {
            this.Image = empty;
            isFull = false;
            isEmpty = true;
        }

        public void MakeFull() {
            this.Image = full;
            isFull = true;
            isEmpty = false;
        }
    }
}