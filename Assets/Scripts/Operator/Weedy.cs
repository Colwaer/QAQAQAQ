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
        cost = 3;
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






    public Weedy(float attack, float defend, float magicDamage, float magicDefend, float maxHelath, float attackDistance)
            : base(attack, defend, magicDamage, magicDefend, maxHelath, attackDistance)
    {

    }
}
