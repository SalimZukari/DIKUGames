using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Events;
using DIKUArcade.Math;
using DIKUArcade;
using System.IO;
using Breakout.BreakoutStates;
using Breakout.PowerUps;

namespace Breakout.IBlock {
    public class HazardBlock : Block {
        private DynamicShape shape;
        private static readonly string[] hazardImages = {
            Path.Combine("..", "Assets", "Images", "LoseLife.png"),
            Path.Combine("..", "Assets", "Images", "Slowness.png")
        };
        private Random random;

        public HazardBlock(DynamicShape shape, Image blocksImage, Image damagedImage, BlockType type) 
            : base(shape, blocksImage, damagedImage, type) {
                this.blocksImage = blocksImage;
                this.shape = shape;
                health = 10;
                random = new Random();
        }

        public override void Damage() {
            health -= 10;
            if (health <= 0) {
                SpawnPowerUp();
                Destroy();
            }
        }

        public void SpawnPowerUp() {
            int index = random.Next(hazardImages.Length);
            string imagePath = hazardImages[index];
            Image hazardImage = new Image(imagePath);
            EffectType type = (EffectType)(index + 4); 
            Effect hazard = new Effect(type, new DynamicShape(Shape.Position, Shape.Extent), hazardImage);
            GameRunning.Effects.AddEntity(hazard);
        }
    }
}