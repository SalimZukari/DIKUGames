using System;
using Breakout.IBlock;
using DIKUArcade.Entities;

namespace Breakout.IBlock;

public class BlockObserver {
    public BlockObserver() {
    }
    public void CheckBlocks(EntityContainer<Block> blocks) {
        int length = blocks.CountEntities();
        EntityContainer<Unbreakable> unbreakables = new EntityContainer<Unbreakable>();
        EntityContainer<Hardened> hardeneds = new EntityContainer<Hardened>();
        EntityContainer<PowerUpBlock> powerUps = new EntityContainer<PowerUpBlock>();


        foreach(Block block in blocks) {
            if (block is Hardened) {
                hardeneds.AddEntity((Hardened)block);
            } else if (block is Unbreakable) {
                unbreakables.AddEntity((Unbreakable)block);
            } else if (block is PowerUpBlock) {
                powerUps.AddEntity((PowerUpBlock)block);
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