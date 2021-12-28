using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_BackDropFade : s_Singleton<s_BackDropFade>
{
    public Image fadeImg;
    public IEnumerator Fade(Color colour) {
        fadeImg.gameObject.SetActive(true);
        float t = 0;
        while (fadeImg.color != colour)
        {
            t += Time.unscaledDeltaTime;
            fadeImg.color = Color.Lerp(fadeImg.color, colour, t);
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime * 2);
        }
        fadeImg.gameObject.SetActive(false);
    }
}
