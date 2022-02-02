using System.Collections;
using UnityEngine;
using BettingApp.Manager;

namespace BettingApp.Bet
{
    public class BetShowState : MonoBehaviour, IBetState
    {
        private const float StateDelay = 2f;

        private BetController _betController;

        public void Transition(BetController betController)
        {
            if (!_betController)
                _betController = betController;

            StartCoroutine(Handle());
        }

        /// <summary>
        /// Handle the proper state behavior, when is done the current bet state is updated.
        /// </summary>
        IEnumerator Handle()
        {
            _betController.boxAnim.SetTrigger("show");

            yield return new WaitForSecondsRealtime(StateDelay);

            _betController.CurrBetState = BetState.ShowItemComplete;
        }
    }
}