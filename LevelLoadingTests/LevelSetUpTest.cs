using NUnit.Framework;
using System.IO;
using Breakout;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Breakout.IBlock;
using System;

namespace BreakoutTests {
    public class LevelSetUpTest {
        private LevelSetUp levelSetUp;

        [SetUp]
        public void Setup() {
            levelSetUp = new LevelSetUp(Path.Combine(
                    "..", "..", "..", "..", "Assets", "Levels", "TestLevel.txt"
                ));
            DIKUArcade.GUI.Window.CreateOpenGLContext();
        }

        [Test]
        public void TestGetBlocks() {
            // Arrange
            var blocks = levelSetUp.GetBlocks();
            int count = 0;
            
            // Act
            foreach(var block in blocks) {
                count++;
                Console.WriteLine("{0}", block.GetType());
            }

            // Assert
            Assert.AreEqual(3, count);
        }

        [Test]
        public void TestSetUp() {
            Assert.IsNotNull(levelSetUp.GetBlocks());
            Assert.AreEqual(3, levelSetUp.GetBlocks().CountEntities());
        }

        [Test]
        public void TestNormalBlockInContainer() {
            var blockContainer = levelSetUp.GetBlocks();
            int numberNorm = 0;
            foreach (Block block in blockContainer) {
                if (block.GetType() == BlockType.Normal) {
                    numberNorm += 1;
                }
            }

            Assert.AreEqual(1, numberNorm);
        }

        [Test]
        public void TestUnbreakableBlockInContainer() {
            var blockContainer = levelSetUp.GetBlocks();
            int numberNorm = 0;
            foreach (Block block in blockContainer) {
                if (block.GetType() == BlockType.Unbreakable) {
                    numberNorm += 1;
                }
            }

            Assert.AreEqual(1, numberNorm);
        }

        [Test]
        public void TestHardenedBlockInContainer() {
            var blockContainer = levelSetUp.GetBlocks();
            int numberNorm = 0;
            foreach (Block block in blockContainer) {
                if (block.GetType() == BlockType.Hardened) {
                    numberNorm += 1;
                }
            }

            Assert.AreEqual(1, numberNorm);
        }

        [Test]
        public void TestMakeNormalBlock() {
            var blockContainer = new EntityContainer<Block>();
            var damagedImagePath = Path.Combine("..", "Assets", 
                            "Images", "red-block-damaged.png");
            var imagePath = Path.Combine("..", "Assets", "Images", "red-block.png");
            var block = levelSetUp.CreateNewBlock(
                0.5f,
                0.5f,
                new Image(imagePath),
                new Image(damagedImagePath),
                BlockType.Normal
            );
            blockContainer.AddEntity(block);

            Assert.IsNotNull(blockContainer);
            Assert.AreEqual(block.GetType(), BlockType.Normal);
        }

        [Test]
        public void TestMakeUnbreakableBlock() {
            var blockContainer = new EntityContainer<Block>();
            var damagedImagePath = Path.Combine("..", "Assets", 
                            "Images", "red-block-damaged.png");
            var imagePath = Path.Combine("..", "Assets", "Images", "red-block.png");
            var block = levelSetUp.CreateNewBlock(
                0.5f,
                0.5f,
                new Image(imagePath),
                new Image(damagedImagePath),
                BlockType.Unbreakable
            );
            blockContainer.AddEntity(block);

            Assert.IsNotNull(blockContainer);
            Assert.AreEqual(true, (block is Unbreakable));
        }

        [Test]
        public void TestMakeHardenedBlock() {
            var blockContainer = new EntityContainer<Block>();
            var damagedImagePath = Path.Combine("..", "Assets", 
                            "Images", "red-block-damaged.png");
            var imagePath = Path.Combine("..", "Assets", "Images", "red-block.png");
            var block = levelSetUp.CreateNewBlock(
                0.5f,
                0.5f,
                new Image(imagePath),
                new Image(damagedImagePath),
                BlockType.Hardened
            );
            blockContainer.AddEntity(block);

            Assert.IsNotNull(blockContainer);
            Assert.AreEqual(true, (block is Hardened));
        }

        [Test]
        public void TestStringToType() {
            var normal = levelSetUp.StringToBlockType("Normal");
            var unbreakable = levelSetUp.StringToBlockType("Unbreakable");
            var hardened = levelSetUp.StringToBlockType("Hardened");

            Assert.AreEqual(BlockType.Normal, normal);
            Assert.AreEqual(BlockType.Unbreakable, unbreakable);
            Assert.AreEqual(BlockType.Hardened, hardened);
        }

        [Test]
        public void TestCoolKeyLength() {
            int lengthOfCoolKey = levelSetUp.GetCoolKey().Count;

            Assert.AreEqual(3, lengthOfCoolKey);
        }

        [Test]
        public void TestColorNormal() {
            string normalColor = levelSetUp.GetCoolKey()["orange-block.png"];

            Assert.AreEqual("Normal", normalColor);
        }

        [Test]
        public void TestColorUnbreakable() {
            string unbColor = levelSetUp.GetCoolKey()["brown-block.png"];

            Assert.AreEqual("Unbreakable", unbColor);
        }

        [Test]
        public void TestColorHardened() {
            string hardColor = levelSetUp.GetCoolKey()["red-block.png"];

            Assert.AreEqual("Hardened", hardColor);
        }
    }
}
