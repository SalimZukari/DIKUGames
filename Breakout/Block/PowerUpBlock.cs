using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Events;
using DIKUArcade.Math;
using DIKUArcade;
using System.IO;
using Breakout.PowerUps;

namespace Breakout.IBlock {
    public class PowerUpBlock : Block {
        private DynamicShape shape;
        private static readonly string[] powerUpImages = {
            Path.Combine("..", "Assets", "Images", "DoubleSpeedPowerUp.png.png"),
            Path.Combine("..", "Assets", "Images", "LifePickUp.png"),
            Path.Combine("..", "Assets", "Images", "SpeedPickUP.png"),
            Path.Combine("..", "Assets", "Images", "ball2.png"),
            Path.Combine("..", "Assets", "Images", "playerStride.png")
        };
        private Random random;

        public PowerUpBlock(DynamicShape shape, Image blocksImage, Image damagedImage, BlockType type) 
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
            int index = random.Next(powerUpImages.Length);
            string imagePath = powerUpImages[index];
            Image powerUpImage = new Image(imagePath);
            PowerUpType type = (PowerUpType)index; 
            var powerUp = new PowerUp(type, new DynamicShape(Shape.Position, Shape.Extent), powerUpImage, 10.0f);
            BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                EventType = GameEventType.GameStateEvent,
                Message = "SPAWN_POWERUP",
                ObjectArg1 = powerUp
            });
        }
    }
}