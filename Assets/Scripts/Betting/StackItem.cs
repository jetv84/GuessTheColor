using UnityEngine;
using BettingApp.Manager;
using BettingApp.Seat;

namespace BettingApp.Pool
{
    public class StackItem : MonoBehaviour
    {
        private bool _isInitialized = false;

        public MeshRenderer mesh;

        /// <summary>
        /// The stack item is initialized, check if it is already ininitialized,
        /// If so the pool system can recycle it and just enable the already instantiated one
        /// Otherwise assing the proper color and position in the grill to the new one.
        /// </summary>
        public void InitStack(SeatData playerSeat)
        {
            if (_isInitialized)
                return;

            // Lucky player! RNo more available spots in the grill / "table"
            // Juts place the stack in the default vector.zero... for now...
            // TODO: Clear the current grill position list and create a new one in the top level Y axis?
            if (playerSeat.grillPosIndex >= playerSeat.posInGrill.Count)
                return;

            Vector3 stackPos = playerSeat.posInGrill[playerSeat.grillPosIndex];
            this.transform.position = stackPos;

            Color32 currColor = GameManager.Instance.settings.chipColors[playerSeat.chipColorIndex];
            mesh.material.color = currColor;

            playerSeat.grillPosIndex++;
            playerSeat.chipColorIndex++;

            if (playerSeat.chipColorIndex == GameManager.Instance.settings.chipColors.Length)
                playerSeat.chipColorIndex = 0;

            _isInitialized = true;
        }
    }
}
