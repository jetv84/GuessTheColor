using System.Collections;
using UnityEngine;
using BettingApp.Manager;

namespace BettingApp.Bet
{
    public class BetSetColorState : MonoBehaviour, IBetState
    {
        private const float StateDelay = 1f;

        private BetController _betController;

        public void Transition(BetController betController)
        {
            if (!_betController)
                _betController = betController;

            StartCoroutine(Handle());
        }

        /// <summary>
        /// Handle the proper state behavior, when is done the current bet state is updated.
        /// Choose a Random Color: A = 1, B = 2
        /// </summary>
        IEnumerator Handle()
        {
            yield return new WaitForSecondsRealtime(StateDelay);

            int randomColor = _betController.CurrentColor;

            Color32 currColor;

            switch (randomColor)
            {
                case 1:
                    currColor = GameManager.Instance.settings.betColorA;
                    break;
                case 2:
                    currColor = GameManager.Instance.settings.betColorB;
                    break;
                default:
                    currColor = Color.white;
                    break;
            }

            _betController.itemMesh.material.color = currColor;

            yield return new WaitForSecondsRealtime(StateDelay);

            _betController.HandleBetResult();

            _betController.CurrBetState = BetState.SetColorComplete;
        }
    }
}