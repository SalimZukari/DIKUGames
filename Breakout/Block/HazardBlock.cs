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
            EffectType type = (EffectType)(index + 5);
            Effect hazard = HazardTypeToObject(type, hazardImage);
            if (GameRunning.Effects != null) {
                GameRunning.Effects.AddEntity(hazard);
            }
        }

        public Effect HazardTypeToObject(EffectType type, Image image) {
            switch (type) {
                case EffectType.LoseLife:
                    return new LoseLife(new DynamicShape(Shape.Position, Shape.Extent), image);
                case EffectType.Slowness:
                    return new Slowness(new DynamicShape(Shape.Position, Shape.Extent), image);
                default:
                    return new Effect(type, new DynamicShape(Shape.Position, Shape.Extent), image);
            }
        }
    }
}