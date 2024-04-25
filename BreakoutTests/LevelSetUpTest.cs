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
            levelSetUp = new LevelSetUp();
            
        }

        [Test]
        public void TestSetUp() {

            Assert.IsNotNull(levelSetUp.GetBlocks());
            Assert.AreEqual(0, levelSetUp.GetBlocks().CountEntities());
        }

        [Test]
        public void TestGetBlocks() {
            // Arrange

            // Act
            levelSetUp.SetUp();
            var blocks = levelSetUp.GetBlocks();
            int count = 0;

            // Assert
            foreach(var block in blocks) {
                count++;
            }
            Assert.AreEqual(0, count);
            Assert.AreEqual(0, blocks);

            
        }
        [Test]
        public void TestGetBlock1() {
            levelSetUp.SetUp();


            Assert.AreEqual(0, levelSetUp.GetBlocks().CountEntities());
        }
    }
}
