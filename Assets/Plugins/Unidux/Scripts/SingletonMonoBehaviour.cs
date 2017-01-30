using UnityEngine;

namespace Unidux
{
    public class SingletonMonoBehaviour<TClass> : MonoBehaviour where TClass : SingletonMonoBehaviour<TClass>
    {
        protected static TClass instance;

        public static TClass Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (TClass) FindObjectOfType(typeof(TClass));

                    if (instance == null)
                    {
                        Debug.LogWarning(typeof(TClass) + "is nothing");
                    }
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            CheckInstance();
        }

        protected bool CheckInstance()
        {
            if (instance == null)
            {
                instance = (TClass) this;
                return true;
            }
            else if (Instance == this)
            {
                return true;
            }

            Destroy(this);
            return false;
        }
    }
}
