using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGravitable
{
    void UpdatePhysics();

    void Jump(float amount);

    bool CheckGround();
}
