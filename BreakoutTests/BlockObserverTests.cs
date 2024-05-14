using NUnit.Framework;
using Breakout;
using Breakout.IBlock;
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

namespace BreakoutTests;

public class BlockObserverTests {

    private Block block;
    private BlockObserver blockObserver;
    private Unbreakable unbreakable1;
    private Unbreakable unbreakable2;
    private Hardened hardened;
    private EntityContainer<Block> blockContainer;

    [OneTimeSetUp]
    public void Init() {
        DIKUArcade.GUI.Window.CreateOpenGLContext();
    }

    [SetUp]
    public void Setup() {
        blockObserver = new BlockObserver();
        blockContainer = new EntityContainer<Block>();
        block = new Block(new DynamicShape(new Vec2F(0.1f, 0.1f),
            new Vec2F(0.2f, 0.2f)), new Image(Path.Combine("..", "Assets", "Images", "red-block.png")),
            new Image(Path.Combine("..", "Assets", "Images", "red-block-damaged.png")), BlockType.Normal);
        unbreakable1 = new Unbreakable(new DynamicShape(new Vec2F(0.1f, 0.1f),
            new Vec2F(0.2f, 0.2f)), new Image(Path.Combine("..", "Assets", "Images", "brown-block.png")),
            new Image(Path.Combine("..", "Assets", "Images", "brown-block-damaged.png")), BlockType.Normal);
        unbreakable2 = new Unbreakable(new DynamicShape(new Vec2F(0.1f, 0.1f),
            new Vec2F(0.2f, 0.2f)), new Image(Path.Combine("..", "Assets", "Images", "brown-block.png")),
            new Image(Path.Combine("..", "Assets", "Images", "brown-block-damaged.png")), BlockType.Normal);
        hardened = new Hardened(new DynamicShape(new Vec2F(0.1f, 0.1f),
            new Vec2F(0.2f, 0.2f)), new Image(Path.Combine("..", "Assets", "Images", "orange-block.png")),
            new Image(Path.Combine("..", "Assets", "Images", "orange-block-damaged.png")), BlockType.Normal);
        blockContainer.AddEntity(block);
        blockContainer.AddEntity(unbreakable1);
        blockContainer.AddEntity(unbreakable2);
        blockContainer.AddEntity(hardened);
    }
    
    [Test]
    public void TestUnbreakableImmunity() {
        blockObserver.CheckBlocks(blockContainer);
        Assert.AreEqual(false, unbreakable1.GetCanBreak());
        Assert.AreEqual(false, unbreakable2.GetCanBreak());
    }

    [Test]
    public void TestHardenedHealth() {
        blockObserver.CheckBlocks(blockContainer);
        Assert.AreEqual(20, hardened.Health);
    }

    [Test]
    public void TestHardenedDamagedHealth() {
        hardened.Damage();
        blockObserver.CheckBlocks(blockContainer);
        Assert.AreEqual(10, hardened.Health);
    }

    [Test]
    public void TestUnbreakRemoveImmunity() {
        blockContainer.ClearContainer();
        blockContainer.AddEntity(unbreakable1);
        blockContainer.AddEntity(unbreakable2);
        blockObserver.CheckBlocks(blockContainer);

        Assert.AreEqual(true, unbreakable1.GetCanBreak());
        Assert.AreEqual(true, unbreakable2.GetCanBreak());
    }
}