using System;
using UnityEngine;
using TMPro;

using Photon.Pun;

namespace BettingApp.Manager
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField]
        private PopUp _popUp;
        [SerializeField]
        private TextMeshProUGUI _peerInfo;
        [SerializeField]
        private TextMeshProUGUI _stateInfo;

        [HideInInspector]
        public string nickName;

        /// <summary>
        /// Show the proper peer connection type, master or client
        /// Just for informative purposes...
        /// </summary>
        public void SetPeerInfo()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _peerInfo.text = "Master";
            }
            else
            {
                _peerInfo.text = "Client";
            }
        }

        /// <summary>
        /// Show the current status of the Photon Behavior with the meesage to show as a parameter., 
        /// this will be shown only in the Init scene
        /// </summary>
        public void ShowInfoText(string message)
        {
            _stateInfo.text = message;
        }

        /// <summary>
        /// Show the proper Popup with the meesage to show as a parameter.
        /// </summary>
        public void ShowPopUp(string message)
        {
            _popUp.gameObject.SetActive(true);
            _popUp.textMessage.text = message;
        }
    }
}
