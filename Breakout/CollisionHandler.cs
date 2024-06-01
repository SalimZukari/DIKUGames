using DIKUArcade.Entities;
using DIKUArcade.Physics;
using DIKUArcade.Math;
using Breakout;
using Breakout.PowerUps;
using Breakout.IBlock;
using Breakout.BreakoutStates;

namespace Breakout {
    public class CollisionHandler {
        private Player player;
        private LevelSetUp level;
        private EntityContainer<Ball> balls;
        private EntityContainer<Effect> effects;
        private EntityContainer<Effect> collidedEffects;
        private int score;

        public CollisionHandler(Player player, LevelSetUp level, EntityContainer<Ball> balls, 
                                EntityContainer<Effect> effects, EntityContainer<Effect> collidedEffects, int Score) {
            this.player = player;
            this.level = level;
            this.balls = balls;
            this.effects = effects;
            this.collidedEffects = collidedEffects;
            this.score = Score;
        }

        public void CheckCollisions() {
            balls.Iterate(ball => {
                ball.Movement();
                ball.CheckDeleteBall();
                CheckBlockCollisions(ball);
                CheckPlayerCollisions(ball);
            });
        }

        private void CheckBlockCollisions(Ball ball) {
            level.GetBlocks().Iterate(block => {
                var collideBlock = CollisionDetection.Aabb((DynamicShape)ball.Shape, block.Shape);
                if (collideBlock.Collision) {
                    if (block is PowerUpBlock powerUpBlock) {
                        powerUpBlock.Damage();
                    } else {
                        block.Damage();
                    }
                    if (block.Health == 0) {
                        score += 10;
                    }
                    ball.HitsBlockMove();
                }
            });
        }

        private void CheckPlayerCollisions(Ball ball) {
            float playerLeft = player.Shape.Position.X;
            float playerRight = player.Shape.Position.X + player.Shape.Extent.X;
            float playerMid = (playerRight + playerLeft) / 2.0f;
            float ballPos = ball.Shape.Position.X + (ball.Shape.Extent.X / 2.0f);
            var collidePlayer = CollisionDetection.Aabb((DynamicShape)ball.Shape, player.Shape);
            if (collidePlayer.Collision) {
                if (playerLeft <= ballPos && ballPos < playerMid) {
                    ball.GoLeft();
                } else if (playerMid <= ballPos && ballPos < playerRight) {
                    ball.GoRight();
                }
            }
        }

        public void CheckEffectCollisions() {
            if (effects != null && collidedEffects != null) {
                effects.Iterate(effect => {
                    effect.Update();
                    if (player.Shape.Position.X < effect.Shape.Position.X 
                        && effect.Shape.Position.X < player.Shape.Position.X + player.Shape.Extent.X
                        && player.Shape.Position.Y < effect.Shape.Position.Y 
                        && effect.Shape.Position.Y < player.Shape.Position.Y + player.Shape.Extent.Y) {
                            GameRunning.ApplyActivate(effect, player, balls);
                            collidedEffects.AddEntity(effect);
                            effect.DeleteEntity();
                    } else if (effect.Shape.Position.Y < 0.0f) {
                        effect.DeleteEntity();
                    }
                });
            }
        }

        public int GetScore() {
            return score;
        }
    }
}