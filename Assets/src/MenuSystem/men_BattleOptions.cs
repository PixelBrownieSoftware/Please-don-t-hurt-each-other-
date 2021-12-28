using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class men_BattleOptions : s_Menu<men_BattleOptions>
{
    private o_BattleCharacter battleCharacter;

    public static void Show()
    {
        Open();
    }

    public static void Show(o_BattleCharacter bc) {
        Open();
        instance.battleCharacter = bc;
    }
    public static void Hide()
    {
        Close();
    }

    public void ActionMenu() {
        men_ActionMenu.Show(battleCharacter);
    }
    public void Conserve()
    {
        //men_ActionMenu.Show();
    }
    public void GoalsMenu()
    {
        men_goals.Show();
    }
    public void AnalyzeMenu()
    {
        //men_ActionMenu.Show();
    }
    public override void OnBackPressed()
    {
        throw new System.NotImplementedException();
    }
}
