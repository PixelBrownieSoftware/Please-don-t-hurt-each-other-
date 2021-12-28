using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class men_SelectStage : s_BasicMenu<men_SelectStage>
{
    public AudioClip OST;

    public static void Appear() {
        Show();

        s_BGM.GetInstance().StartCoroutine(s_BGM.GetInstance().FadeInMusic(instance.OST, 0.8f));
    }

    public void SelectStage() {
        s_GameSceneManager.instance.StartCoroutine(s_GameSceneManager.instance.StartBattle());
        Close();
    }
    
}
