using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;

public interface I_B_system {
    void Process();
    void Backtrack();
}

public class s_BattleAction {
    public o_BattleCharacter user;
    public o_BattleCharacter target;
    //Move
    //Press turn flag
}
public enum TEAM_INDEX
{
    ALL = -1,
    ALLIES,
    OPPOSITION,
    GHOST
}

public class s_BattleController : s_Singleton<s_BattleController>
{
    public s_BattleAction battleAction;

    public int turnPoints = 0;
    public int remainingTurns = 0;
    int initTurns = 0;

    public o_BattleCharacter currentCharacter;
    public int difficultyNumber = 0;
    public int difficultyIndex = 0;

    s_B_Teams teamsObj;
    s_B_TurnManager turnObj;
    s_B_ProcessAction processObj;

    public Image notifImage;
    //public TextMeshProUGUI notifText;
    //public TextMeshProUGUI turnCountText;
    //public TextMeshProUGUI turnRemainderText;

    //public TextMeshProUGUI ghostMPText;
    public Slider ghostMP;

    public enum BATTLE_STATEMACHINE {
        NEW_ROUND,
        IDLE,
        CHARACTER_SELECT,
        PROCESSING,
        FINISH_TURN,
        END_BATTLE
    }
    public BATTLE_STATEMACHINE battleState;

    public TEAM_INDEX teamIndex;
    public d_difficulty[] difficulties;

    public void IncrementTeamIndex() {

        switch (teamIndex) {
            case TEAM_INDEX.ALLIES:
                teamIndex = TEAM_INDEX.OPPOSITION;
                break;
            case TEAM_INDEX.OPPOSITION:
                s_B_Teams.instance.ghost.status.AddVariable("Magic", 4);
                teamIndex = TEAM_INDEX.GHOST;
                break;
            case TEAM_INDEX.GHOST:
                turnPoints++;
                remainingTurns--;
                teamIndex = TEAM_INDEX.ALLIES;
                break;
        }
    }

    public bool CheckIfLooseConditionMet(string condName)
    {
        switch (condName)
        {
        }
        return false;
    }

    /*
    public bool CheckIfConditionMet2(d_encounterCondition cond)
    {
        List<o_BattleCharacter> charas = new List<o_BattleCharacter>();
        switch (cond.teamIndex)
        {
            case TEAM_INDEX.ALLIES:
                charas = s_B_Teams.instance.GetCharacters(TEAM_INDEX.ALLIES);
                break;
        }
        switch (cond.checkCondition)
        {

            case d_encounterCondition.CHECK_COND.CHECK_IF_DEFEATED:
                foreach (var chara in charas)
                {
                    if (chara.health <= 0)
                    {
                        return true;
                    }
                }
                break;

            case d_encounterCondition.CHECK_COND.CHECK_VARIABLES:

                int count = charas.Count;
                int index = 0;
                foreach (var chara in charas)
                {
                    if (chara.ai.conditions["RunAway"] == true)
                    {
                        print(chara.ai.conditions["RunAway"]);
                        index++;
                    }
                }
                if (index == count)
                    return true;
                break;
        }
        return false;
    }

    public bool CheckIfWinConditionMet(string condName) {
        switch (condName) {
            case "RunAway":
                List<o_BattleCharacter> allies = s_B_Teams.instance.GetCharacters(TEAM_INDEX.ALLIES);
                int count = allies.Count;
                int index = 0;
                foreach (var chara in allies)
                {
                    if (chara.ai.conditions["RunAway"] == true)
                    {
                        print(chara.ai.conditions["RunAway"]);
                        index++;
                    }
                }
                if(index == count)
                    return true;
                break;
        }
        return false;
    }
    */

    protected new void Awake()
    {
        base.Awake();
        teamsObj = GetComponent<s_B_Teams>();
        turnObj = GetComponent<s_B_TurnManager>();
        processObj = GetComponent<s_B_ProcessAction>();
    }

