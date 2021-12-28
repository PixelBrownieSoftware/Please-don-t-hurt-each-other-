using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class men_Target : s_Menu<men_Target>
{
    public Button[] buttonsAllies;
    public Button[] buttonsOppos;
    private List<o_BattleCharacter> m_Ally_targets = new List<o_BattleCharacter>();
    private List<o_BattleCharacter> m_Oppos_targets = new List<o_BattleCharacter>();
    private d_move move;

    public void SetTarget(o_BattleCharacter bc) {

    }

    public static void Hide() {
        Close();
    }
    
    public static void Show(d_move move) {
        Open();
        instance.move = move;
        instance.m_Ally_targets = s_B_Teams.instance.GetCharacters( TEAM_INDEX.ALLIES);
        instance.m_Oppos_targets = s_B_Teams.instance.GetCharacters( TEAM_INDEX.OPPOSITION);
        
        for (int i = 0; i < instance.buttonsAllies.Length; i++)
        {
            instance.buttonsAllies[i].onClick.RemoveAllListeners();
            instance.buttonsAllies[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < instance.buttonsOppos.Length; i++)
        {
            instance.buttonsAllies[i].onClick.RemoveAllListeners();
            instance.buttonsOppos[i].gameObject.SetActive(false);
        }
        
        for (int i = 0; i < instance.m_Ally_targets.Count; i++)
        {
            var button = instance.buttonsAllies[i];
            button.gameObject.SetActive(true);
            button.onClick.RemoveAllListeners();
            o_BattleCharacter bc = instance.m_Ally_targets[i];
            button.transform.position = Camera.main.WorldToScreenPoint(bc.transform.position);
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = instance.m_Ally_targets[i].name;
            button.onClick.AddListener(delegate {
                s_B_TurnManager.instance.StartAction().StartCoroutine(s_B_TurnManager.instance.StartAction().ExecuteMove(instance.move, bc));
                Close();
                men_ActionMenu.Hide();
                men_BattleOptions.Hide();
            });
        }
        for (int i = 0; i < instance.m_Oppos_targets.Count; i++)
        {
            var button = instance.buttonsOppos[i];
            button.gameObject.SetActive(true);
            button.onClick.RemoveAllListeners();
            o_BattleCharacter bc = instance.m_Oppos_targets[i];
            button.transform.position = Camera.main.WorldToScreenPoint(bc.transform.position);
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = instance.m_Oppos_targets[i].name;
            button.onClick.AddListener(delegate {
                s_B_TurnManager.instance.StartAction().StartCoroutine(s_B_TurnManager.instance.StartAction().ExecuteMove(instance.move, bc));
                Close();
                men_ActionMenu.Hide();
                men_BattleOptions.Hide();
            });
        }
    }

    /*
    IEnumerator ExecuteMove(o_BattleCharacter bc)
    {
        print(s_B_TurnManager.instance.StartAction().name + " " + move.name);
        switch (move.moveType)
        {
            case d_move.MOVE_TYPE.ATTACK:
                bc.health -= move.power;
                break;
            case d_move.MOVE_TYPE.STATUS:
                switch (move.flagInc)
                {
                    case "Heal":
                        bc.health += move.power;
                        bc.health = Mathf.Clamp(bc.health, 0, bc.maxHealth);
                        break;
                }
                break;
        }
        if (bc.ai != null)
        {
            if (bc.ai.variables.Find(x => x.name == move.flagInc) != null)
            {
                bc.ai.variables.Find(x => x.name == move.flagInc).variable += move.power;
            }
        }
        print("Turn ended, you mother fucker");
        yield return new WaitForSeconds(0.1f);
        print("Turn ended, you mother fucker2");
        s_B_TurnManager.instance.EndTurn();
    }
    */

    public override void OnBackPressed()
    {
        men_ActionMenu.Show();
    }
}
