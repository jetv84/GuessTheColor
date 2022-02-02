
namespace BettingApp.Manager
{
    public enum BetAmmount
    {
        None = 0,
        Decrease = -1,
        Increase = 1
    }

    public enum BetColorID
    {
        None = 0,
        A = 1,
        B = 2
    }

    public enum PlayerState
    {
        Idle,
        Betting,
        Ready,
        Win,
        Lose
    }

    public enum BetState
    {
        SequenceInit,
        SetColorComplete,
        HideItemComplete,
        ShowItemComplete,
        SequenceComplete,
    }
}
