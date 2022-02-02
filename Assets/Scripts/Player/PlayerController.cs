
using UnityEngine;
using TMPro;
using BettingApp.Manager;
using BettingApp.Seat;
using System.Collections;
using Photon.Pun;

namespace BettingApp.Player
{
    public class PlayerController : MonoBehaviourPunCallbacks
    {
        private const float InitToGrantDelay = 1f;
        private const float LoseToGrantDelay = 3.5f;
        private const int WinClip = 0;
        private const int LoseClip = 1;

        private delegate void OnPlayerStateChange(PlayerState state);
        private OnPlayerStateChange onStateChange;

        private PlayerHUD _hud;
        private PlayerStacks _stacks;

        private int _currBetAmmount;
        private int _currTotalAmmount;
        private int _currColorSelected;

        private PlayerState _currPlayerState;

        [HideInInspector]
        public SeatData seatData;

        public int CurrBetAmount
        {
            get { return _currBetAmmount; }
            set { _currBetAmmount = value; }
        }

        public int CurrTotalAmmount
        {
            get { return _currTotalAmmount; }
            set { _currTotalAmmount = value; }
        }

        public int CurrColorSelected
        {
            get { return _currColorSelected; }
            set { _currColorSelected = value; }
        }

        public PlayerState CurrPlayerState
        {
            get { return _currPlayerState; }
            set { _currPlayerState = value; }
        }

        private void Start()
        {
            onStateChange += OnStateChange;

            InitPlayer();
        }

        /// <summary>
        /// Get the current Photon View.
        /// </summary>
        public PhotonView GetView()
        {
            return this.photonView;
        }

        /// <summary>
        /// The player data it's initialized.
        /// </summary>
        public void InitPlayer()
        {
            _hud = this.GetComponent<PlayerHUD>();
            _stacks = this.GetComponent<PlayerStacks>();

            if (GetView().IsMine)
            {
                seatData = GameManager.Instance.seats.localSeat;
                seatData.PlayerInstance = this;
                _hud.InitLocalHUD();
            }
            else
            {
                seatData = GameManager.Instance.seats.remoteSeat;
                seatData.PlayerInstance = this;
                _hud.InitRemoteHUD();
            }

            seatData.SetGrillPositions();

            this.transform.position = seatData.GetPlayerSeat.position;
            this.transform.rotation = seatData.GetPlayerSeat.rotation;

            _hud.textNickName.text = photonView.Owner.NickName;

            CurrBetAmount = (int)BetAmmount.None;
            _hud.textBet.text = "$" + CurrBetAmount;

            CurrColorSelected = (int)BetColorID.None;
            _hud.textColor.text = BetColorID.None.ToString();

            CurrPlayerState = PlayerState.Idle;
            _hud.textState.text = CurrPlayerState.ToString();

            StartCoroutine(GrantInitialAmmount(InitToGrantDelay));

            UIManager.Instance.SetPeerInfo();
        }

        /// <summary>
        /// Update the player's state according to its current state
        /// </summary>
        public void OnStateChange(PlayerState state)
        {
            CurrPlayerState = state;
            _hud.textState.text = CurrPlayerState.ToString();
        }

        /// <summary>
        /// The Increase or Decrease button was pressed, the player bet it's updated.
        /// The RPC message is sent to the remote client in case we are in MP mode
        /// </summary>
        [PunRPC]
        public void UpdateBet(short betAmmount)
        {
            int fixedBetAmmount = betAmmount * GameManager.Instance.settings.betAmmountMultiplier;

            // Check If the player's bet is bellow zero or above the current total ammount,
            // If so just return.
            if (ReachedBetLimits(fixedBetAmmount))
                return;

            CurrBetAmount += fixedBetAmmount;
            _hud.textBet.text = "$" + CurrBetAmount.ToString();

            onStateChange.Invoke(PlayerState.Betting);
        }

        /// <summary>
        /// The Color toggle was pressed, the player color it's updated.
        /// The RPC message is sent to the remote client in case we are in MP mode
        /// </summary>
        [PunRPC]
        public void UpdateColor(short betColorID)
        {
            string colorName = string.Empty;

            switch (betColorID)
            {
                case (int)BetColorID.A:
                    colorName = GameManager.Instance.settings.nameColorA;
                    break;
                case (int)BetColorID.B:
                    colorName = GameManager.Instance.settings.nameColorB;
                    break;
                default:
                    colorName = "N/A";
                    break;
            }

            CurrColorSelected = betColorID;
            _hud.textColor.text = colorName;

            onStateChange.Invoke(PlayerState.Betting);
        }

