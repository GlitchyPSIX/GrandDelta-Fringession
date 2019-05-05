using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    public string title;

    [TextArea(3, 3)]
    public string[] captions;
}
