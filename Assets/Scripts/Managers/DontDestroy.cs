using UnityEngine;

namespace BettingApp.Manager
{
    public class DontDestroy : MonoBehaviour
    {
        private static DontDestroy _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
