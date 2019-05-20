using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeTest : InteractableNPC
{
    public override ActionStates Interaction { get => ActionStates.TALK; set { } }
    public override string Name { get => "Spherio"; set { } }

    public GameObject UIElement;

    public override void Action()
    {
        if (UIElement.GetComponent<Shaker>() == null)
        {
            UIElement.AddComponent<Shaker>();
        }
        UIElement.GetComponent<Shaker>().setShake(14, Random.Range(0.5f, 2), Random.Range(1, 16), (Shaker.ShakeStyle)Mathf.FloorToInt(Random.Range(0,3)), true);
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
