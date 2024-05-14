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
using Breakout.BreakoutStates;

namespace BreakoutTests;

public class BlockTest {

    private Block block;
    private Hardened hardened;
    private Unbreakable unbreakable;
    private EntityContainer<Block> blocks;
    [OneTimeSetUp]
    public void Init() {
        DIKUArcade.GUI.Window.CreateOpenGLContext();

    }

    [SetUp]
    public void Setup() {
        block = new Block(new DynamicShape(new Vec2F(0.1f, 0.1f),
            new Vec2F(0.2f, 0.2f)), new Image(Path.Combine("..", "Assets", "Images", "red-block.png")),
            new Image(Path.Combine("..", "Assets", "Images", "red-block-damaged.png")), BlockType.Normal);
        hardened = new Hardened(new DynamicShape(new Vec2F(0.1f, 0.1f),
            new Vec2F(0.2f, 0.2f)), new Image(Path.Combine("..", "Assets", "Images", "red-block.png")),
            new Image(Path.Combine("..", "Assets", "Images", "red-block-damaged.png")), BlockType.Hardened);
        unbreakable = new Unbreakable(new DynamicShape(new Vec2F(0.1f, 0.1f),
            new Vec2F(0.2f, 0.2f)), new Image(Path.Combine("..", "Assets", "Images", "red-block.png")),
            new Image(Path.Combine("..", "Assets", "Images", "red-block-damaged.png")), BlockType.Unbreakable);
        blocks = new EntityContainer<Block>();
        blocks.AddEntity(block);
        blocks.AddEntity(hardened);
        blocks.AddEntity(unbreakable);
    }
    [Test]
    public void TestBlockHealth() {
        Assert.AreEqual(10, block.Health);
    }
    [Test]
    public void TestBlockDamage() {
        block.Damage();
        Assert.AreEqual(0, block.Health);
    }
    [Test]
    public void TestBlockDestroy() {
        block.Damage();
        block.Damage();
        block.Damage();
        Assert.AreEqual(true, block.IsDeleted());
    }  
    [Test]
    public void TestBlockType() {
        Assert.AreEqual(BlockType.Normal, block.GetType());
        Assert.AreEqual(BlockType.Hardened, hardened.GetType());
        Assert.AreEqual(BlockType.Unbreakable, unbreakable.GetType());
        Assert.AreNotEqual(block.GetType(), hardened.GetType());
    }
    [Test]
    public void unbreakableBlockDamage() {
        unbreakable.Damage();
        Assert.AreEqual(10, unbreakable.Health);
    }
    [Test]
    public void unbreakableBlockDamage1() {
        unbreakable.RemoveImmunity();
        unbreakable.Damage();
        Assert.AreEqual(0, unbreakable.Health);
        Assert.AreEqual(true, unbreakable.IsDeleted());
    }
    [Test]
    public void hardenedBlockDamage() {
        hardened.Damage();
        Assert.AreEqual(10, hardened.Health);
    }

    [Test]
    public void hardenedBlockDamage1() {
        hardened.Damage();
        hardened.Damage();
        Assert.AreEqual(0, hardened.Health);
        Assert.AreEqual(true, hardened.IsDeleted());
    }

    [Test]
    public void hardenedImage1() {
        var shape = new DynamicShape(new Vec2F(0.1f, 0.1f),
            new Vec2F(0.2f, 0.2f));
        var img = new Image(Path.Combine("..", "Assets", "Images", "red-block.png"));
        var img1 = new Image(Path.Combine("..", "Assets", "Images", "red-block-damaged.png"));
        Hardened hardened1 = new Hardened(shape, img, img1, BlockType.Hardened);
        hardened1.Damage();
        IBaseImage image = hardened1.GetDamagedImage();
        IBaseImage image1 = img1;
        Assert.AreEqual(image, image1);
    }

    [Test]
    public void hardenedImage() {
        var shape = new DynamicShape(new Vec2F(0.1f, 0.1f),
            new Vec2F(0.2f, 0.2f));
        var img = new Image(Path.Combine("..", "Assets", "Images", "red-block.png"));
        var img1 = new Image(Path.Combine("..", "Assets", "Images", "red-block-damaged.png"));
        Hardened hardened1 = new Hardened(shape, img, img1, BlockType.Hardened);
        IBaseImage image = img;
        IBaseImage image1 = hardened1.GetImage();
        Assert.AreEqual(image, image1);
    }

}