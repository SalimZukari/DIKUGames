using System;

namespace Breakout.IBlock;
public interface IBlock {
    int Health { get; }
    void Damage();
}