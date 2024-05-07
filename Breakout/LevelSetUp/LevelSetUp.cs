using System;
using DIKUArcade.Entities;
using DIKUArcade.Graphics; 
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
                && positionsListN != null) {
                    foreach ((float x, float y) in positionsListN) {
                        var imagePath = Path.Combine("..", "Assets", "Images", colorEntry);
                        var damagedImagePath = Path.Combine("..", "Assets", "Images", colorEntry.Replace(".png", "") + "-damaged.png");
                        blocks.AddEntity(new Block(
                            new DynamicShape(new Vec2F(x, y), new Vec2F(0.09f, 0.05f)),
                            new Image(Path.Combine("..", "Assets", "Images", colorEntry)),
                            new Image(damagedImagePath),
                            StringToBlockType(coolBlockKey[colorEntry])
                        ));
                    }
                } else if (positions.TryGetValue(colorEntry,
                    out List<(float, float)>? positionsList)
                    && positionsList == null) {
                        Console.WriteLine("A positionList is null");
                } else {
                    Console.WriteLine(
                        "colorEntry '{0}' does not exist as a key in coolBlockKey",
                        colorEntry
                    );
                }
            }

            EntityContainer<Block> blocksToDestroy = new EntityContainer<Block>();
            foreach (Block block in blocks) {
                if (block.GetType() != BlockType.Normal) {
                    blocksToDestroy.AddEntity(block);
                }
            }

            foreach (Block block in blocksToDestroy) {
                blocks.AddEntity(CreateNewBlock(
                                            block.GetPosition().X,
                                            block.GetPosition().Y,
                                            block.GetImage(),
                                            block.GetDamagedImage(),
                                            block.GetType()

                    ));
                block.Destroy();
            }
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

        public BlockType StringToBlockType(string type) {
            BlockType blockType = new BlockType();
            if (BlockType.TryParse(type, out blockType)) {
                switch (blockType) {
                    case BlockType.Normal:
                        return BlockType.Normal;
                    case BlockType.Unbreakable:
                        return BlockType.Unbreakable;
                    case BlockType.Hardened:
                        return BlockType.Hardened;
                }
            }

            return BlockType.Normal;
        }

        public Block CreateNewBlock(float x, float y, Image image, Image damagedImage, BlockType type) {
            switch (type) {
                case BlockType.Unbreakable:
                    return new Unbreakable(
                        new DynamicShape(new Vec2F(x, y), new Vec2F(0.09f, 0.06f)),
                        image,
                        damagedImage,
                        type
                    );
                case BlockType.Hardened:
                    return new Hardened(
                        new DynamicShape(new Vec2F(x, y), new Vec2F(0.09f, 0.06f)),
                        image,
                        damagedImage,
                        type
                    );
                default:
                    return new Block(
                        new DynamicShape(new Vec2F(x, y), new Vec2F(0.09f, 0.06f)),
                        image,
                        damagedImage,
                        type
                    );
            }
        }

        public EntityContainer<Block> GetBlocks() {
            return blocks;
        }
    }
}