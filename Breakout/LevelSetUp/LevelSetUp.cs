using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics; 
using System;
using System.Linq;
using DIKUArcade.Math;
using Breakout.IBlock;

namespace Breakout {
    public class LevelSetUp {
        private InterpretData layout;
        private EntityContainer<Block> blocks;
        private IDictionary<string, string> coolBlockKey = new Dictionary<string, string>();

        public LevelSetUp(string file) {
            layout = new InterpretData(file);
            blocks = new EntityContainer<Block>();

            SetUp();
        }

        public void SetUp() {
            ColorOfSpecialBlocks();
            var colors = layout.GetLegendOrganized().Values;
            var positions = layout.GetMapOrganized();
            var blockTypes = layout.GetMetaOrganized();

            foreach (var colorEntry in colors) {
                if (positions.TryGetValue(colorEntry, out List<(float, float)>? positionsListN)
                && positionsListN != null
                && coolBlockKey[colorEntry] == "Normal") {
                    foreach ((float x, float y) in positionsListN) {
                        blocks.AddEntity(new Block(
                            new DynamicShape(new Vec2F(x, y), new Vec2F(0.09f, 0.05f)),
                            new Image(Path.Combine("..", "Assets", "Images", colorEntry))
                        ));
                    }
                } else if (positions.TryGetValue(colorEntry, out List<(float, float)>? positionsListU)
                && positionsListU != null
                && coolBlockKey[colorEntry] == "Unbreakable") {
                    foreach ((float x, float y) in positionsListU) {
                        blocks.AddEntity(new Unbreakable(
                            new DynamicShape(new Vec2F(x, y), new Vec2F(0.09f, 0.05f)),
                            new Image(Path.Combine("..", "Assets", "Images", colorEntry))
                        ));
                    }
                } else if (positions.TryGetValue(colorEntry, out List<(float, float)>? positionsList)
                && positionsList == null) {
                    Console.WriteLine("A positionList is null");
                } else {
                    Console.WriteLine(
                        "colorEntry '{0}' does not exist as a key in coolBlockKey",
                        colorEntry
                    );
                }
            }
        }

        public BlockType MetaDataToBlockType(string type) {
            BlockType blockType;
            if (Enum.TryParse(type, out blockType)) {
                switch (blockType) {
                    case BlockType.Unbreakable:
                        return BlockType.Unbreakable;
                }
            } return BlockType.Normal;
        }

        public void ColorOfSpecialBlocks() {
            var legend = layout.GetLegendOrganized();
            var meta = layout.GetMetaOrganized();
            foreach (KeyValuePair<string, string> entry in meta) {
                if (entry.Value.Length == 2 && legend.ContainsKey(entry.Value[1])) {
                    coolBlockKey.TryAdd(legend[entry.Value[1]], entry.Key);
                }
            }
            foreach (KeyValuePair<char, string> entry2 in legend) {
                coolBlockKey.TryAdd(entry2.Value, "Normal");
            }
        }

        public EntityContainer<Block> GetBlocks() {
            return blocks;
        }
    }
}