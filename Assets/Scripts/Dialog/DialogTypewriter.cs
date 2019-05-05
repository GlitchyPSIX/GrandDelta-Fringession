using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogTypewriter : MonoBehaviour
{

    public Button continueButton;

    public float speed = 0.005f;
    internal string fulltext;
    internal string currenttext;

    internal Text visibleText;

    // Start is called before the first frame update
    void Start()
    {
        visibleText = GetComponent<Text>();
        fulltext = visibleText.text;
        visibleText.text = currenttext;
    }

    virtual public void startTypewriter()
    {
        continueButton.interactable = false;
        fulltext = visibleText.text;
        visibleText.text = currenttext;
        StartCoroutine(TypeWrite());
    }

    virtual public IEnumerator TypeWrite()
    {
        for (int i = 0; i < fulltext.Length + 1; i++)
        {
            currenttext = fulltext.Substring(0, i) + "<color=\"#0000\"> " + fulltext.Substring(i) + "</color>";
            visibleText.text = currenttext;
            yield return new WaitForSeconds(speed);
        }
        continueButton.interactable = true;
    }
}
