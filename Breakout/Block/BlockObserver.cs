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
            if (hardened.Health == 10) {
                Image blockimage = hardened.blocksImage;
                var pic = blockimage.GetTexture().ToString();
                pic = pic.Replace(".png", "");
                hardened.blocksImage = new Image(Path.Combine("Assets", "Images", pic + "-damged.png"));


            }
        }
        
        if (unbreakables.CountEntities() == length) {
            foreach(Unbreakable unbreakable in unbreakables) {
                unbreakable.RemoveImmunity();
            }
        }
    }
}