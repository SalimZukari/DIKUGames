using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout.IBlock;
public class Hardened : Block {

    protected new Image damagedImage;
    public Image DamagedImage {
        get { return damagedImage; }
    }
    public Hardened(DynamicShape shape, Image blocksImage, Image damagedImage, BlockType type) 
        : base(shape, blocksImage, damagedImage, type) {
            this.damagedImage = damagedImage;
            this.Image = blocksImage;
            health = 20;
    }
    public override void Damage() {
        health -= 10;
        if (health <= 0) {
            Destroy();
        }
    }

    public void SetDamagedImage(Image image) {
        this.Image = image;
    }
}