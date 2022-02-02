
using UnityEngine;
using BettingApp.Manager;
using System.Collections;
using BettingApp.Player;
using Photon.Pun;
using BettingApp.Seat;

namespace BettingApp.Bet
{
    public class BetController : MonoBehaviourPunCallbacks
    {
        private IBetState _setColorState, _showState, _hideState;

        private const int MinRange = 1;
        private const int MaxRange = 3;

        private SeatController _seatController;

        private BetState _currBetState;
        private int _currentColor;
        private int _randomColor;

        public Animator boxAnim;
        public MeshRenderer itemMesh;

        public BetState CurrBetState
        {
            get { return _currBetState; }
            set { _currBetState = value; }
        }

        public int CurrentColor
        {
            get { return _currentColor; }
            set { _currentColor = value; }
        }

        /// <summary>
        /// Get the current Photon View.
        /// </summary>
        public PhotonView GetView()
        {
            return this.photonView;
        }

        private void Start()
        {
            Init();
        }
        /// <summary>
        /// The bet controller and its different states are initialized.
        /// </summary>
        public void Init()
        {
            GameManager.Instance.bet = this;

            _setColorState = gameObject.AddComponent<BetSetColorState>();
            _showState = gameObject.AddComponent<BetShowState>();
            _hideState = gameObject.AddComponent<BetHideState>();
        }

        /// <summary>
        /// We are ready here, Only the master generates a random color and start the bet sequence
        /// </summary>
        public void HandleBetSequence()
        {
            _seatController = GameManager.Instance.seats;

            if (PhotonNetwork.IsMasterClient)
            {
                // The master generates a random color, so it can be the same for both peers.
                GenerateRandomColor();

                // The master broadcast the generated color to the client.
                GetView().RPC("BroadcastGeneratedColor", RpcTarget.AllBuffered, _randomColor as object);
            }

            // At this point master and client are ready to start the sequnce.
            StartCoroutine(InitStateTransitions());
        }

        /// <summary>
        /// The Master generates a random color that will be compared with the selected by both peers
        /// </summary>
        private void GenerateRandomColor()
        {
            _randomColor = Random.Range(MinRange, MaxRange);
        }

        /// <summary>
        /// Broadcast the previously generated color by the master client. 
        /// </summary>
        [PunRPC]
        private void BroadcastGeneratedColor(int color)
        {
            CurrentColor = color;
        }

        /// <summary>
        /// Moves through the different betting states: "_setColorState, _showState, _hideState", 
        /// Wait until the current state is completed in order to move to the next one. 
        /// </summary>
        public IEnumerator InitStateTransitions()
        {
            // Wait until the player's state is set to Ready in both peers master and client to continue the sequence,
            // Thus the game will be synchronized for both peers.
            yield return new WaitUntil(() =>
                _seatController.localSeat.PlayerInstance.CurrPlayerState == PlayerState.Ready &&
                _seatController.remoteSeat.PlayerInstance.CurrPlayerState == PlayerState.Ready
            );

            CurrBetState = BetState.SequenceInit;

            _setColorState.Transition(this);

            yield return new WaitUntil(() => CurrBetState == BetState.SetColorComplete);

            _showState.Transition(this);

            yield return new WaitUntil(() => CurrBetState == BetState.ShowItemComplete);

            _hideState.Transition(this);

            yield return new WaitUntil(() => CurrBetState == BetState.HideItemComplete);

            CurrBetState = BetState.SequenceComplete;
        }

        /// <summary>
        /// After the random color is shown check for the results and takes the proper action.
        /// Send the result to the other peer.
        /// </summary>
        public void HandleBetResult()
        {
            PlayerController player = GameManager.Instance.seats.localSeat.PlayerInstance;

            if (player.GetView().IsMine)
            {
                if (CurrentColor == player.CurrColorSelected)
                {
                    player.GetView().RPC("HandlePlayerWin", RpcTarget.AllBuffered);
                }
                else
                {
                    player.GetView().RPC("HandlePlayerLose", RpcTarget.AllBuffered);
                }
            }
        }

        /// <summary>
        /// The bet sequence is completed, takes the proper actions for the next one.
        /// Send the result to the other peer.
        /// </summary>
        public void EndBetSequence()
        {
            PlayerController player = GameManager.Instance.seats.localSeat.PlayerInstance;

            if (player.GetView().IsMine)
            {
                player.GetView().RPC("ResetBetSettings", RpcTarget.AllBuffered);
            }
        }
    }
}
