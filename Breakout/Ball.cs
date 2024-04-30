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

    private static Vec2F extent = new Vec2F(0.008f, 0.021f);
    private static Vec2F direction = new Vec2F(0.0f, 0.1f);

    public Ball(Vec2F position, IBaseImage image)
        : base(new DynamicShape(position, extent, direction), image) {
    }

    // private bool IsCollide

    private void ChangeDirections() {
        Random rnd = new Random();
        if (direction.Y < 0.0f) {
            direction.X = (float)rnd.Next(1, -1);
            direction.Y = 1.0f;
        } else if (direction.Y > 0.0f) {
            direction.X = (float)rnd.Next(1, -1);
            direction.Y = -1.0f;
        }
    }


}