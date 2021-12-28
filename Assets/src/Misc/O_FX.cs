using System.Collections;
using MagnumFoundation2.System.Core;
using UnityEngine;

public class O_FX : MonoBehaviour, I_Spawnable
{
    [SerializeField]
    private Animator anim;

    public void PlaySound(AudioClip playSound) {
        s_soundmanager.GetInstance().PlaySound(playSound);
    }
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public bool CheckIfAnimDone() {
        return anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1;
    }

    void I_Spawnable.OnSpawn() {
        anim.Play(0);
    }
}
