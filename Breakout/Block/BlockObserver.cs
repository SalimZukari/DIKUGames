using System;
using DIKUArcade.Entities;

namespace Breakout.IBlock;
public class BlockObserver {
    public BlockObserver() {
    }

    public void CheckBlocks(EntityContainer<Block> blocks) {
        int length = blocks.CountEntities();
        int numbOfUnbreaks = 0;
        EntityContainer<Unbreakable> unbreakables = new EntityContainer<Unbreakable>();

        foreach(Block block in blocks) {
            if (block is Unbreakable) {
                unbreakables.AddEntity((Unbreakable)block);
            }
        }


        if (unbreakables.CountEntities() == length) {
            foreach(Unbreakable unbreakable in unbreakables) {
                unbreakable.RemoveImmunity();
            }
        }
    }
}