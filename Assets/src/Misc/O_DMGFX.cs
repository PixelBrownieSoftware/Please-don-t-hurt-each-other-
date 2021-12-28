using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class O_DMGFX : O_FX, I_Spawnable
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private TextMeshProUGUI textMesh;
    [SerializeField]
    private TextMeshProUGUI textMeshNumber;
    
    public void SetColour(Color colour)
    {
        textMesh.color = new Color(colour.r, colour.g, colour.b, textMesh.color.a);
    }

    public void ChangeName(string numbers, string varName) {
        textMesh.text = numbers;
        textMeshNumber.text = varName;
    }
}
