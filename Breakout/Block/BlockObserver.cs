using System;
using Breakout.IBlock;
using DIKUArcade.Entities;

namespace Breakout.IBlock;

public class BlockObserver {
    /// <summary>
    /// Used for updating special block qualities
    /// </summary>
    public BlockObserver() {
    }
    public void CheckBlocks(EntityContainer<Block> blocks) {
        int length = blocks.CountEntities();
        EntityContainer<Unbreakable> unbreakables = new EntityContainer<Unbreakable>();
        EntityContainer<Hardened> hardeneds = new EntityContainer<Hardened>();
        EntityContainer<PowerUpBlock> powerUps = new EntityContainer<PowerUpBlock>();

        /// <summary>
        /// So that the right special block attribute is working.
        /// </summary>
        foreach(Block block in blocks) {
            if (block is Hardened) {
                hardeneds.AddEntity((Hardened)block);
            } else if (block is Unbreakable) {
                unbreakables.AddEntity((Unbreakable)block);
            } else if (block is PowerUpBlock) {
                powerUps.AddEntity((PowerUpBlock)block);
            }
        }

        /// <summary>
        /// If a hardened block is hit but still has health,
        /// then the block simply changes image.
        /// </summary>
        foreach (Hardened hardened in hardeneds) {
            if (hardened.Health == 10 && hardened.GetImage() == hardened.BlocksImage) {
                hardened.SetDamagedImage(hardened.DamagedImage);
            }
        }
        
        /// <summary>
        /// If only unbreakable blocks remain, they become breakable.
        /// </summary>
        if (unbreakables.CountEntities() == length) {
            foreach(Unbreakable unbreakable in unbreakables) {
                unbreakable.RemoveImmunity();
            }
        }
    }
}