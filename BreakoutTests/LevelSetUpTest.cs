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
                    "..", "..", "..", "..", "Assets", "Levels", "level1.txt"
                ));
        }

        [Test]
        public void TestGetBlocks() {
            // Arrange
            var blocks = levelSetUp.GetBlocks();
            int count = 0;
            
            // Act
            foreach(var block in blocks) {
                count++;
            }

            // Assert
            Assert.AreEqual(76, count);
        }

        [Test]
        public void TestSetUp() {
            // This should double the number of blocks in the container
            levelSetUp.SetUp();

            Assert.IsNotNull(levelSetUp.GetBlocks());
            Assert.AreEqual(152, levelSetUp.GetBlocks().CountEntities());
        }
    }
}
