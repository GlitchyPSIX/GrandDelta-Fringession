using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    void Heal();

    void Damage();

    void Kill();

    void Resucitate();
}
