
using UnityEngine;
using BettingApp.Bet;
using BettingApp.Seat;
using BettingApp.Player;

namespace BettingApp.Manager
{
    public class GameManager : Singleton<GameManager>
    {
        [HideInInspector]
        public BetController bet;
        [HideInInspector]
        public SeatController seats;

        public GameSettingsScriptable settings;
    }
}