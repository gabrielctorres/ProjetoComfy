using System;
using UnityEngine;

[Serializable]
public class DayData
{
    public int dayNumber;
    public int maxOrders;
    public float minSatisfactionRate;

    public DayData(int day, int maxOrders, float minSatisfaction)
    {
        this.dayNumber = day;
        this.maxOrders = maxOrders;
        this.minSatisfactionRate = minSatisfaction;
    }
}
