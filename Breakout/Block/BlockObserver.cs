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
        EntityContainer<Unbreakable> unbreakables = new EntityContainer<Unbreakable>();
        EntityContainer<Hardened> hardeneds = new EntityContainer<Hardened>();


        foreach(Block block in blocks) {
            if (block is Hardened) {
                hardeneds.AddEntity((Hardened)block);
            } else if (block is Unbreakable) {
                unbreakables.AddEntity((Unbreakable)block);
            }
        }

        foreach (Hardened hardened in hardeneds) {
            if (hardened.Health == 10 && hardened.GetImage() == hardened.BlocksImage) {
                hardened.SetDamagedImage(hardened.DamagedImage);
            }
        }
        
        if (unbreakables.CountEntities() == length) {
            foreach(Unbreakable unbreakable in unbreakables) {
                unbreakable.RemoveImmunity();
            }
        }
    }
}