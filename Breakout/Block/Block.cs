using System;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;


namespace Breakout.IBlock;
public class Block : Entity, IBlock {
    private Vec2F startPos; 
    protected int health;
    private BlockType type;
    private IBaseImage blocksImage;

    public int Health {
        get { return health; }
    }

    public Block(DynamicShape shape, IBaseImage blocksImage, BlockType type) 
        : base(shape, blocksImage) {
            this.blocksImage = blocksImage;
            this.startPos = shape.Position;
            this.type = type;
            health = 10;
    }

    public virtual void Damage() {
        health -= 10;
        if (health <= 0) {
            Destroy();
        }
    }

    public virtual void Destroy() {
        DeleteEntity();
    }

    public virtual BlockType GetType() {
        return type;
    }

    public virtual Vec2F GetPosition() {
        return startPos;
    }

    public virtual IBaseImage GetImage() {
        return blocksImage;
    }
}
