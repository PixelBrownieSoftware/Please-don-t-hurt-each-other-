using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class o_BattleCharacterAI : MonoBehaviour
{
    public o_BattleCharAI[] characterAI;
    o_BattleCharacterStatus status;

    private Stack<d_move> GOAP = new Stack<d_move>();
    private o_BattleCharacter userBC;
    public s_Goal[] goals;


    private void Awake()
    {
        userBC = GetComponent<o_BattleCharacter>();
        status = GetComponent<o_BattleCharacterStatus>();
    }

    /*
    public void SetAI(d_encounterCharacter data) {

        characterAI = data.charDat.actions;
        m_conditions.Clear();
        {
            int i = 0;
            foreach (var variable in data.charDat.variable)
            {
                otBars[i].gameObject.SetActive(true);
                m_variables.Add(variable.name, variable.val);
                m_variablesMax.Add(variable.name, userBC.data.FindVariable(variable.name).maxVal);
            }
        }
        foreach (AI_condition cond in data.conditions)
        {
            m_conditions.Add(cond.condName, cond.condTrue);
        }
        
        foreach (o_BattleCharAI ai in characterAI)
        {
            foreach (AI_condition cond in ai.conditions)
            {
                if (m_conditions.ContainsKey(cond.condName))
                {
                    continue;
                }
                m_conditions.Add(cond.condName, cond.condTrue);
            }
        }
        //FindActionToFitGoal();
    }
    */
    public void SetAI()
    {
        
        characterAI = userBC.data.actions;
        //FindActionToFitGoal();
    }

    /*
    public void FindActionToFitGoal2()
    {
        AI_condition winCond = s_BattleController.instance.encounterGroup.winCond[0];
        if (tag == "Allies")
        {
            winCond = s_BattleController.instance.encounterGroup.winCond[0];
        }
        else if (tag == "Opposition")
        {
            winCond = s_BattleController.instance.encounterGroup.looseCond[0];
        }

        List<d_move> originList = new List<d_move>();
        List<d_move> openList = new List<d_move>();
        foreach (var ai in userBC.data.moves)
        {
            originList.Add(ai);
            openList.Add(ai);
        }
        s_Goal goal = goals[0];
        AI_condition[] conditions = goals[0].conditions;
        switch (goal.goalType)
        {
            case s_Goal.GOAL_TYPE.ESCAPE:
                d_move mov = openList.Find(x => x.moveType == d_move.MOVE_TYPE.ESCAPE);
                openList.Insert(0, mov);
                break;

            case s_Goal.GOAL_TYPE.STAT_CHECK:

                break;
        }

        while (openList.Count > 0)
        {
            d_move curAI = openList[0];
            //Check if the ai solves the condition
            foreach (var cond in conditions) {
                //switch () {}
            }
            
            void FoundAI()
            {
                openList.Clear();
                openList.AddRange(originList);
                openList.Remove(curAI);
                if (curAI.conditions != null)
                    if (curAI.conditions.Length > 0)
                        conditions = curAI.conditions;
                GOAP.Push(curAI);
            }
            openList.Remove(curAI);
        }
    }

    public void FindActionToFitGoal()
    {
        AI_condition winCond = s_BattleController.instance.encounterGroup.winCond[0];
        if (tag == "Allies") {
            winCond = s_BattleController.instance.encounterGroup.winCond[0];
        }
        else if (tag == "Opposition")
        {
            winCond = s_BattleController.instance.encounterGroup.looseCond[0];
        }

        List<o_BattleCharAI> originList = new List<o_BattleCharAI>();
        List<o_BattleCharAI> openList = new List<o_BattleCharAI>();
        foreach (var ai in characterAI) {
            originList.Add(ai);
            openList.Add(ai);
        }
        AI_condition condition = winCond;
        while (openList.Count > 0) {
            o_BattleCharAI curAI = openList[0];
            //Check if the ai solves the condition
            if (curAI.MatchActionsEffect(condition))
            {
                if (m_conditions.ContainsKey(condition.condName))
                {
                    if (curAI.MatchActionsCond(m_conditions[condition.condName], condition.condName))
                    {
                        print("Found ya");
                        FoundAI();
                        break;
                    }
                    else
                    {
                        FoundAI();
                    }
                }
                else
                {
                    FoundAI();
                }
            }
            void FoundAI()
            {
                openList.Clear();
                openList.AddRange(originList);
                openList.Remove(curAI);
                if(curAI.conditions != null)
                    if(curAI.conditions.Length > 0)
                        condition = curAI.conditions[0];
                //GOAP.Push(curAI);
            }
            openList.Remove(curAI);
        }
    }
    */


    public o_BattleCharAI[] GetAIs() {
        List<o_BattleCharAI> availAis = new List<o_BattleCharAI>();
        List<o_BattleCharacter> opp = new List<o_BattleCharacter>();
        List<o_BattleCharacter> allies = new List<o_BattleCharacter>();

        foreach (var ai in characterAI) {

            switch (ai.move.targetType)
            {
                case d_move.TARG_TYPE.SINGLE:
                    if (tag == "Opposition")
                    {
                        opp = s_B_Teams.instance.GetCharacters(TEAM_INDEX.ALLIES);
                        allies = s_B_Teams.instance.GetCharacters(TEAM_INDEX.OPPOSITION);
                    }
                    else if (tag == "Allies")
                    {
                        opp = s_B_Teams.instance.GetCharacters(TEAM_INDEX.OPPOSITION);
                        allies = s_B_Teams.instance.GetCharacters(TEAM_INDEX.ALLIES);
                    }
                    break;
            }

            bool goodMoveCost = false;
            int costFufilled = 0;
            foreach (d_move.condCost cond in ai.move.costs) {
                if (!status.HasVariable(cond.name)) {
                    break;
                }
                if (status.GetVariable(cond.name).val >= cond.number)
                {
                    costFufilled++;
                }
                else
                    break;
            }
            if (costFufilled == ai.move.costs.Length) {
                goodMoveCost = true;
            }
            if (goodMoveCost) {

                bool canDo = true;
                foreach (AI_condition cond in ai.conditions)
                {
                    bool isFufilled = false;

                    switch (cond.conditionType)
                    {
                        case AI_condition.COND_TYPE.CHECK_IF_CHAR_EXIST_OPP:
                            isFufilled = opp.Find(x => x.name == cond.condName) != null;
                            break;

                        case AI_condition.COND_TYPE.CHECK_IF_CHAR_EXIST_PARTY:
                            isFufilled = allies.Find(x => x.name == cond.condName) != null;
                            break;

                        case AI_condition.COND_TYPE.VAR_EQ:
                            isFufilled = status.GetVariable(cond.condName).val == cond.condNum;
                            break;

                        case AI_condition.COND_TYPE.VAR_GR:
                            isFufilled = status.GetVariable(cond.condName).val > cond.condNum;
                            break;

                        case AI_condition.COND_TYPE.VAR_LESS:
                            isFufilled = status.GetVariable(cond.condName).val < cond.condNum;
                            break;

                        case AI_condition.COND_TYPE.VAR_GR_EQ:
                            isFufilled = status.GetVariable(cond.condName).val >= cond.condNum;
                            break;

                        case AI_condition.COND_TYPE.VAR_LESS_EQ:
                            isFufilled = status.GetVariable(cond.condName).val <= cond.condNum;
                            break;
                    }
                    if (!isFufilled)
                    {
                        canDo = false;
                        break;
                    }
                }
                if (canDo)
                {
                    availAis.Add(ai);
                }
            }
        }


        return availAis.ToArray();
    }

    public o_BattleCharacter GetTarget(o_BattleCharAI aiCode) {

        foreach (var cond in aiCode.conditions) {
            if (cond.setTarget) {
                List<o_BattleCharacter> targets = new List<o_BattleCharacter>();
                switch (cond.conditionType)
                {
                    case AI_condition.COND_TYPE.CHECK_IF_CHAR_EXIST_PARTY:
                        if (tag == "Opposition")
                        {
                            targets = s_B_Teams.instance.GetCharacters(TEAM_INDEX.OPPOSITION).FindAll(x => x.name == cond.targName);
                        }
                        else if (tag == "Allies")
                        {
                            targets = s_B_Teams.instance.GetCharacters(TEAM_INDEX.ALLIES).FindAll(x => x.name == cond.targName);
                        }
                        return targets[Random.Range(0, targets.Count)];

                    case AI_condition.COND_TYPE.CHECK_IF_CHAR_EXIST_OPP:
                    if (tag == "Opposition")
                        {
                            targets = s_B_Teams.instance.GetCharacters(TEAM_INDEX.ALLIES).FindAll(x => x.name == cond.targName);
                        }
                        else if (tag == "Allies")
                        {
                            targets = s_B_Teams.instance.GetCharacters(TEAM_INDEX.OPPOSITION).FindAll(x => x.name == cond.targName);
                        }
                        return targets[Random.Range(0, targets.Count)];
                }
            }
        }
        return null;
    }

    /*
    if (GOAP.Count > 0)
    {
        ai = GOAP.Peek();
        bool cond_fufilled() {

            int fufilConditions = ai.conditions.Length;
            int conditions = 0;
            foreach (var cond in ai.conditions)
            {
                bool m_setFlag = false;
                switch (cond.conditionType)
                {
                    case AI_condition.COND_TYPE.BOOL:
                        m_setFlag = (m_conditions[cond.condName] == cond.condTrue);
                        break;

                    case AI_condition.COND_TYPE.VAR_EQ:

                        break;
                }
            }
            return false;
        }
        if (cond_fufilled()) {
            GOAP.Pop();
            if (GOAP.Count > 0)
                ai = GOAP.Peek();
        }
    }
    */
    public void Process()
    {
        o_BattleCharAI ai = null;
        o_BattleCharacter bc = null;
        if(characterAI.Length > 0)
        {
            o_BattleCharAI[] ais = GetAIs();
            if (ais.Length > 0)
            {
                ai = ais[Random.Range(0, ais.Length)];
            }
        }
        

        if (ai != null)
        {
            //print("Here to!");
            List<o_BattleCharacter> chr = new List<o_BattleCharacter>();
            switch (ai.move.targetType)
            {
                case d_move.TARG_TYPE.SINGLE:
                    bc = GetTarget(ai);
                    if (bc == null)
                    {
                        if (tag == "Opposition")
                        {
                            chr = s_B_Teams.instance.GetCharacters(TEAM_INDEX.ALLIES);
                            bc = chr[Random.Range(0, chr.Count)];
                        }
                        else if (tag == "Allies")
                        {
                            chr = s_B_Teams.instance.GetCharacters(TEAM_INDEX.OPPOSITION);
                            bc = chr[Random.Range(0, chr.Count)];
                        }
                    }
                    break;
                case d_move.TARG_TYPE.SELF:
                    bc = userBC;
                    break;
            }
            userBC.StartCoroutine(userBC.ExecuteMove(ai.move, bc));
        }
        else
        {
            //print("I got nothing, dude");
            userBC.StartCoroutine(userBC.DoNothing( ));
        }
    }
}
