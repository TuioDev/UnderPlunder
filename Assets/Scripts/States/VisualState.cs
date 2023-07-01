using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class VisualState : MonoBehaviour
{
    protected Player PlayerRef;

    public VisualState(Player player)
    {
        PlayerRef = player;
    }
    protected abstract void Enter(Player player);
    protected abstract void Execute();
    protected abstract void Exit(Player player);
}
