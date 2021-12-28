using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;

public class o_BattleCharacterStatus : MonoBehaviour
{
    [System.Serializable]
    public class battle_var {
        public battle_var(s_BattleCharacterVariable varia) {
            name = varia.name;
            max = varia.maxVal;
            min = varia.minVal;
            val = varia.val;
        }
        public string name;
        public int val, max, min; 
    }
    public List<battle_var> m_variables = new List<battle_var>();
    public GameObject[] othBarsObjects;
    Slider[] otBars;
    //TextMeshProUGUI[] otTextBars;

    private void Awake()
    {
        otBars = new Slider[othBarsObjects.Length];
        //otTextBars = new TextMeshProUGUI[othBarsObjects.Length];
        int i = 0;
        foreach (var otherBars in othBarsObjects)
        {
            otBars[i] = otherBars.GetComponent<Slider>();
            //otTextBars[i] = otherBars.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            i++;
            otherBars.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        for (int i =0; i < m_variables.Count; i++)
        {
            var a = m_variables[i];
            otBars[i].value = ((float)a.val / (float)a.max);
            //otTextBars[i].text = a.name;
        }
    }

    public void ResetVariables() {
        m_variables.Clear();
    }

    public void AddVariable(string nameVar, int add)
    {
        int num = m_variables.FindIndex(x => x.name == nameVar);
        m_variables[num].val += add;
        m_variables[num].val = Mathf.Clamp(m_variables[num].val, m_variables[num].min, m_variables[num].max);
    }
    public battle_var GetVariable(string nameVar)
    {
        return m_variables.Find(x => x.name == nameVar);
    }

    public bool HasVariable(string nameVar) {
        return m_variables.Find(x => x.name == nameVar) != null;
    }

    public void SetVariables(o_BattleCharacterData data) {

        foreach (var otherBars in othBarsObjects)
        {
            otherBars.gameObject.SetActive(false);
        }
        m_variables.Clear();
        {
            int i = 0;
            foreach (var variable in data.variable)
            {
                battle_var newVariable = new battle_var(variable);
                m_variables.Add(newVariable);
                otBars[i].gameObject.SetActive(true);
            }
        }
    }
}
