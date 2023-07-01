using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health
{
    public int Amount { get; private set; }
    
    public void SetAmount(int value)
    {
        Amount = value;
    }
    public void TakeDamage(int value)
    {
        Amount -= value;
    }

    public void Recover(int value)
    {
        Amount += value;
    }
}
