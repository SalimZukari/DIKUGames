using System;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;


namespace Breakout.IBlock;
public class Block : Entity {
    /// <summary>
    /// The base block, with all normal attributes
    /// </summary>
    private Vec2F startPos; 
    protected int health;
    protected Image blocksImage;
    protected BlockType type;
    protected Image damagedImage;

    public int Health {
        get { return health; }
    }

    public Image BlocksImage {
        get { return blocksImage; }
        protected set { blocksImage = value; } 
    }

    public Block(DynamicShape shape, Image blocksImage, Image damagedImage, BlockType type) 
        : base(shape, blocksImage) {
            this.blocksImage = blocksImage;
            this.startPos = shape.Position;
            this.damagedImage = damagedImage;
            this.type = type;
            health = 10;
    }

    /// <summary>
    /// For when a block takes damage.
    /// </summary>
    public virtual void Damage() {
        health -= 10;
        if (health <= 0) {
            Destroy();
        }
    }

    /// <summary>
    /// When the block has zero health.
    /// </summary>
    public virtual void Destroy() {
        DeleteEntity();
    }

    public virtual new BlockType GetType() {
        return type;
    }

    /// <summary>
    /// Used in level set up class,
    /// for example to switch a normal block to
    /// a hazard block.
    /// </summary>
    public virtual void SwitchType(BlockType newType) {
        type = newType;
    }

    public virtual Vec2F GetPosition() {
        return startPos;
    }

    public virtual Image GetImage() {
        return blocksImage;
    }

    /// <summary>
    /// For switching the image of the block.
    /// </summary>
    public virtual Image GetDamagedImage() {
        return damagedImage;
    }
}