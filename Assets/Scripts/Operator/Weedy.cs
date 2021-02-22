using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class Weedy : BaseOperator
{
    public GameObject weedyDirChoosePanelPre;

    public AnimationClip specialAttackIdleClip;
    public AnimationClip idleClip;

    GameObject weedyDirChoosePanel;


    

    protected override void Awake()
    {
        base.Awake();
        
    }
    private void Start()
    {
        Init();
    }
    void Init()
    {
        cost = 2;
        health = 100f;
        m_maxHelath = 100f;
        m_attack = 20f;
        m_defend = 10f;
        m_magicDamage = 15f;
        m_magicDefend = 10f;
        maxEnergy = 100f;
        attackDelay = 0.6f;
        
    }

    public override void ShowDirChoosePanel()
    {
        weedyDirChoosePanel = Instantiate(weedyDirChoosePanelPre);
        //Debug.Log("enter");
        ShowAttackArea();

    }
    public override void OffShowDirChoosePanel()
    {
        if (weedyDirChoosePanel != null)
            Destroy(weedyDirChoosePanel);
        OffShowAttackArea();
    }
    protected override void TimePass_EnergyAdd()
    {
        CurrentEnergy += Time.deltaTime;
    }
    protected override void OnAttack_EnergyAdd()
    {
        CurrentEnergy += 30f;
    }
    protected override void OnHurt_EnergyAdd()
    {
        CurrentEnergy += 10f;
    }
    protected override void SpecialAttackDetail1()
    {
        //animatorOverrideController["WeedySpecialAttack1"] = specialAttack1Clip;

        CurrentEnergy = 0;

        specialAttackDelay = 0.5f;

        RemoveNull();
        for (int i = 0; i < enemiesInAttackAreas.Count; i++)
        {
            if (enemiesInAttackAreas[i] != null)
            {
                enemiesInAttackAreas[i].Hurt(m_magicDamage, AttackKind.Magic);
                enemiesInAttackAreas[i].Dizzy(2.0f);
            }
        }
    }
    protected override void SpecialAttackDetail2()
    {
        //animatorOverrideController["WeedySpecialAttack1"] = specialAttack2Clip;

        specialAttackDelay = 0.65f;

        m_attackInterval = 1.5f;

        if (CurrentEnergy >= maxEnergy)
        {
            Debug.Log("switch animation");

            //animatorOverrideController["WeedySpecialAttack1"] = specialAttack2Clip;
            animatorOverrideController["WeedyIdle"] = specialAttackIdleClip;
        }

        if (CurrentEnergy >= 0)
        {
            forceSpecialAttack = true;
            CurrentEnergy -= 15f;
        }
        else
        {
            m_attackInterval = 1.0f;
            animatorOverrideController["WeedyIdle"] = idleClip;
            forceSpecialAttack = false;
        }
        

        isAttacking = false;
        isIdling = false;
        isSpecialAttacking = true;
        animator.SetBool("isIdling", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isSpecialAttacking", true);

        

        RemoveNull();
        if (currentAttackTarget != null)
        {
            
            currentAttackTarget.Hurt(m_attack, AttackKind.Physics);
            currentAttackTarget.Hurt(m_attack, AttackKind.Magic);
            //currentAttackTarget.Hurt(3.0f, AttackKind.Real);
            currentAttackTarget.Dizzy(0.5f);
        }
        

    }




}
