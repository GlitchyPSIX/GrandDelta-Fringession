using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Text;
using System.Text.RegularExpressions;

public class Typewriter : MonoBehaviour
{

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
        fulltext = visibleText.text;
        visibleText.text = currenttext;
        StartCoroutine(TypeWrite());
    }

    virtual public IEnumerator TypeWrite()
    {
        //TODO: Make it so it supports multiple coloring and formatting.
        //Perhaps use a Queue<string>
        bool skip = false;
        bool closingTag = false;
        StringBuilder enqueuedRich = new StringBuilder();
        char[] charArray = fulltext.ToCharArray();
        for (int i = 0; i < fulltext.Length + 1; i++)
        {
            if (i + 1 < fulltext.Length + 1)
            {
                if (charArray[i] == '<')
                {
                    enqueuedRich.Clear();
                    if (fulltext.ToCharArray()[i + 1] == '/')
                    {
                        closingTag = true;
                    }
                    skip = true;
                    continue;
                }
                else if (skip && fulltext.ToCharArray()[i] == '>')
                {
                    skip = false;
                    closingTag = false;
                    continue;
                }
            }
            if (!skip)
            {
                string _cTag = ((enqueuedRich.Length != 0) ? "</" + enqueuedRich.ToString() + ">" : "");
                Regex repl = null;
                if (_cTag != "")
                {
                    repl = new Regex(_cTag);
                }
                Debug.Log(fulltext.Substring(0, i) + _cTag + "<color=\"#0000\"> " + (_cTag == "" ? fulltext.Substring(i) : repl.Replace(fulltext.Substring(i), "", 1)) + "</color>");
                currenttext = fulltext.Substring(0, i) + _cTag + "<color=\"#0000\"> " + (_cTag == "" ? fulltext.Substring(i) : repl.Replace(fulltext.Substring(i), "", 1)) + "</color>";
        visibleText.text = currenttext;
                yield return new WaitForSeconds(speed);
            }
            else
            {
                if (closingTag)
                {
                    continue;
                }
                else
                {
                    enqueuedRich.Append(charArray[i]);
                    continue;
                }
            }

        }
    }
}
