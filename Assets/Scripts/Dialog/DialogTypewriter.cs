using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Text;

public class DialogTypewriter : Typewriter
{

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
        StartCoroutine(TypeWrite());
    }

    override public IEnumerator TypeWrite()
    {
        yield return StartCoroutine(base.TypeWrite());
        continueButton.interactable = true;
        yield return null;
    }
}
