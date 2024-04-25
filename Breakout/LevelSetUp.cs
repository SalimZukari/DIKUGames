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

        public LevelSetUp(string file) {
            layout = new InterpretData(file);
            blocks = new EntityContainer<Block>();

            SetUp();
        }

        public void SetUp() {
            var colors = layout.GetLegendOrganized().Values;
            var positions = layout.GetMapOrganized();

            foreach (var colorEntry in colors) {
                if (positions.TryGetValue(colorEntry, out List<(float, float)>? positionsList)) {
                    if (positionsList != null) {
                        foreach ((float x, float y) in positionsList) {
                            blocks.AddEntity(new Block(
                                new DynamicShape(new Vec2F(x, y), new Vec2F(0.1f, 0.05f)),
                                new Image(Path.Combine("..", "Assets", "Images", colorEntry))
                            ));
                        }
                    } else {
                        Console.WriteLine("positionList is null");
                    }
                } 
            }
        }

        public EntityContainer<Block> GetBlocks() {
            return blocks;
        }
    }
}