using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class Weedy : BaseOperator
{
    public GameObject weedyDirChoosePanelPre;

    GameObject weedyDirChoosePanel;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    void Init()
    {
        cost = 3;
        health = 100f;
        m_maxHelath = 100f;
        m_attack = 20f;
        m_defend = 10f;
        m_magicDamage = 15f;
        m_magicDefend = 10f;
        maxEnergy = 100f;
        attackDelay = 0.6f;
        specialAttackDelay = 0.5f;
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
    protected override void SpecialAttackDetail()
    {
        foreach(BaseEnemy item in enemiesInAttackAreas)
        {
            item.Hurt(m_magicDamage, AttackKind.Magic);
            item.Dizzy(2.0f);
        }
    }
    




}
