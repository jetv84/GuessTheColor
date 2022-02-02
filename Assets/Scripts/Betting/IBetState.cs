namespace BettingApp.Bet
{
    public interface IBetState
    {
        void Transition(BetController controller);
    }
}