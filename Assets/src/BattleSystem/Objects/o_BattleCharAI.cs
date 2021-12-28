using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public struct AI_condition {
    
    public string condName;
    public bool condTrue;
    public int condNum;
    public enum COND_TYPE {
        VAR_EQ,
        VAR_GR,
        VAR_GR_EQ,
        VAR_LESS_EQ,
        VAR_LESS,
        CHECK_IF_CHAR_EXIST_PARTY,
        CHECK_IF_CHAR_EXIST_OPP
    }
    //For setting the target if the condition is true
    public bool setTarget;
    public string targName;
    public COND_TYPE conditionType;
}


/*
[System.Serializable]
public class o_BattleCharAI
{
    public AI_condition[] conditions;
    public AI_condition[] effectConditions;
    public AI_condition[] effects;

    //Condition | Effect
    public bool MatchActionsCond(bool flag, string flName)
    {
        bool boolean = false;
        if (conditions.Length > 0)
        {
            foreach (var condition in conditions)
            {
                if (condition.condName == flName
                    && condition.condTrue == flag)
                {
                    boolean = true;
                    break;
                }
            }
        }
        else
        {
            boolean = true;
        }
        return boolean;
    }
    //The requirements that are needed to get this to trigger the effect
    public bool MatchActionsEffectCond(bool flag, string flName)
    {
        bool boolean = false;
        if (conditions.Length > 0)
        {
            foreach (var condition in effectConditions)
            {
                if (condition.condName == flName
                    && condition.condTrue == flag)
                {
                    boolean = true;
                    break;
                }
            }
        }
        else
        {
            boolean = true;
        }
        return boolean;
    }
    public bool MatchActionsEffect(AI_condition cond) {
        bool boolean = false;
        foreach (var condition in effects) {
            if (condition.condName == cond.condName
                && condition.condTrue == cond.condTrue) {
                
                boolean = true;
                break;
            }
        }
        return boolean;
    }

    public enum ACTION {
        ATTACK,
        SUPPORT,
        NOTHING,
        SPECIFIC
    }
    public d_move move;
    public ACTION action;
}
*/
[System.Serializable]
public class o_BattleCharAI
{
    public AI_condition[] conditions;
    public enum TARG_MOVE {
        ON_PARTY,
        NOT_PARTY,
        SPECIFIC
    }
    public d_move move;
}
