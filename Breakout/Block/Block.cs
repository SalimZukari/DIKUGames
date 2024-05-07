using System;
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

namespace Breakout.IBlock;
public class Block : Entity, IBlock {
    private Vec2F startPos; 
    protected int health;
    public Image blocksImage;
    protected BlockType type;
    public Image damagedImage;

    public int Health {
        get { return health; }
    }

    public Block(DynamicShape shape, Image blocksImage, Image damagedImage, BlockType type) 
        : base(shape, blocksImage) {
            this.blocksImage = blocksImage;
            this.startPos = shape.Position;
            this.damagedImage = damagedImage;
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

    public virtual Image GetImage() {
        return blocksImage;
    }
    public virtual Image GetDamagedImage() {
        return damagedImage;
    }
}