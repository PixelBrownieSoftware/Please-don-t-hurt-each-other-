using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Difficulty", menuName = "Difficulty setting")]
public class d_difficulty : ScriptableObject
{
    public o_BattleCharacterData[] potAllies;
    public o_BattleCharacterData[] potEnemies;

    public int min_allies = 1;
    public int max_allies = 1;
    public int min_foes = 1;
    public int max_foes = 1;

    public int min_turns = 5;
    public int max_turns = 8;

    public int difficultyShiftTurns = 5;
}
