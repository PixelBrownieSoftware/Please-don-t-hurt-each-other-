using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New move", menuName = "Move")]
public class d_move : ScriptableObject
{
    [System.Serializable]
    public struct condCost {
        public string name;
        public int number;
    }

    public enum MOVE_TYPE {
        ATTACK,
        STATUS,
        ESCAPE
    }
    public int magCost = 0;
    public int power;
    public condCost[] costs;
    public s_Variable[] flagInc;
    public enum TARG_TYPE {
        SINGLE,
        ALL,
        SELF
    }
    public TARG_TYPE targetType;
    public MOVE_TYPE moveType;
    public GameObject animObj;
}
