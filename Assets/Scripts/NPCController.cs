using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{

    Animator anm;

    public string startingState;

    // Start is called before the first frame update
    void Start()
    {
        anm = GetComponent<Animator>();
        anm.Play(startingState);
        anm.SetInteger("Animation", 99);
    }

    private void Update()
    {    
    }

    public void setAnimation(int state)
    {
        anm.SetInteger("Animation", state);
    }

}
