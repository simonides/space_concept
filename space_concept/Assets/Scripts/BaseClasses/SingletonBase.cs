using UnityEngine;
using System.Collections;
namespace Custom
{
    namespace Base
    {
        public abstract class SingletonBase<T> : MonoBehaviour
            where T : SingletonBase<T>
        {


            private static T instance = null;

            public static T GetInstance()
            {
                //Debug.Log ("Get Singleton Insance");
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<T>();
                    if(instance == null) {
                        throw new MissingComponentException("Can't find GameObject of type " + typeof(T).Name);
                    }

                    DontDestroyOnLoad(instance.gameObject);
                }
                return instance;
            }

            protected virtual void Awake()
            {
                Debug.Log("Singleton Awake");
                if (instance == null)
                {
                    ///instance = T;
                    Debug.Log("Singleton Init");
                    //DontDestroyOnLoad(instance);
                }
                else
                {
                    if (this != instance)
                    {
                        Debug.Log("Destroyed addtional Singleton");
                        Destroy(this.gameObject);
                    }
                }
            }
            protected virtual void Awake(T derived)
            {
                //Debug.Log ("Singleton Awake");
                if (instance == null)
                {
                    instance = derived;
                    //Debug.Log ("Singleton Init");
                    DontDestroyOnLoad(instance);
                }
                else
                {
                    if (derived != instance)
                    {
                        //Debug.Log("Destroyed addtional Singleton");
                        Destroy(this.gameObject);
                    }
                }
            }

        }
    }
}
