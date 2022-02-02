using UnityEngine;

namespace BettingApp.Manager
{
    public class  Singleton<T> : 
        MonoBehaviour where T : Component {
        
        private static T _instance;

        /// <summary>
        /// Get the current instance of the generic object, if null it's created.
        /// </summary>
        public static T Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Check the current state of the instance and take the proper action
        /// </summary>
        public virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}