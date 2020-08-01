using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<Instance> : MonoBehaviour where Instance : Singleton<Instance>
{
    public static Instance instance;

    public virtual void Awake()
    {
        if (!instance)
        {
            instance = this as Instance;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
