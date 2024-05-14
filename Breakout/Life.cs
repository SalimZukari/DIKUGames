using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Breakout;

public class Life : Entity {
    private static Vec2F extent = new Vec2F(0.035f, 0.035f);

    public Life(Vec2F position, Image fullHeart, Image emptyHeart)
        : base(new StationaryShape(position, extent), fullHeart) {
    }
}