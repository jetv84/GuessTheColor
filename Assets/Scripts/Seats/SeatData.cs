
using BettingApp.Player;
using UnityEngine;

namespace BettingApp.Seat
{
    public class SeatData : SeatBase
    {
        public override PlayerController PlayerInstance
        { 
            get { return base.PlayerInstance; }
            set { base.PlayerInstance = value; }
        }

        public override Transform GetPlayerSeat 
        { 
            get { return base.GetPlayerSeat; } 
        }

        public override void SetGrillPositions() { base.SetGrillPositions(); }
    }
}
