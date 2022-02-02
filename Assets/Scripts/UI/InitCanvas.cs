using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Photon.Pun;
using BettingApp.Manager;

namespace BettingApp.UI
{
    public class InitCanvas : MonoBehaviour
    {
        private const float DelayToCheck = 2.0f;

        [Header("UI Components")]
        [SerializeField]
        private Button _buttonPlay;
        [SerializeField]
        private TMP_InputField _textFieldName;

        private void Start()
        {
            _buttonPlay.onClick.AddListener(() => OnPlayButtonPressed());
            _textFieldName.onValueChanged.AddListener((value) => { OnTextNameChanged(value); });

            StartCoroutine(CheckForPhotonConnection());
        }

        private void OnPlayButtonPressed()
        {
            MultiplayerManager.Instance.StartMatch();
        }

        private void OnTextNameChanged(string name)
        {
            UIManager.Instance.nickName = name;
        }

        /// <summary>
        /// Enable the Play Button until the Photon connection is ready.
        /// </summary>
        IEnumerator CheckForPhotonConnection()
        {
            while (!PhotonNetwork.IsConnectedAndReady)
            {
                yield return new WaitForSeconds(DelayToCheck);
            }

            _buttonPlay.interactable = true;
        }
    }
}
