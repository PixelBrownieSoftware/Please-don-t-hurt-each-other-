using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_Singleton<T> : MonoBehaviour where T : s_Singleton<T>
{
    public static T instance { get; private set; }

    protected void Awake()
    {
        instance = (T)this;
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
