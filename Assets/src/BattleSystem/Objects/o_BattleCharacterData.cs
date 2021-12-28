using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class s_BattleCharacterVariable {
    public string name;
    public int maxVal = 1, minVal, val;

    public s_BattleCharacterVariable() {

    }

    public s_BattleCharacterVariable(string name, int val, int minVal, int maxVal)
    {
        this.name = name;
        this.val = val;
        this.minVal = minVal;
        this.maxVal = maxVal;
    }

    public s_BattleCharacterVariable GetClone() {
        return new s_BattleCharacterVariable(name, val, minVal, maxVal);
    }
}

[System.Serializable]
public class s_Variable
{
    public string name;
    public int val;
}

[System.Serializable]
public class s_Goal {
    public enum GOAL_TYPE
    {
        STAT_CHECK,
        KILL_ALL,
        ESCAPE
    }
    public enum TARGET {
        SPECIFIC,
        ALLIES,
        OPPOSITION,
        SELF
    }
    public TARGET target;
    public GOAL_TYPE goalType;
    public AI_condition[] conditions;
    public int goalToGoToOnceFufilled;
}

[CreateAssetMenu(fileName = "New goal data", menuName = "AI goal data")]
public class d_CharacterGoalData : ScriptableObject
{
    public s_Goal[] goals;
}

[CreateAssetMenu(fileName ="New battle character", menuName = "Battle character data")]
public class o_BattleCharacterData : ScriptableObject
{
    public enum CHAR_TYPE {
        NORMAL,
        STRONG,
        UNIQUE
    }
    public CHAR_TYPE characterType;

    public int turnIcons = 1;
    public int health;
    public int moveRangeDefault;
    public o_BattleCharAI[] actions;
    public s_BattleCharacterVariable FindVariable(string varName) {
        foreach (var stuff in variable) {
            if (varName == stuff.name) {
                return stuff;
            }
        }
        return null;
    }
    public s_BattleCharacterVariable[] variable;
    public d_move[] moves;
    public string[] aiFlags;
    public RuntimeAnimatorController animation;
}
