using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    public enum ItemType
    {
        health, points, lives
    }

    [SerializeField] ItemType itemType;
    [SerializeField] int healthValue;
    [SerializeField] int pointValue;
    [SerializeField] int livesValue;

    public ItemType getConsumableType()
    {
        return this.itemType;
    }
    public int getConsumableValue()
    {
        int consumableValue = 0;
        switch(itemType)
        {
            case ItemType.health:
                consumableValue = this.healthValue;
                break;
            case ItemType.points:
                consumableValue = this.pointValue;
                break;
            case ItemType.lives:
                consumableValue = this.livesValue;
                break;
        }
        return consumableValue;
    }
}
