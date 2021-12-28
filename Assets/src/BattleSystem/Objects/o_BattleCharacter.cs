using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MagnumFoundation2.System.Core;

public class o_BattleCharacter : MonoBehaviour
{
    public bool canParticipate = true;
    public int health, maxHealth;
    public o_BattleCharacterAI ai;
    public o_BattleCharacterData data;
    public o_BattleCharacterStatus status;
    public SpriteRenderer spriteRend;
    private Animator anim;
    public float moveRange = 10;

    public Slider hpBar;

    public AudioClip hurtSound;
    public AudioClip healSound;

    private void Awake()
    {
        anim = spriteRend.GetComponent<Animator>();
        ai = GetComponent<o_BattleCharacterAI>();
        status = GetComponent<o_BattleCharacterStatus>();
    }
    public void SetData(o_BattleCharacterData data)
    {
        this.data = data;
        if(spriteRend != null) {
            if (tag == "Allies")
            {
                spriteRend.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                spriteRend.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
        if(data.animation != null)
            anim.runtimeAnimatorController = data.animation;
        health = maxHealth = data.health;
        ai.SetAI();
        status.SetVariables(data);
    }

    /*
    public void SetData(d_encounterCharacter data) {
        this.data = data.charDat;
        spriteRend.sprite = this.data.sprite;
        health = maxHealth = data.charDat.health;
        ai.SetAI(data);
    }
    */

    public IEnumerator DoBlinkActionAnim() {

        for (int i = 0; i < 2; i++)
        {
            float t = 0;
            float spd = 13.6f;
            while (spriteRend.color != Color.black)
            {
                spriteRend.color = Color.Lerp(Color.white, Color.black, t);
                t += Time.deltaTime * spd;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            t = 0;
            while (spriteRend.color != Color.white)
            {
                spriteRend.color = Color.Lerp(Color.black, Color.white, t);
                t += Time.deltaTime * spd;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }

    public IEnumerator DoNothing()
    {
        yield return StartCoroutine(DoBlinkActionAnim());
        yield return new WaitForSeconds(0.2f);

        yield return s_B_TurnManager.instance.StartCoroutine(s_B_TurnManager.instance.EndTurn());
    }

    public IEnumerator SpawnObject(string animName, Vector3 position) {
        if (S_ObjectPooler.instance.SpawnObject(animName, position, Quaternion.identity).TryGetComponent(out O_FX fx))
        {
            while (!fx.CheckIfAnimDone())
            {
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        yield return new WaitForSeconds(Time.deltaTime);
    }

    public IEnumerator ExecuteMove(d_move move, o_BattleCharacter bc)
    {
        //print(name + " " + move.name);
        float timer = .1f;
        if (s_B_Teams.instance.ghost == this) {
            status.AddVariable("Magic", -move.magCost);
        }
        foreach (var costs in move.costs) {
            status.AddVariable(costs.name, -costs.number);
        }
        yield return StartCoroutine(DoBlinkActionAnim());
        yield return StartCoroutine(s_BattleController.instance.ShowNotification(move.name));

        yield return StartCoroutine(SpawnObject(move.animObj.name, bc.transform.position));

        Vector2 offset = new Vector2(0, Random.Range(-20, 20));
        switch (move.moveType)
        {
            case d_move.MOVE_TYPE.ATTACK:
                {
                    s_soundmanager.GetInstance().PlaySound(hurtSound);
                    if (S_ObjectPooler.instance.SpawnObject("dmg_numbers", bc.transform.position + (Vector3)offset, Quaternion.identity).TryGetComponent(out O_DMGFX fx))
                    {
                        fx.SetColour(Color.red);
                        fx.ChangeName(""+move.power, "");
                        while (fx.CheckIfAnimDone())
                        {
                            yield return new WaitForSeconds(Time.deltaTime);
                        }
                    }
                }
                bc.health -= move.power;
                for (int i = 0; i < 3; i++) {
                    bc.spriteRend.color = Color.red;
                    yield return new WaitForSeconds(timer);
                    bc.spriteRend.color = Color.white;
                    yield return new WaitForSeconds(timer);
                }
                bc.spriteRend.color = Color.white;
                break;
            case d_move.MOVE_TYPE.STATUS:
                foreach (var flagInc in move.flagInc)
                {
                    switch (flagInc.name)
                    {
                        case "Heal":
                            bc.health += flagInc.val;
                            s_soundmanager.GetInstance().PlaySound(healSound);
                            {
                                if (S_ObjectPooler.instance.SpawnObject("dmg_numbers", bc.transform.position + (Vector3)offset, Quaternion.identity).TryGetComponent(out O_DMGFX fx))
                                {
                                    fx.SetColour(Color.green);
                                    fx.ChangeName("+" + flagInc.val, "");
                                }
                            }
                            bc.health = Mathf.Clamp(bc.health, 0, bc.maxHealth);
                            bc.spriteRend.color = Color.green;
                            yield return new WaitForSeconds(timer);
                            bc.spriteRend.color = Color.white;
                            yield return new WaitForSeconds(timer);
                            break;

                        default:
                            if (bc.ai != null)
                            {
                                if (bc.status.HasVariable(flagInc.name))
                                {
                                    bc.status.AddVariable(flagInc.name, flagInc.val);
                                    {
                                        if (S_ObjectPooler.instance.SpawnObject("dmg_numbers", bc.transform.position + (Vector3)offset, Quaternion.identity).TryGetComponent(out O_DMGFX fx))
                                        {
                                            fx.SetColour(Color.white);
                                            fx.ChangeName(" " + flagInc.val, flagInc.name);
                                        }
                                    }
                                    for (int i = 0; i < 3; i++)
                                    {
                                        bc.spriteRend.color = Color.cyan;
                                        yield return new WaitForSeconds(timer);
                                        bc.spriteRend.color = Color.white;
                                        yield return new WaitForSeconds(timer);
                                    }
                                    bc.spriteRend.color = Color.white;
                                    //print(move.flagInc + " " + bc.ai.variables[move.flagInc]);
                                }
                            }
                            break;
                    }
                }
                break;
        }
        yield return new WaitForSeconds(0.2f);
        s_B_TurnManager.instance.StartCoroutine(s_B_TurnManager.instance.EndTurn());
    }
    
    
    void Update()
    {
        hpBar.value = ((float)health / (float)maxHealth);
    }
}
