using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the process of actions
public class s_B_ProcessAction : MonoBehaviour
{
    public o_BattleCharacter battleCharacter;

    public void Process()
    {
        if (battleCharacter != null) {
            if (battleCharacter.ai != null) {
                battleCharacter.ai.Process();
            }
        }
    }
}
