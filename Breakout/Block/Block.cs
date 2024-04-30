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
    private int health;
    private IBaseImage blocksImage;

    public int Health {
        get { return health; }
    }

    public Block(DynamicShape shape, IBaseImage blocksImage) : base(shape, blocksImage) {
        this.blocksImage = blocksImage;
        this.startPos = shape.Position;
        health = 10;
    }

    public void Damage() {
        health -= 10;
        if (health <= 0) {
            Destroy();
        }
    }

    private void Destroy() {
        DeleteEntity();
    }
}
