using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
        private static T _instance;

        public static T instance
        { 
                get
                {
                        if (_instance == null)
                        {
                                _instance = FindFirstObjectByType<T>();
                        }
                        
                        return _instance;
                }
                private set
                {
                        _instance = value;
                }
        }

        public virtual void Awake()
        {
                if (instance != null && instance != this)
                {
                        Destroy(this.gameObject);
                }
                else
                {
                        instance = (T) this;    
                }
        }
    
}
