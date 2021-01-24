﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CostManager : Singleton<CostManager>
{
    private float m_maxCost = 99;
    private float m_cost;
    private float m_growSpeed;
    private float m_growVal;
    private float m_timer = 0;
    public Action<float> OnCostChanged;
    
    private void Update()
    {
        if(m_timer > m_growVal)
        {
            Add(m_growSpeed);
            m_timer = 0;
        }
        else m_timer += Time.deltaTime;
    }
    public bool Pay(float price)
    {
        if(price < 0)
        {
            Debug.LogError("Invalid Cost");
            return false;
        }
        if(price > m_cost) return false;
        m_cost += price;
        OnCostChanged(m_cost);
        return true;
    }
    public bool Add(float delta)
    {
        if(delta < 0)
        {
            Debug.LogError("Invalid Addition");
            return false;
        }
        m_cost = Mathf.Min(m_maxCost, delta + m_cost);
        OnCostChanged(m_cost);
        return true;
    }
    public void Init(float growSpeed,float growVal,float beginCost = 0,float maxCost = 99)
    {
        m_maxCost = maxCost;
        m_growSpeed = growSpeed;
        m_growVal = growVal;
        m_cost = beginCost;
        m_maxCost = maxCost;
    }
}