    public IEnumerator ShowNotification(string moveName) {

        //notifText.gameObject.SetActive(true);
        notifImage.gameObject.SetActive(true);
        //notifText.text = moveName;
        float t = 0;
        float spd = 4.75f;
        while (notifImage.color != Color.white)
        {
            notifImage.color = Color.Lerp(Color.clear, Color.white, t);
            //notifText.color = Color.Lerp(Color.clear, Color.white, t);
            t += Time.deltaTime * spd;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        t = 0;
        yield return new WaitForSeconds(0.25f);
        while (notifImage.color != Color.clear)
        {
            notifImage.color = Color.Lerp(Color.white, Color.clear, t);
           // notifText.color = Color.Lerp(Color.white, Color.clear, t);
            t += Time.deltaTime * spd;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        //notifText.gameObject.SetActive(false);
        notifImage.gameObject.SetActive(false);
    }
    
    private void Update()
    {
        //turnCountText.text = "" + turnPoints;
        //turnRemainderText.text = "" + remainingTurns + "/" + initTurns;
        o_BattleCharacterStatus.battle_var ghostMP = teamsObj.ghost.status.GetVariable("Magic");
        //ghostMPText.text = "" + ghostMP.val + "/" + ghostMP.max;

        if (turnObj.StartAction() != null)
        {
            currentCharacter = turnObj.StartAction();
        }
        else {
            currentCharacter = null;
        }
        switch (battleState)
        {
            case BATTLE_STATEMACHINE.NEW_ROUND:
                turnObj.StartCoroutine(turnObj.AddRoundCharacters(teamsObj.GetCharacters(teamIndex)));
                battleState = BATTLE_STATEMACHINE.IDLE;
                break;

            case BATTLE_STATEMACHINE.CHARACTER_SELECT:
                if (teamIndex != TEAM_INDEX.GHOST)
                    turnObj.StartAction().ai.Process();
                else
                    men_BattleOptions.Show(turnObj.StartAction());
                battleState = BATTLE_STATEMACHINE.PROCESSING;
                break;

            case BATTLE_STATEMACHINE.FINISH_TURN:
                bool hasLost = false;
                foreach (var chara in s_B_Teams.instance.GetCharacters(TEAM_INDEX.ALL))
                {
                    if (chara.health <= 0)
                    {
                        hasLost = true;
                    }
                }
                if (hasLost)
                {
                    battleState = BATTLE_STATEMACHINE.IDLE;
                    s_GameSceneManager.instance.StartCoroutine(s_GameSceneManager.instance.EndBattle());
                }
                else if (remainingTurns > 0)
                {
                    if (s_B_TurnManager.instance.fullTurnIcons == 0)
                    {
                        instance.IncrementTeamIndex();
                        instance.battleState = BATTLE_STATEMACHINE.NEW_ROUND;
                    }
                    else
                    {
                        battleState = BATTLE_STATEMACHINE.CHARACTER_SELECT;
                    }
                }
                else if (remainingTurns == 0)
                {
                    if (s_B_TurnManager.instance.fullTurnIcons == 0) {

                        battleState = BATTLE_STATEMACHINE.IDLE;
                        if (difficultyIndex < difficulties.Length)
                        {
                            difficultyNumber++;
                            if (difficultyNumber > difficulties[difficultyIndex].difficultyShiftTurns)
                            {
                                difficultyIndex++;
                                difficultyNumber = 0;
                            }
                        }
                        StartCoroutine(NextPart());
                    }
                   // s_GameSceneManager.instance.StartCoroutine(s_GameSceneManager.instance.EndBattle());
                }
                break;
        }
    }

    public IEnumerator NextPart() {

        yield return s_BackDropFade.instance.Fade(Color.black);
        SetTurnStuff();
        yield return s_BackDropFade.instance.Fade(Color.clear);
        battleState = BATTLE_STATEMACHINE.NEW_ROUND;
    }

    public void SetTurnStuff() {

        remainingTurns = Random.Range(difficulties[difficultyIndex].min_turns, difficulties[difficultyIndex].max_turns);
        initTurns = remainingTurns;
        s_B_Teams.instance.AddMembers(difficulties[difficultyIndex]);
    }

    void Start()
    {
        SetTurnStuff();
    }
}
