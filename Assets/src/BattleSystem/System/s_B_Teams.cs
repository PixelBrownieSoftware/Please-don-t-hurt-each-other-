using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_B_Teams : s_Singleton<s_B_Teams>
{
    public List<o_BattleCharacter> allies;
    public List<o_BattleCharacter> opposition;
    public o_BattleCharacter ghost;
    public o_BattleCharacterData ghostData;

    public GameObject alliesObj;
    public GameObject opposObj;
    public GameObject ghostObj;

    private o_BattleCharacter[] allySlots;
    private o_BattleCharacter[] opposSlots;

    public void ResetCharacters() {
        allies.Clear();
        opposition.Clear();
        foreach (var ally in allySlots) {
            ally.status.ResetVariables();
            ally.gameObject.SetActive(false);
        }
        foreach (var oppos in opposSlots)
        {
            oppos.status.ResetVariables();
            oppos.gameObject.SetActive(false);
        }
    }

    private new void Awake()
    {
        base.Awake(); 
        allySlots = alliesObj.GetComponentsInChildren<o_BattleCharacter>();
        opposSlots = opposObj.GetComponentsInChildren<o_BattleCharacter>();
        foreach (var ally in allySlots)
        {
            ally.hpBar.gameObject.SetActive(false);
        }
        foreach (var oppos in opposSlots)
        {
            oppos.hpBar.gameObject.SetActive(false);
        }
    }

    public List<o_BattleCharacter> GetCharacters(TEAM_INDEX index) {
        switch (index) {
            case (TEAM_INDEX)(-1):
                List<o_BattleCharacter> allMembers = new List<o_BattleCharacter>();
                allMembers.AddRange(allies);
                allMembers.AddRange(opposition);
                return allMembers;

            case TEAM_INDEX.ALLIES:
                return allies;
            case TEAM_INDEX.OPPOSITION:
                return opposition;
            case TEAM_INDEX.GHOST:
                List<o_BattleCharacter> ghosts = new List<o_BattleCharacter>();
                ghosts.Add(ghost);
                return ghosts;
        }
        return null;
    }

    public void AddMembers(d_difficulty diff) {

        int allyLeng = Random.Range(diff.min_allies, diff.max_allies + 1);
        int foesLeng = Random.Range(diff.min_foes, diff.max_foes + 1);

        ResetCharacters();
        allies = new List<o_BattleCharacter>();
        opposition = new List<o_BattleCharacter>();

        o_BattleCharacterData prevChar = null;
        for (int i =0; i < allyLeng; i++) {
            allySlots[i].gameObject.SetActive(true);
            o_BattleCharacterData cha = diff.potAllies[Random.Range(0, diff.potAllies.Length)];

            if (cha == prevChar && diff.potAllies.Length > 1) {
                List<o_BattleCharacterData> dat = new List<o_BattleCharacterData>(diff.potAllies);
                dat.Remove(prevChar);
                cha = dat[Random.Range(0, dat.Count)];
            }
            prevChar = cha;

            allySlots[i].SetData(cha);
            allySlots[i].name = cha.name;
            allies.Add(allySlots[i]);
            allySlots[i].hpBar.gameObject.SetActive(true);
        }
        prevChar = null;
        for (int i = 0; i < foesLeng; i++)
        {
            opposSlots[i].gameObject.SetActive(true);
            o_BattleCharacterData cha = diff.potEnemies[Random.Range(0, diff.potEnemies.Length)];

            if (cha == prevChar && diff.potEnemies.Length > 1)
            {
                List<o_BattleCharacterData> dat = new List<o_BattleCharacterData>(diff.potEnemies);
                dat.Remove(prevChar);
                cha = dat[Random.Range(0, dat.Count)];
            }
            prevChar = cha;
            
            opposSlots[i].SetData(cha);
            opposSlots[i].name = cha.name;
            opposition.Add(opposSlots[i]);
            opposSlots[i].hpBar.gameObject.SetActive(true);
        }
        ghost.SetData(ghostData);
    }

}
