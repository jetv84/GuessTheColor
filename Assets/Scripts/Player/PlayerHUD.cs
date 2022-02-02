
using UnityEngine;
using UnityEngine.UI;
using BettingApp.Manager;
using TMPro;

namespace BettingApp.Player
{
    public class PlayerHUD : MonoBehaviour
    {
        private const int ClickClip = 2;

        private PlayerInvoker _invoker;
        private PlayerController _playerController;
        private PlayerCommand _updateBet, _updateColor, _placeBet;

        [Header("Input Components")]
        [SerializeField]
        private GameObject _inputPanel;
        [SerializeField]
        private Button _buttonBetDecrease;
        [SerializeField]
        private Button _buttonBetIncrease;
        [SerializeField]
        private Button _buttonPlaceBet;
        [SerializeField]
        private Toggle _toggleColorA;
        [SerializeField]
        private Toggle _toggleColorB;
        [SerializeField]
        private ToggleGroup _toggleGroupColor;

        [Header("HUD Components")]
        public Camera mainCamera;
        public RectTransform infoPanel;
        public TextMeshProUGUI textNickName;
        public TextMeshProUGUI textBet;
        public TextMeshProUGUI textColor;
        public TextMeshProUGUI textTotal;
        public TextMeshProUGUI textState;

        /// <summary>
        /// Add the local player input commands and listeners
        /// </summary>
        public void InitLocalHUD()
        {
            _invoker = this.GetComponent<PlayerInvoker>();
            _playerController = this.GetComponent<PlayerController>();

            _updateBet = new UpdateBet(_playerController);
            _updateColor = new UpdateColor(_playerController);
            _placeBet = new PlaceBet(_playerController);

            // Buttons LIsteners
            _buttonBetDecrease.onClick.AddListener(() => OnAmmountButtonPressed((short)BetAmmount.Decrease));
            _buttonBetIncrease.onClick.AddListener(() => OnAmmountButtonPressed((short)BetAmmount.Increase));
            _buttonPlaceBet.onClick.AddListener(() => OnPlaceBetButtonPressed());

            // Toggle Listeners
            _toggleColorA.onValueChanged.AddListener((value) => { OnColorToggleChanged(value, (short)BetColorID.A); });
            _toggleColorB.onValueChanged.AddListener((value) => { OnColorToggleChanged(value, (short)BetColorID.B); });

            // Assign the betting colors to the toggles
            _toggleColorA.GetComponentInChildren<Image>().color = GameManager.Instance.settings.betColorA;
            _toggleColorB.GetComponentInChildren<Image>().color = GameManager.Instance.settings.betColorB;
        }

        /// <summary>
        /// Hide the Input panel for remote players.
        /// </summary>
        public void InitRemoteHUD()
        {
            _inputPanel.SetActive(false);
            infoPanel.anchorMin = new Vector2(0.18f, 0.78f);
            infoPanel.anchorMax = new Vector2(0.18f, 0.78f);
            mainCamera.gameObject.SetActive(false);
        }

        /// <summary>
        /// Ammount Buttons Listener
        /// </summary>
        private void OnAmmountButtonPressed(short betAmmount)
        {
            _invoker.ExecuteCommand(_updateBet, betAmmount);

            // Check if we are ready to bet, if so enable the bet button
            PlaceBetButtonState();

            AudioManager.Instance.PlaySoundClip(ClickClip);
        }

        /// <summary>
        /// Color Toggles Listener
        /// </summary>
        private void OnColorToggleChanged(bool value, short colorID)
        {
            if (!value) return;

            _invoker.ExecuteCommand(_updateColor, colorID);

            // Check if we are ready to bet, if so enable the bet button
            PlaceBetButtonState();

            // Avoid to uncheck the pressed Toggle
            _toggleGroupColor.allowSwitchOff = false;

            AudioManager.Instance.PlaySoundClip(ClickClip);
        }

        /// <summary>
        /// Place Bet Button Listener
        /// </summary>
        private void OnPlaceBetButtonPressed()
        {
            _invoker.ExecuteCommand(_placeBet);

            AudioManager.Instance.PlaySoundClip(ClickClip);

            // Disable the UI buttons
            PlayerInputState(false);
        }

        /// <summary>
        /// Check the current state of the PlaceBetButton, 
        /// if there is already a bet placed and color selected, 
        /// then it's enabled, otherwise it's disabled. 
        /// </summary>
        public void PlaceBetButtonState()
        {
            bool enableButton = false;

            if (_playerController.CurrBetAmount != 0 && 
                _playerController.CurrColorSelected != 0)
            {
                enableButton = true;
            }

            _buttonPlaceBet.interactable = enableButton;
        }

        /// <summary>
        /// Enable or Disable the User input depending on the current bet state
        /// </summary>
        public void PlayerInputState(bool state)
        {
            _buttonPlaceBet.interactable = state;
            _buttonBetDecrease.interactable = state;
            _buttonBetIncrease.interactable = state;
            _toggleColorA.interactable = state;
            _toggleColorB.interactable = state;
        }

        /// <summary>
        /// Reset the Toggle group to its default state.
        /// </summary>
        public void ResetColorToggleGroup()
        {
            _toggleGroupColor.allowSwitchOff = true;
            _toggleGroupColor.SetAllTogglesOff();
        }
    }
}
