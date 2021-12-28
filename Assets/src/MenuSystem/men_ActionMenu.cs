using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class men_ActionMenu : s_Menu<men_ActionMenu>
{
    private List<d_move> skills = new List<d_move>();
    public Button[] buttons;

    public static void Show(o_BattleCharacter battleChar)
    {
        Open();
        instance.skills = battleChar.data.moves.ToList();
        instance.CreateSkillMenu();
    }
    public static void Hide()
    {
        Close();
    }

    private void CreateSkillMenu()
    {
        for (int i = 0; i < instance.buttons.Length; i++)
        {
            var b = instance.buttons[i];
            b.onClick.RemoveAllListeners();
            if (i < instance.skills.Count)
            {
                int ind = i;
                b.gameObject.SetActive(true);
                b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = instance.skills[i].name + " "  + instance.skills[i].magCost + " MP";
                if (s_B_Teams.instance.ghost.status.GetVariable("Magic").val >= instance.skills[i].magCost) {
                    b.onClick.AddListener(
                        delegate {
                            if (instance.skills[ind].targetType == d_move.TARG_TYPE.SELF) {
                                s_B_TurnManager.instance.StartAction().StartCoroutine(s_B_TurnManager.instance.StartAction().ExecuteMove(instance.skills[ind], s_B_Teams.instance.ghost));
                                Hide();
                                men_BattleOptions.Hide();
                            } else {
                                men_Target.Show(instance.skills[ind]);
                            }

                             }
                    );
                }
            }
            else
            {
                b.gameObject.SetActive(false);
            }
        }
    }

    public static void Show()
    {
        Open();
    }

    public override void OnBackPressed()
    {
        men_BattleOptions.Show();
    }
}