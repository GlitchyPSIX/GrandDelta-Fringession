using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableNPC : MonoBehaviour
{
    public enum ActionStates
    {
        NOTHING,
        TALK,
        INTERACT
    }
    public abstract ActionStates Interaction { get; set; }
    public abstract string Name { get; set; }

    public abstract void Action();
}
