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






    
}
