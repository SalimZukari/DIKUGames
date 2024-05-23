using NUnit.Framework;
using Breakout;
using Breakout.IBlock;
using Breakout.PowerUps;
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
    string testFilePath = Path.Combine(
            "..", "..", "..", "..", "Assets", "Levels", "level1.txt"
        );
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
        GameRunning gameRunning = new GameRunning(testFilePath);
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

    [Test]
    public void powerUpBlockSpawn() {
        var shape = new DynamicShape(new Vec2F(0.1f, 0.1f),
            new Vec2F(0.2f, 0.2f));
        var img = new Image(Path.Combine("..", "Assets", "Images", "red-block.png"));
        var img1 = new Image(Path.Combine("..", "Assets", "Images", "red-block-damaged.png"));
        PowerUpBlock powerUp = new PowerUpBlock(shape, img, img1, BlockType.PowerUp);
        powerUp.SpawnPowerUp();
        Assert.AreEqual(GameRunning.Effects.CountEntities(), 1);
    }

    [Test]
    public void DoubleSizeToObjectTest() {
        var shape = new DynamicShape(new Vec2F(0.1f, 0.1f),
            new Vec2F(0.2f, 0.2f));
        var img = new Image(Path.Combine("..", "Assets", "Images", "red-block.png"));
        var img1 = new Image(Path.Combine("..", "Assets", "Images", "red-block-damaged.png"));
        PowerUpBlock powerUp = new PowerUpBlock(shape, img, img1, BlockType.PowerUp);
        Effect doubleSize = powerUp.PowerUpTypeToObject(EffectType.DoubleSize, img);
        Effect expectedDS = new DoubleSize(
            new DynamicShape(powerUp.Shape.Position, powerUp.Shape.Extent), img
        );
        Assert.AreEqual(doubleSize.GetType(), expectedDS.GetType());
    }

    [Test]
    public void ExtraLifeToObjectTest() {
        var shape = new DynamicShape(new Vec2F(0.1f, 0.1f),
            new Vec2F(0.2f, 0.2f));
        var img = new Image(Path.Combine("..", "Assets", "Images", "red-block.png"));
        var img1 = new Image(Path.Combine("..", "Assets", "Images", "red-block-damaged.png"));
        PowerUpBlock powerUp = new PowerUpBlock(shape, img, img1, BlockType.PowerUp);
        Effect extraLife = powerUp.PowerUpTypeToObject(EffectType.ExtraLife, img);
        Effect expectedEL = new ExtraLife(
            new DynamicShape(powerUp.Shape.Position, powerUp.Shape.Extent), img
        );
        Assert.AreEqual(extraLife.GetType(), expectedEL.GetType());
    }

    [Test]
    public void PlayerSpeedToObjectTest() {
        var shape = new DynamicShape(new Vec2F(0.1f, 0.1f),
            new Vec2F(0.2f, 0.2f));
        var img = new Image(Path.Combine("..", "Assets", "Images", "red-block.png"));
        var img1 = new Image(Path.Combine("..", "Assets", "Images", "red-block-damaged.png"));
        PowerUpBlock powerUp = new PowerUpBlock(shape, img, img1, BlockType.PowerUp);
        Effect playerSpeed = powerUp.PowerUpTypeToObject(EffectType.PlayerSpeed, img);
        Effect expectedPS = new PlayerSpeed(
            new DynamicShape(powerUp.Shape.Position, powerUp.Shape.Extent), img
        );
        Assert.AreEqual(playerSpeed.GetType(), expectedPS.GetType());
    }

    [Test]
    public void MoreTimeToObjectTest() {
        var shape = new DynamicShape(new Vec2F(0.1f, 0.1f),
            new Vec2F(0.2f, 0.2f));
        var img = new Image(Path.Combine("..", "Assets", "Images", "red-block.png"));
        var img1 = new Image(Path.Combine("..", "Assets", "Images", "red-block-damaged.png"));
        PowerUpBlock powerUp = new PowerUpBlock(shape, img, img1, BlockType.PowerUp);
        Effect moreTime = powerUp.PowerUpTypeToObject(EffectType.MoreTime, img);
        Effect expectedMT = new MoreTime(
            new DynamicShape(powerUp.Shape.Position, powerUp.Shape.Extent), img
        );
        Assert.AreEqual(moreTime.GetType(), expectedMT.GetType());
    }

    [Test]
    public void WideToObjectTest() {
        var shape = new DynamicShape(new Vec2F(0.1f, 0.1f),
            new Vec2F(0.2f, 0.2f));
        var img = new Image(Path.Combine("..", "Assets", "Images", "red-block.png"));
        var img1 = new Image(Path.Combine("..", "Assets", "Images", "red-block-damaged.png"));
        PowerUpBlock powerUp = new PowerUpBlock(shape, img, img1, BlockType.PowerUp);
        Effect wide = powerUp.PowerUpTypeToObject(EffectType.Wide, img);
        Effect expectedW = new Wide(
            new DynamicShape(powerUp.Shape.Position, powerUp.Shape.Extent), img
        );
        Assert.AreEqual(wide.GetType(), expectedW.GetType());
    }

    [Test]
    public void hazardBlockSpawn() {
        var shape = new DynamicShape(new Vec2F(0.1f, 0.1f),
            new Vec2F(0.2f, 0.2f));
        var img = new Image(Path.Combine("..", "Assets", "Images", "red-block.png"));
        var img1 = new Image(Path.Combine("..", "Assets", "Images", "red-block-damaged.png"));
        HazardBlock hazard = new HazardBlock(shape, img, img1, BlockType.PowerUp);
        hazard.SpawnPowerUp();
        Assert.AreEqual(GameRunning.Effects.CountEntities(), 1);
    }

    [Test]
    public void LoseLifeToObjectTest() {
        var shape = new DynamicShape(new Vec2F(0.1f, 0.1f),
            new Vec2F(0.2f, 0.2f));
        var img = new Image(Path.Combine("..", "Assets", "Images", "red-block.png"));
        var img1 = new Image(Path.Combine("..", "Assets", "Images", "red-block-damaged.png"));
        HazardBlock hazard = new HazardBlock(shape, img, img1, BlockType.PowerUp);
        Effect loseLife = hazard.HazardTypeToObject(EffectType.LoseLife, img);
        Effect expectedLL = new LoseLife(
            new DynamicShape(hazard.Shape.Position, hazard.Shape.Extent), img
        );
        Assert.AreEqual(loseLife.GetType(), expectedLL.GetType());
    }

    [Test]
    public void SlownessToObjectTest() {
        var shape = new DynamicShape(new Vec2F(0.1f, 0.1f),
            new Vec2F(0.2f, 0.2f));
        var img = new Image(Path.Combine("..", "Assets", "Images", "red-block.png"));
        var img1 = new Image(Path.Combine("..", "Assets", "Images", "red-block-damaged.png"));
        HazardBlock hazard = new HazardBlock(shape, img, img1, BlockType.PowerUp);
        Effect slow = hazard.HazardTypeToObject(EffectType.Slowness, img);
        Effect expectedS = new Slowness(
            new DynamicShape(hazard.Shape.Position, hazard.Shape.Extent), img
        );
        Assert.AreEqual(slow.GetType(), expectedS.GetType());
    }

}