using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class s_BasicMenu<T> : s_Menu<T> where T: s_BasicMenu<T>
{
    public static void Show()
    {
        Open();
    }
    public static void Hide()
    {
        Close();
    }

    public override void OnBackPressed()
    {

    }
}
