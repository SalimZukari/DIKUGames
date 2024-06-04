using Breakout.PowerUps;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using Breakout.BreakoutStates;

namespace Breakout.IBlock {
    public class HazardBlock : Block {
        /// <summary>
        /// These blocks spawn hazards when hit
        /// </summary>
        private DynamicShape shape;
        private static readonly string[] hazardImages = {
            Path.Combine("..", "Assets", "Images", "LoseLife.png"),
            Path.Combine("..", "Assets", "Images", "Slowness.png")
        };
        private Random random;

        public HazardBlock(DynamicShape shape, Image blocksImage, Image damagedImage, 
            BlockType type) 
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

        /// <summary>
        /// Spawns a hazard when the block is destroyed
        /// </summary>
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

        /// <summary>
        /// Converts the hazard type to an object
        /// </summary>
        public Effect HazardTypeToObject(EffectType type, Image image) {
            switch (type) {
                case EffectType.LoseLife:
                    return new LoseLife(new DynamicShape(Shape.Position, Shape.Extent), image);
                case EffectType.Slowness:
                    return new Slowness(new DynamicShape(Shape.Position, Shape.Extent), image);
                default:
                    return new Effect(type, new DynamicShape(Shape.Position, Shape.Extent), 
                        image);
            }
        }
    }
}