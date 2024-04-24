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

        public LevelSetUp() {
            layout = new InterpretData("Assets/Levels/level3.txt");
            blocks = new EntityContainer<Block>();
        }

        public EntityContainer<Block> SetUp() {
            var colors = layout.ReadLegend().Values;
            var positions = layout.ReadMap();

            foreach (var colorEntry in colors) {
                if (positions.TryGetValue(colorEntry, out List<(float, float)> positionsList)) {
                    foreach ((float x, float y) in positionsList) {
                        // Console.WriteLine("{0} {1}", x, y);
                        blocks.AddEntity(new Block(
                            new DynamicShape(new Vec2F(x, y), new Vec2F(0.1f, 0.05f)),
                            new Image(Path.Combine("Assets", "Images", colorEntry))
                        ));
                    }
                } 
            }

            return blocks;
        }
    }
}