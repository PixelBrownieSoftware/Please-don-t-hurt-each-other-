using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_B_TurnManager : s_Singleton<s_B_TurnManager>
{
    public int fullTurnIcons;
    public GameObject[] turnIconsObj;
    private Image[] turnIcons;
    private Animator[] turnIconAnims;

    public Sprite allies_Icon;
    public Sprite opponent_icon;
    public Sprite ghost_icon;

    private new void Awake()
    {
        base.Awake();
        int index = 0;
        turnIconAnims = new Animator[turnIconsObj.Length];
        turnIcons = new Image[turnIconsObj.Length];
        foreach (var turnIcon in turnIconsObj) {
            turnIcons[index] = turnIcon.transform.GetChild(0).GetComponent<Image>();
            turnIcons[index].color = Color.clear;
            turnIconAnims[index] = turnIcons[index].GetComponent<Animator>();
            index++;
        }
    }

    public IEnumerator TurnIconUp(bool show, int ind) {
        switch (s_BattleController.instance.teamIndex) {
            case TEAM_INDEX.ALLIES:
                turnIcons[ind].sprite = allies_Icon;
                break;

            case TEAM_INDEX.GHOST:
                turnIcons[ind].sprite = ghost_icon;
                break;

            case TEAM_INDEX.OPPOSITION:
                turnIcons[ind].sprite = opponent_icon;
                break;
        }
        if (show)
        {
            turnIcons[ind].color = Color.white;
            turnIconAnims[ind].Play("turn_icon_appear");
            yield return new WaitForSeconds(0.25f);
        }
        else
        {
            turnIconAnims[ind].Play("turn_icon_disappear");
            yield return new WaitForSeconds(0.25f);
            turnIcons[ind].color = Color.clear;
        }
    }

    public IEnumerator EndTurn()
    {
        fullTurnIcons--;

        yield return StartCoroutine(TurnIconUp(false, fullTurnIcons));

        o_BattleCharacter bc = turnOrder.Dequeue();
        turnOrder.Enqueue(bc);
        //print("Next up is " + turnOrder.Peek().name);
        s_BattleController.instance.battleState = s_BattleController.BATTLE_STATEMACHINE.FINISH_TURN;
    }

    public Queue<o_BattleCharacter> turnOrder = new Queue<o_BattleCharacter>();

    public IEnumerator AddRoundCharacters(List<o_BattleCharacter> battleCharacters) {
        turnOrder.Clear();
        foreach (var cha in battleCharacters)
        {
            turnOrder.Enqueue(cha);
            for (int i = 0; i < cha.data.turnIcons; i++)
            {
                yield return StartCoroutine(TurnIconUp(true, fullTurnIcons));
                fullTurnIcons += 1;
            }
        }
        s_BattleController.instance.battleState = s_BattleController.BATTLE_STATEMACHINE.CHARACTER_SELECT;
    }

    public o_BattleCharacter StartAction() {
        if (turnOrder.Count == 0)
            return null;
        return turnOrder.Peek();
    }
}
