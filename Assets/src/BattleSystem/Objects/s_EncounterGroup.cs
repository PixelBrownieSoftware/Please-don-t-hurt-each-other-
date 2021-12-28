using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class d_encounterCharacter {
    public o_BattleCharacterData charDat;
    public AI_condition[] conditions;
    public d_CharacterGoalData goals;
}

[System.Serializable]
public class d_encounterCondition {
    public TEAM_INDEX teamIndex;
    public bool all = true;
    //Who this rule applies to
    public int[] indexes;
    //The Conditions
    public AI_condition[] cond;
    public CHECK_COND checkCondition = CHECK_COND.CHECK_VARIABLES;
    public enum CHECK_COND {
        CHECK_VARIABLES,
        CHECK_IF_DEFEATED
    }
}

[CreateAssetMenu(fileName = "New encounter group", menuName = "Encounter group")]
public class s_EncounterGroup : ScriptableObject
{
    public d_encounterCharacter[] allies;
    public d_encounterCharacter[] opponents;
    public AI_condition[] winCond;
    public d_encounterCondition[] winCondN;
    public AI_condition[] looseCond;
    public d_encounterCondition[] looseCondN;
}
