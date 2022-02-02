
using BettingApp.Manager;

namespace BettingApp.Player
{
    public abstract class PlayerCommand
    {
        public virtual void Execute() { }
        public virtual void Execute(short enumValue) { }
    }
}