using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Text;

public class DialogTypewriter : Typewriter
{

    public enum TypewriterStatus
    {
        IDLE, WRITING, INACTIVE
    }

    TypewriterStatus _status;
    public TypewriterStatus Status { get { return _status; } set { _status = value; } }

    public Button continueButton;
    // Start is called before the first frame update
    void Start()
    {
        visibleText = GetComponent<Text>();
        fulltext = visibleText.text;
        visibleText.text = currenttext;
    }



    public void blankTypewriter()
    {
        continueButton.interactable = false;
        fulltext = visibleText.text;
        visibleText.text = currenttext;
    }

    override public void startTypewriter()
    {
        blankTypewriter();
        _status = TypewriterStatus.WRITING;
        StartCoroutine(TypeWrite());
    }

    override public IEnumerator TypeWrite()
    {
        yield return StartCoroutine(base.TypeWrite());
        continueButton.interactable = true;
        _status = TypewriterStatus.IDLE;
        yield return null;
    }
}
