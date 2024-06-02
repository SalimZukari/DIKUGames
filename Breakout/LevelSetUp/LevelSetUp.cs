using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics; 
using DIKUArcade.Math;
using Breakout.IBlock;

namespace Breakout {
    public class LevelSetUp {
        private InterpretData layout;
        private EntityContainer<Block> blocks;
        public string CurrentLevelFile { get; private set; }
        private IDictionary<string, string> coolBlockKey = new Dictionary<string, string>();
        private IDictionary<BlockType, Func<float, float, Image, Image, Block>> blockCreators;

        public InterpretData Layout {
            get { return layout; }
        }

        public LevelSetUp(string file) {
            CurrentLevelFile = file;
            layout = new InterpretData(file);
            blocks = new EntityContainer<Block>();
            InitializeBlockCreators();
            SetUp();
        }

        private void InitializeBlockCreators() {
            blockCreators = new Dictionary<BlockType, Func<float, float, Image, Image, Block>> {
                { BlockType.Unbreakable, (x, y, img, dmgImg) => 
                    new Unbreakable(new DynamicShape(new Vec2F(x, y), new Vec2F(0.08f, 0.04f)), img, dmgImg, BlockType.Unbreakable)}, 
                { BlockType.Hardened, (x, y, img, dmgImg) => 
                    new Hardened(new DynamicShape(new Vec2F(x, y), new Vec2F(0.08f, 0.04f)), img, dmgImg, BlockType.Hardened)},
                { BlockType.PowerUp, (x, y, img, dmgImg) => 
                    new PowerUpBlock( new DynamicShape(new Vec2F(x, y), new Vec2F(0.08f, 0.04f)), img, dmgImg, BlockType.PowerUp)}, 
                { BlockType.Hazard, (x, y, img, dmgImg) => 
                    new HazardBlock(new DynamicShape(new Vec2F(x, y), new Vec2F(0.08f, 0.04f)), img, dmgImg, BlockType.Hazard)},
                { BlockType.Normal, (x, y, img, dmgImg) => 
                    new Block(new DynamicShape(new Vec2F(x, y), new Vec2F(0.08f, 0.04f)), img, dmgImg, BlockType.Normal)}
            };
        }

        public void SetUp() {
            ColorOfSpecialBlocks();
            var colors = layout.GetLegendOrganized().Values;
            var positions = layout.GetMapOrganized();
            var blockTypes = layout.GetMetaOrganized();
            EntityContainer<Block> firstContainer = new EntityContainer<Block>();

            foreach (var colorEntry in colors) {
                if (positions.TryGetValue(colorEntry, out List<(float, float)>? positionsListN)
                && positionsListN != null) {
                    foreach ((float x, float y) in positionsListN) {
                        var imagePath = Path.Combine("..", "Assets", "Images", colorEntry);
                        var damagedImagePath = Path.Combine("..", "Assets", "Images", 
                            colorEntry.Replace(".png", "") + "-damaged.png"
                        );
                        if (coolBlockKey.TryGetValue(colorEntry, out string blockTypeString) 
                        && BlockType.TryParse(blockTypeString, out BlockType blockType)) {
                            firstContainer.AddEntity(CreateNewBlock(x, y, new Image(imagePath), new Image(damagedImagePath), blockType));
                        }
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

            int i = 0;
            foreach (Block block in firstContainer) {
                i++;
                if (block.GetType() != BlockType.Normal) {
                    block.DeleteEntity();
                } else if (block.GetType() == BlockType.Normal 
                    && i == 10) {
                        block.SwitchType(BlockType.Hazard);
                        block.DeleteEntity();
                        i = 0;
                } else if (block.GetType() == BlockType.Normal
                    && i != 10) {
                        blocks.AddEntity(block);
                }
            }

            foreach (Block block in firstContainer) {
                if (block.IsDeleted()) {
                    blocks.AddEntity(CreateNewBlock(
                                            block.GetPosition().X,
                                            block.GetPosition().Y,
                                            block.GetImage(),
                                            block.GetDamagedImage(),
                                            block.GetType()

                    ));
                }
            }
        }

        public void LoadLevel(string file) {
            CurrentLevelFile = file;
            layout = new InterpretData(file);
            blocks.ClearContainer();
            SetUp();
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

        public Block CreateNewBlock(float x, float y, Image image, Image damagedImage, BlockType type) {
            if (blockCreators.TryGetValue(type, out var creator)) {
                return creator(x, y, image, damagedImage);
            } else {
                return new Block(
                    new DynamicShape(new Vec2F(x, y), new Vec2F(0.08f, 0.04f)),
                    image,
                    damagedImage,
                    type
                );
            }
        }


        public int GetLevelNumber() {
            string currentLevelFile = CurrentLevelFile;
            string standardLevelName = "level";
            string fileExtension = ".txt";
            
            int currentLevelNumber = int.Parse(currentLevelFile.Substring(
                currentLevelFile.IndexOf(standardLevelName) + standardLevelName.Length,
                currentLevelFile.IndexOf(fileExtension) - currentLevelFile.IndexOf(standardLevelName) - standardLevelName.Length
            ));

            return currentLevelNumber;
        }

        public string GetNextLevelFile() {
            int nextLevelNumber = GetLevelNumber() + 1;
            string nextLevelFile = Path.Combine("..", "Assets", "Levels", $"level{nextLevelNumber}.txt");
            return nextLevelFile;
        }

        public EntityContainer<Block> GetBlocks() {
            return blocks;
        }

        public IDictionary<string, string> GetCoolKey() {
            return coolBlockKey;
        }
    }
}