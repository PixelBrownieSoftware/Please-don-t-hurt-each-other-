using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class men_goals : s_Menu<men_goals>
{
    public GameObject wins;
    public GameObject losses;
    private TextMeshProUGUI[] winsTexts;
    private TextMeshProUGUI[] lossesTexts;

    private new void Awake()
    {
        base.Awake();
        winsTexts = wins.transform.GetChild(2).transform.GetComponentsInChildren<TextMeshProUGUI>();
        lossesTexts = losses.transform.GetChild(2).transform.GetComponentsInChildren<TextMeshProUGUI>();
    }

    public string GetShow(string stuff) {
        string desc = "* ";
        switch (stuff) {
            case "RunAway":
                desc += "All allies escape";
                break;
        }
        switch (stuff)
        {
            case "KillFoes":
                desc += "Someone dies";
                break;
        }
        return desc + ".";
    } 

    public static void Show()
    {
        Open();
        /*
        for (int i = 0; i < s_BattleController.instance.encounterGroup.winCond.Length; i++) {
            instance.winsTexts[i].text = instance.GetShow(s_BattleController.instance.encounterGroup.winCond[i].condName);
        }
        for (int i = 0; i < s_BattleController.instance.encounterGroup.looseCond.Length; i++)
        {
            instance.lossesTexts[i].text = instance.GetShow(s_BattleController.instance.encounterGroup.looseCond[i].condName);
        }
        */
    }

    public override void OnBackPressed()
    {
        Close();
    }
}
