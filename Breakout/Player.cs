using System;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout;
public class Player : IGameEventProcessor {
    private Entity entity;
    private DynamicShape shape;
    private float moveRight = 0.0f;
    private float moveLeft = 0.0f;
    private float MOVEMENT_SPEED = 0.01f;

    public Player(DynamicShape shape, IBaseImage image) {
        entity = new Entity(shape, image);
        this.shape = shape;
        BreakoutBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);
    }

    public DynamicShape Shape {
        get { return shape; }
        set { shape = value; }
    }

    public float MoveRight {
        get { return moveRight; }
    }

    public float MoveLeft {
        get { return moveLeft; }
    }

    private void UpdateDirection() {
        shape.Direction.X = moveLeft + moveRight;
    }

    public void Render() {
        entity.RenderEntity();
    }

    public void Move() {
        shape.Move();
        if (shape.Position.X < 0.0f) {
            shape.Position.X = 0.0f;
        } else if (shape.Position.X + shape.Extent.X > 1.0f) {
            shape.Position.X = 1.0f - shape.Extent.X;
        }
    }

    private void SetMoveLeft(bool val) {
        if (val) {
            moveLeft -= MOVEMENT_SPEED;
        } else {
            moveLeft = 0.0f;
        } UpdateDirection();
    }

    private void SetMoveRight(bool val) {
        if (val) {
            moveRight += MOVEMENT_SPEED;
        } else {
            moveRight = 0.0f;
        } UpdateDirection();
    }

    public Vec2F GetPosition() {
        return shape.Position;
    }

    public void ResetPosition() {
        shape.Position = new Vec2F(0.45f, 0.1f);
    }

    public void ProcessEvent(GameEvent gameEvent) {
        if (gameEvent.EventType == GameEventType.PlayerEvent) {
            switch (gameEvent.Message) {
                case "Move_left":
                    SetMoveLeft(true);
                break;
                case "Move_right":
                    SetMoveRight(true);
                break;
                case "No_move_left":
                    SetMoveLeft(false);
                break;
                case "No_move_right":
                    SetMoveRight(false);
                break;
            }
        }
    }
}
