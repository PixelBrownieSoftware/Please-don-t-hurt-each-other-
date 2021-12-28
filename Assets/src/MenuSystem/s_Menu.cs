using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class s_Menu<T> : s_Menu where T : s_Menu<T>
{
    public static T instance { get; private set; }

    protected virtual void Awake()
    {
        instance = (T)this;
    }

    protected static void Open() {

        if (instance == null)
        {
            s_MenuManager.instance.CreateMenu<T>();
        }
        else
        {
            instance.gameObject.SetActive(true);
        }
        s_MenuManager.instance.OpenMenu(instance);
    }
    protected static void Close()
    {
        s_MenuManager.instance.CloseMenu(instance);
    }
}
public abstract class s_Menu : MonoBehaviour
{
    public bool DestroyWhenClosed = true;

    public bool DisableMenusUnderneath = true;

    public abstract void OnBackPressed();
}