using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    public int healAmount;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<PlayerBase>() != null)
        {
            collision.gameObject.GetComponent<PlayerBase>().Heal(healAmount);
        }
    }
}
