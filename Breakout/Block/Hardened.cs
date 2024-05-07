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

    protected new Image damagedImage;
    public Image DamagedImage {
        get { return damagedImage; }
    }
    public Hardened(DynamicShape shape, Image blocksImage, Image damagedImage, BlockType type) 
        : base(shape, blocksImage, damagedImage, type) {
            this.damagedImage = damagedImage;
            health = 20;
    }
    public override void Damage() {
        health -= 10;
        if (health <= 0) {
            Destroy();
        }
    }

    public void SetDamagedImage(Image image) {
        BlocksImage = image;
    }
}