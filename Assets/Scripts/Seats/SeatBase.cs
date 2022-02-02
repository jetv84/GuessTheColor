using System.Collections.Generic;
using UnityEngine;
using BettingApp.Manager;
using BettingApp.Player;

namespace BettingApp.Seat
{
    public class SeatBase : MonoBehaviour
    {
        private const int BulksPerRow = 2;
        private const int TotalOfStacks = 120;
        private const int StacksPerBulk = 10;

        private PlayerController _playerInstance;

        private Vector3 _currStackPos;
        private Vector3 _currBulkPos;

        private float _bulkOffsetMult;
        private float _stackOffsetMult;
        private float _currStackOffsetX;
        private float _currStackOffsetZ;

        private int _currStackCount;
        private int _currBulkCount;

        [SerializeField]
        private Transform _playerSeat;
        [SerializeField]
        private Transform _grillAnchor;

        [HideInInspector]
        public int grillPosIndex;
        [HideInInspector]
        public int chipColorIndex;
        [HideInInspector]
        public List<Vector3> posInGrill;

        /// <summary>
        /// Get the player Instance, wether is local or remote.
        /// </summary>
        public virtual PlayerController PlayerInstance
        {
            get { return _playerInstance; }
            set { _playerInstance = value; }
        }

        /// <summary>
        /// Get the player position, wether is local or remote.
        /// </summary>
        public virtual Transform GetPlayerSeat
        {
            get { return _playerSeat; }
        }

        /// <summary>
        /// Initialize the positions in the grill, A list of available positions is created,
        /// in order to place the player stacks in the proper spot wether is local or remote.
        /// </summary>
        public virtual void SetGrillPositions()
        {
            posInGrill = new List<Vector3>();

            _bulkOffsetMult = GameManager.Instance.settings.bulkOffsetMultiplier;
            _stackOffsetMult = GameManager.Instance.settings.stackOffsetMultiplier;

            _currStackCount = 0;
            _currBulkCount = 0;

            Vector3 currPos = _grillAnchor.position;
            _currStackPos = currPos;
            _currBulkPos = currPos;

            _currStackOffsetX = _currStackPos.x;
            _currStackOffsetZ = _currStackPos.z;

            for (int i = 0; i < TotalOfStacks; i++)
            {
                posInGrill.Add(currPos);

                currPos = GetNextPositionInGrill();
            }
        }

        /// <summary>
        /// Get the next spot position in the "imaginary" grill, creating bulks of 5 by 2
        /// </summary>
        private Vector3 GetNextPositionInGrill()
        {
            _currStackCount++;

            // Get the next bulk of chips position 
            if (_currStackCount == StacksPerBulk)
            {
                _currStackCount = 0;
                _currBulkCount++;

                // Moves the bulk anchor to the next column
                if (_currBulkCount == BulksPerRow)
                {
                    _currBulkCount = 0;

                    _currBulkPos.x += _bulkOffsetMult * 2;
                    _currBulkPos.z = _currBulkPos.z - _bulkOffsetMult;
                }
                // Moves the bulk anchor in the current row to the top
                else
                {
                    _currBulkPos.z = _currBulkPos.z + _bulkOffsetMult;
                }

                _currStackOffsetX = _currBulkPos.x;
                _currStackOffsetZ = _currBulkPos.z;
            }
            // Get the stack position in the current bulk of chips
            else
            {
                // Moves the stacks in the z axis
                if (_currStackCount == StacksPerBulk / 2)
                {
                    _currStackOffsetX = _currBulkPos.x;
                    _currStackOffsetZ += _stackOffsetMult;
                }
                // Move the stacks in the X axis
                else
                {
                    _currStackOffsetX += _stackOffsetMult;
                }
            }

            // Finally, we got the next position!!!
            return new Vector3(_currStackOffsetX, _grillAnchor.position.y, _currStackOffsetZ);
        }
    }
}
