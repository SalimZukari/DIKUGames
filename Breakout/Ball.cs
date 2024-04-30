using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade;
using DIKUArcade.Events;
using DIKUArcade.Input;
using System.Collections.Generic;
using DIKUArcade.GUI;
using DIKUArcade.Physics;

namespace Breakout;

public class Ball : Entity {
    private Entity entity;
    private DynamicShape shape;
    private float MOVEMENT_SPEED;

    /* public Ball(DynamicShape shape, IBaseImage image) {
        entity = new Entity(shape, image);
        this.shape = shape;
    } */

    private static Vec2F extent = new Vec2F(0.035f, 0.035f);
    public static Vec2F direction = new Vec2F(0.0f, 0.01f);
    private bool isMovingUp;
    public Ball(Vec2F position, IBaseImage image)
        : base(new DynamicShape(position, extent, direction), image) {
            isMovingUp = true;
    }

   public void ChangeDirection() {
        Random rnd = new Random();
        if (direction.Y < 0.0f) {
            direction.X = (float)rnd.Next(-100, 100) * 0.000075f;
            direction.Y = 0.01f;
        } else if (direction.Y > 0.0f) {
            direction.X = (float)rnd.Next(-100, 100) * 0.000075f;
            direction.Y = -0.01f;
        }
    }
    

    public void ChangeDirection1() {
        Random rnd = new Random();
        if (Shape.Position.X <= 0.0f + Shape.Extent.X) {
            direction.X = (float)rnd.Next(0, 100) * 0.000075f;
        } else if (Shape.Position.X >= 1.0f - Shape.Extent.X) {
            direction.X = (float)rnd.Next(-100, 0) * 0.000075f;
        }
    }
    public void Movement() {
        Shape.Move();
        if (Shape.Position.Y > 1.0f - Shape.Extent.Y) {
            ChangeDirection1();
        } else if (Shape.Position.X > 1.0f - Shape.Extent.X) {
            ChangeDirection1();
        } else if (Shape.Position.X < 0.0f + Shape.Extent.X) {
            ChangeDirection1();
        } 
    }
}