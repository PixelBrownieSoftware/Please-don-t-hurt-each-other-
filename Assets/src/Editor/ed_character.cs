using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CanEditMultipleObjects]
[CustomEditor(typeof(o_BattleCharacterData))]
public class ed_character : Editor
{
    string[] _choices = new[] { "foo", "foobar" };
    int _choiceIndex = 0;

    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();
        var someClass = target as o_BattleCharacterData;
    }
}
