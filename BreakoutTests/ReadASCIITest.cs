using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using Breakout;

namespace BreakoutTests {
    public class InterpretDataTests {
        private string testFilePath = "/home/student/SU23Guest/DIKUGames/Assets/Levels/level1.txt";
    
        [SetUp]
        public void SetUp() {
        }

        [Test]
        public void TestOrganizingData() {
            var interpretData = new InterpretData(testFilePath);
            var organizedData = interpretData.GetOrganizedData();
            Assert.That(organizedData.ContainsKey("Map"), Is.True);
            Assert.That(organizedData.ContainsKey("Meta"), Is.True);
            Assert.That(organizedData.ContainsKey("Legend"), Is.True);
        }

        [Test]
        public void TestReadLegend() {
            var interpretData = new InterpretData(testFilePath);
            var legendOrganized = interpretData.GetLegendOrganized();
            Assert.That(legendOrganized.ContainsKey('%'), Is.True);
            Assert.That(legendOrganized['%'], Is.EqualTo("blue-block.png"));
            Assert.That(legendOrganized.ContainsKey('0'), Is.True);
            Assert.That(legendOrganized['0'], Is.EqualTo("grey-block.png"));
            Assert.That(legendOrganized.ContainsKey('1'), Is.True);
            Assert.That(legendOrganized['1'], Is.EqualTo("orange-block.png"));
            Assert.That(legendOrganized.ContainsKey('a'), Is.True);
            Assert.That(legendOrganized['a'], Is.EqualTo("purple-block.png"));
        }

        [Test]
        public void TestReadMap() {
            var interpretData = new InterpretData(testFilePath);
            var mapOrganized = interpretData.GetMapOrganized();
            Assert.That(mapOrganized.ContainsKey("Empty"), Is.True); 
            Assert.That(mapOrganized["purple-block.png"].Count, Is.GreaterThan(0));
        }
    

        [Test]
        public void TestReadMeta() {
            var interpretData = new InterpretData(testFilePath);
            var metaOrganized = interpretData.GetMetaOrganized();
            Assert.That(metaOrganized["Name"], Is.EqualTo(" LEVEL 1"));
            Assert.That(metaOrganized["Time"], Is.EqualTo(" 300"));
            Assert.That(metaOrganized["Hardened"], Is.EqualTo(" #"));
            Assert.That(metaOrganized["PowerUp"], Is.EqualTo(" 2"));
        }

        // Test file not found
        [Test]
        public void TestFileNotFound() {
            InterpretData missingFile = new InterpretData("invaildfile.txt");
            Assert.That(missingFile.GetLevelContents(), Is.Empty);
        }

        // Test Empty File Handling
        [Test]
        public void TestEmptyFile() {
            string emptyFilePath = "TestEmptyFile.txt";
            InterpretData emptyFileInterpretData = new InterpretData(emptyFilePath);
            Assert.AreEqual(0, emptyFileInterpretData.GetLevelContents().Length);
        }

        // Test Missing Sections
        [Test]
        public void TestMissingSections() {
            string filePathWithMissingSections = "TestMissingSections.txt";
            // Missing 'a' a) purple-block.png in the legend section
            InterpretData dataWithMissingSections = new InterpretData(filePathWithMissingSections);
            Assert.IsFalse(dataWithMissingSections.GetLegendOrganized().ContainsKey('a'));
        }

        // Test error Handling in ReadMeta
        [Test]
        public void TestReadMetaFail() {
            string filePath = "TestReadMetaFail.txt";
            // "Meta/",
            // "NameLevel 1"  // Missing colon separator
            InterpretData interpretData = new InterpretData(filePath);
            interpretData.OrganizingData();
            interpretData.ReadMeta();
            Assert.IsEmpty(interpretData.GetMetaOrganized());
        }

        // Test Legend Index Read Fail
        [Test]
        public void TestLegendIndexFail() {
            string filePath = "TestLegendIndexFail.txt";   
            InterpretData interpretData = new InterpretData(filePath);
            interpretData.OrganizingData();
            interpretData.ReadLegend();
            Assert.IsEmpty(interpretData.GetLegendOrganized());
        }
    }
}