        /// <summary>
        /// The Place Bet button was pressed, the player its ready to start the bet sequence.
        /// The RPC message is sent to the remote client
        /// </summary>
        [PunRPC]
        public void PlaceBet()
        {
            onStateChange.Invoke(PlayerState.Ready);

            if (GetView().IsMine)
            {
                GameManager.Instance.bet.HandleBetSequence();
            }
        }

        /// <summary>
        /// The total ammount is updated after the bet sequence is completed.
        /// It's decreased or increased depending on the bet result.
        /// </summary>
        private void SetTotalAmmount(int newAmmount)
        {
            int stackFixedAmmount = newAmmount / 10;

            _stacks.UpdateStackAmmount(stackFixedAmmount);

            CurrTotalAmmount = newAmmount;
            _hud.textTotal.text = "$" + CurrTotalAmmount.ToString();
        }

        /// <summary>
        /// Check if the palyer bet is under the allowed min/max range.
        /// </summary>
        public bool ReachedBetLimits(int betAmmount)
        {
            int fixedMaxAmmount = CurrBetAmount + GameManager.Instance.settings.betAmmountMultiplier;

            if ((CurrBetAmount <= 0 && betAmmount < 0) ||
                (fixedMaxAmmount > CurrTotalAmmount && betAmmount > 0))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if the player won. If so, it's handled with the proper behavior.
        /// </summary>
        [PunRPC]
        public void HandlePlayerWin()
        {
            int newAmmount = CurrTotalAmmount + CurrBetAmount;

            SetTotalAmmount(newAmmount);
            onStateChange.Invoke(PlayerState.Win);

            if (GetView().IsMine)
            {
                AudioManager.Instance.PlaySoundClip(WinClip);
                UIManager.Instance.ShowPopUp("You Win!");
            }
        }

        /// <summary>
        /// Check if the player lose. If so, it's handled with the proper behavior.
        /// Check the current ammount if is less than zero, then grants the user more chips.
        /// </summary>
        [PunRPC]
        public void HandlePlayerLose()
        {
            onStateChange.Invoke(PlayerState.Lose);

            int newAmmount = CurrTotalAmmount - CurrBetAmount;

            SetTotalAmmount(newAmmount);

            if (GetView().IsMine)
            {
                AudioManager.Instance.PlaySoundClip(LoseClip);
                UIManager.Instance.ShowPopUp("You Lose!");
            }

            // The player ran out of chips, grant just a bit more.
            if (newAmmount <= 0)
            {
                StartCoroutine(GrantInitialAmmount(LoseToGrantDelay));
            }
        }

        /// <summary>
        /// The player's total ammount balance it's 0.
        /// Is granted with the additional initial ammount. 
        /// </summary>
        IEnumerator GrantInitialAmmount(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);

            SetTotalAmmount(GameManager.Instance.settings.initTotalAmmount);

            if (GetView().IsMine)
            {
                UIManager.Instance.ShowPopUp("Adding $" + GameManager.Instance.settings.initTotalAmmount);
            }
        }

        /// <summary>
        /// The Bet sequence has finished, The bet settings are restarted
        /// </summary>
        [PunRPC]
        public void ResetBetSettings()
        {
            CurrBetAmount = (int)BetAmmount.None;
            _hud.textBet.text = "$" + CurrBetAmount;

            CurrColorSelected = (int)BetColorID.None;
            _hud.textColor.text = BetColorID.None.ToString();

            CurrPlayerState = PlayerState.Idle;
            _hud.textState.text = CurrPlayerState.ToString();

            onStateChange.Invoke(CurrPlayerState);

            if (photonView.IsMine)
            {
                _hud.PlayerInputState(true);
                _hud.PlaceBetButtonState();
                _hud.ResetColorToggleGroup();
            }
        }

        /// <summary>
        /// Unsubscribe to the player state delegate
        /// </summary>
        private void OnDisable()
        {
            onStateChange -= OnStateChange;
        }
    }
}
