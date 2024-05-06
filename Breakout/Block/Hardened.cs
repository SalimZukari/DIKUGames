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
public class Hardened : Block {
    public Hardened(DynamicShape shape, Image blocksImage, BlockType type) 
        : base(shape, blocksImage, type) {
            health = 20;
    }
    public override void Damage() {
        health -= 10;
        if (health <= 0) {
            Destroy();
        }
    }
}