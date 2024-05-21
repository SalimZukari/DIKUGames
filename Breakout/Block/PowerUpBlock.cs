using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Events;
using DIKUArcade.Math;
using DIKUArcade;
using System.IO;
using Breakout.BreakoutStates;
using Breakout.PowerUps;

namespace Breakout.IBlock {
    public class PowerUpBlock : Block {
        private DynamicShape shape;
        private static readonly string[] powerUpImages = {
            Path.Combine("..", "Assets", "Images", "DoubleSpeedPowerUp.png"),
            Path.Combine("..", "Assets", "Images", "LifePickUp.png"),
            Path.Combine("..", "Assets", "Images", "SpeedPickUp.png"),
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
            int index = random.Next(powerUpImages.Length-2);
            string imagePath = powerUpImages[index];
            Image powerUpImage = new Image(imagePath);
            EffectType type = (EffectType)(index); 
            Effect powerUp = PowerUpTypeToObject(type, powerUpImage);
            GameRunning.Effects.AddEntity(powerUp);
            BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                EventType = GameEventType.GameStateEvent,
                Message = "SPAWN_POWERUP",
                ObjectArg1 = powerUp
            });
        }

        public Effect PowerUpTypeToObject(EffectType type, Image image) {
            switch (type) {
                case EffectType.DoubleSpeed:
                    return new DoubleSpeed(new DynamicShape(Shape.Position, Shape.Extent), image);
                case EffectType.ExtraLife:
                    return new ExtraLife(new DynamicShape(Shape.Position, Shape.Extent), image);
                case EffectType.PlayerSpeed:
                    return new PlayerSpeed(new DynamicShape(Shape.Position, Shape.Extent), image);
                case EffectType.DoubleSize:
                    return new DoubleSize(new DynamicShape(Shape.Position, Shape.Extent), image);
                case EffectType.Wide:
                    return new Wide(new DynamicShape(Shape.Position, Shape.Extent), image);
                default:
                    return new Effect(type, new DynamicShape(Shape.Position, Shape.Extent), image);
            }
        }
    }
}