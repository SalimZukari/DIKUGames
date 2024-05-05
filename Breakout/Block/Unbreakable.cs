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
public class Unbreakable : Block {
    bool CanBreak = false;
    public Unbreakable(DynamicShape shape, IBaseImage blocksImage, BlockType type) 
        : base(shape, blocksImage, type) {
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
        bool CanBreak = true;
    }

    public void ApplyImmunity() {
        bool CanBreak = false;
    }

    public bool GetCanBreak() {
        return CanBreak;
    }
}