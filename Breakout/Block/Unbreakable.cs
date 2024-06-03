using System;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout.IBlock;
public class Unbreakable : Block {
    /// <summary>
    /// These blocks can't be broken unless they are
    /// the only blocks remaining
    /// </summary>
    bool CanBreak = false;
    public Unbreakable(DynamicShape shape, Image blocksImage, Image damagedImage, 
        BlockType type) 
            : base(shape, blocksImage, damagedImage, type) {
                this.blocksImage = blocksImage;
    }

    public override void Damage() {
        if (CanBreak) {
            health -= 10;
            if (health <= 0) {
                Destroy();
            }
        }
    }

    public void RemoveImmunity() {
        CanBreak = true;
    }

    public void ApplyImmunity() {
        CanBreak = false;
    }

    public bool GetCanBreak() {
        return CanBreak;
    }
    
}