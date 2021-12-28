using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public interface i_gameManager {
        IEnumerator OnStart();
}

public class s_GameSceneManager : s_Singleton<s_GameSceneManager>
{

    public AudioClip battleOst;

    public void Pause() {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public IEnumerator StartBattle()
    {
        yield return s_BackDropFade.instance.Fade( Color.black);
        Pause();
        yield return SceneManager.LoadSceneAsync("battle_scene", LoadSceneMode.Additive);
        yield return s_BackDropFade.instance.Fade(Color.clear);
        yield return new WaitForSecondsRealtime(0.1f);
        Resume();
        s_BGM.GetInstance().StartCoroutine(s_BGM.GetInstance().FadeInMusic(instance.battleOst));
    }
    public IEnumerator EndBattle()
    {
        s_BGM.GetInstance().StartCoroutine(s_BGM.GetInstance().FadeInMusic(null));
        yield return s_BackDropFade.instance.Fade(Color.black);
        Pause();
        yield return SceneManager.UnloadSceneAsync("battle_scene");
        yield return s_BackDropFade.instance.Fade(Color.clear);
        yield return new WaitForSecondsRealtime(0.1f);
        Resume();
        men_SelectStage.Show();
    }

}
