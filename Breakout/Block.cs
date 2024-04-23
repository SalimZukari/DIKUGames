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

public class Block : Entity {
    private Vec2F startPos; 
    private int hitpoints = 30;
    private IBaseImage blocksImage;
    public int Hitpoints {
        get { return hitpoints; }
    }
    public Block(DynamicShape shape, IBaseImage enemyStrides) : base(shape, enemyStrides) {
        this.blocksImage = blocksImage;
        this.startPos = shape.Position;
    }
    public void Damage() {
        hitpoints -= 10;
        if (hitpoints <= 0) {
            Destroy();
        }
    }
    private void Destroy() {
        DeleteEntity();
    }
}
