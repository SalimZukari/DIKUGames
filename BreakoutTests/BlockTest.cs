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

public class BlockTest {

    private Block block;
    [OneTimeSetUp]
    public void Init() {
        DIKUArcade.GUI.Window.CreateOpenGLContext();
    }

    [SetUp]
    public void Setup() {
        block = new Block(new DynamicShape(new Vec2F(0.1f, 0.1f),
            new Vec2F(0.2f, 0.2f)), new Image(Path.Combine("..", "Assets", "Images", "red-block.png")),
            new Image(Path.Combine("..", "Assets", "Images", "red-block-damaged.png")), BlockType.Normal);
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
}