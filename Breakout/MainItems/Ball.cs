using System;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout;
public class Ball : Entity {
    private Random rnd = new Random();
    private Vec2F position;
    private static Vec2F extent = new Vec2F(0.035f, 0.035f);
    private static Vec2F direction = new Vec2F(0.0000f, -0.01f);
    public Ball(Vec2F position, IBaseImage image)
        : base(new DynamicShape(position, extent, direction), image) {
            this.position = position;
    }

    public Vec2F Direction {
        get { return direction; }
        set { direction = value; }
    }

    public Vec2F Position {
        get {return Shape.Position; }
    }

    public void SetDirectionY(float newDirY) {
        direction.Y = newDirY;
    }

    public void SetExtent() {
        extent = new Vec2F(0.035f, 0.035f);
    }

    public void EnsureOppositeXDir() {
        if (direction.X < 0.0f) {
                direction.X = (float)rnd.Next(-100, 0) * 0.000075f;
            } else {
                direction.X = (float)rnd.Next(0, 100) * 0.000075f;
            }
    }

    public void HitsBlockMove() {
        if (direction.Y < 0.0f) {
            EnsureOppositeXDir();
            direction.Y = 0.01f;
        } else if (direction.Y > 0.0f) {
            EnsureOppositeXDir();
            direction.Y = -0.01f;
        }
    } 

    public void GoLeft() {
        direction.X = (float)rnd.Next(-100, -1) * 0.000075f;
        direction.Y = 0.01f;
    }

    public void GoRight() {
        direction.X = (float)rnd.Next(1, 100) * 0.000075f;
        direction.Y = 0.01f;
    }
    
    public void Movement() {
        Shape.Move();
        Random rnd = new Random();
        if (Shape.Position.Y > 1.0f - Shape.Extent.Y) {
            EnsureOppositeXDir();
            direction.Y = -0.01f;
        } else if (Shape.Position.X > 1.0f - Shape.Extent.X) {
            direction.X = (float)rnd.Next(-100, 0) * 0.000075f;
        } else if (Shape.Position.X < 0.0f) {
            direction.X = (float)rnd.Next(0, 100) * 0.000075f;
        } 
    }

    public void CheckDeleteBall() {
        if (Shape.Position.Y < 0.0f) {
            DeleteEntity();
        }
    }

    public Vec2F GetPosition() {
        return position;
    }
}