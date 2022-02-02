using System.Collections;
using UnityEngine;
using BettingApp.Manager;

namespace BettingApp.Bet
{
    public class BetHideState : MonoBehaviour, IBetState
    {
        private const float StateDelay = 0.5f;

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
            _betController.boxAnim.SetTrigger("hide");

            yield return new WaitForSecondsRealtime(StateDelay);

            _betController.EndBetSequence();

            _betController.CurrBetState = BetState.HideItemComplete;
        }
    }
}
