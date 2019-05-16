using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableNPC : MonoBehaviour
{

    public void Start()
    {
    }

    public enum ActionStates
    {
        NOTHING,
        TALK,
        INTERACT
    }

    public ActionStates interaction;
    public string _name;

    public ActionStates ReturnActionState()
    {
        return interaction;
    }

    public virtual void Interaction()
    {
    
    }
}
