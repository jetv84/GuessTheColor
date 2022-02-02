
using UnityEngine;
using BettingApp.Manager;
using System.IO;
using Photon.Pun;

namespace BettingApp.Seat
{
    public class SeatController : MonoBehaviour
    {
        public SeatData localSeat;
        public SeatData remoteSeat;

        private void Start()
        {
            GameManager.Instance.seats = this;

            CreatePlayer();
        }

        /// <summary>
        /// Instantiate the Photon Player in the scene.
        /// </summary>
        private void CreatePlayer()
        {
            PhotonNetwork.Instantiate(Path.Combine("Photon", "Player"), Vector3.zero, Quaternion.identity);
        }
    }
}
