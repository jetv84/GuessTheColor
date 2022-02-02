using System.Collections.Generic;
using UnityEngine;
using BettingApp.Player;

namespace BettingApp.Manager
{
    public class PoolManager : Singleton<PoolManager>
    {
        private const int StackObjIndex = 0;

        public List<ObjectPoolItem> itemsToPool;

        /// <summary>
        /// Checkfor the current objects in pool, if is needed instantiate more, 
        /// otherwise return the pool object to be enabled in runtime.
        /// </summary>
        public GameObject GetPooledObject(PlayerStacks player)
        {
            int curSize = player.pooledObjects.Count;
            for (int i = 0; i < curSize; i++)
            {
                if (!player.pooledObjects[i].activeInHierarchy)
                {
                    return player.pooledObjects[i];
                }
            }

            if (itemsToPool[StackObjIndex].enableIncrease)
            {
                GameObject obj = (GameObject)Instantiate(itemsToPool[StackObjIndex].objectToPool);
                obj.transform.parent = this.transform;
                player.pooledObjects.Add(obj);

                return obj;
            }

            return null;
        }

        /// <summary>
        /// Get the List with all the current pooled objects in the scene.
        /// </summary>
        public List<GameObject> GetAllPooledObjects(PlayerStacks player)
        {
            return player.pooledObjects;
        }
    }

    [System.Serializable]
    public class ObjectPoolItem
    {
        public GameObject objectToPool;
        public int initPoolAmmount;
        public bool enableIncrease = true;
    }
}
