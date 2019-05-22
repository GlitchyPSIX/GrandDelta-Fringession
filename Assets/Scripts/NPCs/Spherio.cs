using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spherio : InteractableNPC
{
    public override ActionStates Interaction { get => ActionStates.TALK; set { } }
    public override string Name { get => "Spherio"; set { } }

    public Dialog dialog;

    public override void Action()
    {
        FindObjectOfType<DialogManager>().StartDialog(dialog, this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
