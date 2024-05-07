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
using Breakout.IBlock;

namespace Breakout.IBlock;

public class BlockObserver {
    public BlockObserver() {
    }
    public void CheckBlocks(EntityContainer<Block> blocks) {
        int length = blocks.CountEntities();
        int numbOfUnbreaks = 0;
        EntityContainer<Unbreakable> unbreakables = new EntityContainer<Unbreakable>();
        int numbOfHardened = 0;
        EntityContainer<Hardened> hardeneds = new EntityContainer<Hardened>();


        foreach(Block block in blocks) {
            if (block is Hardened) {
                hardeneds.AddEntity((Hardened)block);
            } else if (block is Unbreakable) {
                unbreakables.AddEntity((Unbreakable)block);
            }
        }

        foreach (Hardened hardened in hardeneds) {
            if (hardened.Health == 10 && hardened.GetImage() == hardened.blocksImage) {
                hardened.Image = hardened.damagedImage;
            }
        }
        
        if (unbreakables.CountEntities() == length) {
            foreach(Unbreakable unbreakable in unbreakables) {
                unbreakable.RemoveImmunity();
            }
        }
    }
}