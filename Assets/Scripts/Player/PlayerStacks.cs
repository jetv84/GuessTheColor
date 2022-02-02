using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BettingApp.Manager;
using BettingApp.Pool;
using BettingApp.Seat;

namespace BettingApp.Player
{
    public class PlayerStacks : MonoBehaviour
    {
        private const float StackDelay = 0.1f;

        private SeatData _playerSeat;
 
        private int _currAmmount = 0;

        [HideInInspector]
        public List<GameObject> pooledObjects;

        private void Start()
        {
            _playerSeat = this.GetComponent<PlayerController>().seatData;
            pooledObjects = new List<GameObject>();
        }

        /// <summary>
        /// Update the stacks number with the new ammount
        /// </summary>
        public void UpdateStackAmmount(int newAmmount)
        {
            if (newAmmount > _currAmmount)
            {
                StartCoroutine(IncreaseStacks(newAmmount));
            }
            else
            {
                StartCoroutine(DecreaseStacks(newAmmount));
            }
        }

        /// <summary>
        /// Handle the pooled objects recycling  or instantiating the neccesary objects 
        /// depending on the bet result.
        /// </summary>
        public IEnumerator IncreaseStacks(int newAmmount)
        {
            int increaseDiff = newAmmount - _currAmmount;

            for (int i = 0; i < increaseDiff; i++)
            {
                GameObject currStack = PoolManager.Instance.GetPooledObject(this);
                currStack.SetActive(true);

                currStack.GetComponent<StackItem>().InitStack(_playerSeat);

                yield return new WaitForSecondsRealtime(StackDelay);
            }

            _currAmmount = newAmmount;
        }

        /// <summary>
        /// Disable the neccesary objects in the pool list depending on the bet result.
        /// </summary>
        public IEnumerator DecreaseStacks(int newAmmount)
        {
            int poolCount = PoolManager.Instance.GetAllPooledObjects(this).Count;

            for (int i = poolCount - 1; i >= newAmmount; i--)
            {
                GameObject target = PoolManager.Instance.GetAllPooledObjects(this)[i].gameObject;

                if (!target.activeInHierarchy)
                    continue;

                target.SetActive(false);

                yield return new WaitForSecondsRealtime(StackDelay);
            }

            _currAmmount = newAmmount;
        }
    }
}

