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

public class BlockObserver {
    public BlockObserver() {
    }

    public void CheckBlocks(EntityContainer<Block> blocks) {
        int length = blocks.CountEntities();
        int numbOfUnbreaks = 0;

        blocks.Iterate(block => {
            if (block.GetType() == BlockType.Unbreakable) {
                Unbreakable unbreakable = (Unbreakable)block;

                block.Destroy();
                blocks.AddEntity(unbreakable);

                numbOfUnbreaks += 1;
            }
        });
        
        if (numbOfUnbreaks == length) {
            blocks.Iterate(block => {
                if (block is Unbreakable) {
                    Unbreakable unbreakable = (Unbreakable)block;
                    unbreakable.RemoveImmunity();
                }
            });
        } else {
            blocks.Iterate(block => {
                if (block is Unbreakable) {
                    Unbreakable unbreakable = (Unbreakable)block;
                    unbreakable.ApplyImmunity();
                }
            });
        }

    }
}