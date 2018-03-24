using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Dialog : MonoBehaviour {
    [SerializeField] private Text message;
    [SerializeField] private Canvas UICanvas;
    [SerializeField] private Sprite character;
    [SerializeField] private Image avatar;
    [SerializeField] private KeyCode skipKey;
    [SerializeField] private int dialogID;
    public float charTime = 1.0f;
    public float expiringTime = 5.0f;
    private float[] positionLeft = { -204f, 4.5f };
    private float[] positionRight = { 214f, 4.5f };
    private int charLimit = 59;
    private int maxLines = 3;
    //private int charLimit = 10;

    public static int activeDialog = -1;


    void Update () {
		//Debug-test code

        //toggle canvas test
        if(Input.GetKeyDown(KeyCode.Space))
        {
            UICanvas.enabled = !UICanvas.enabled;
        }

        //write message test
        if(Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(WriteMessage(charTime, expiringTime, "Hello there", true));
        }

        //skip message test
        if(Input.GetKeyDown(skipKey))
        {
            Stop(true);
            //UICanvas.enabled = false;
            //StopAllCoroutines();
        }

        //long message test
        if(Input.GetKeyDown(KeyCode.K))
        {
            string longText = "Bacon ipsum dolor amet bacon pork loin beef shank sirloin. Filet mignon doner kevin fatback jowl. Buffalo tail biltong ground round kielbasa leberkas, pork chop salami boudin tri-tip frankfurter capicola beef ribs jerky. Biltong turkey frankfurter, landjaeger pork loin kielbasa tri-tip pork cow kevin pancetta flank buffalo. Cow doner ground round shank picanha buffalo beef frankfurter flank. Boudin biltong sirloin jerky, pancetta ball tip cow alcatra ground round spare ribs filet mignon. Strip steak burgdoggen shank prosciutto.";
            StartCoroutine(WriteMessage(charTime, expiringTime, longText, false));
        }
        
	}

    
    public IEnumerator WriteMessage(float charTime, float expiringTime, string text, bool isLeft)
    {
        if(activeDialog == -1)
        {
            activeDialog = dialogID;
            avatar.enabled = true;
            RectTransform parent = avatar.GetComponentInParent<RectTransform>();
            parent.anchoredPosition3D = (isLeft) ? new Vector3(positionLeft[0], positionLeft[1]) : new Vector3(positionRight[0], positionRight[1]);
            message.text = "";
            UICanvas.enabled = true;
            avatar.sprite = character;

            //take into account longer lines - may need to subdivide
            //when we've printed 3 lines already, message needs to be adjusted to include the previous 2 as the start, then print the current, and so on
            List<String> lines = SplitString(text);

            //for each line, print each character
            for (int line = 0; line < lines.Count; ++line)
            {
                if (line >= maxLines - 1)
                {
                    message.text = lines[line - (maxLines - 1)] + lines[line - 1];
                }
                for (int i = 0; i < lines[line].Length; ++i)
                {
                    message.text += lines[line][i];
                    yield return new WaitForSeconds(charTime);
                }
            }

            StartCoroutine(DeleteMessage(expiringTime));
        }
    }

    public IEnumerator DeleteMessage(float expiringTime)
    {
        yield return new WaitForSeconds(expiringTime);
        message.text = "";
        activeDialog = -1;
        //UICanvas.enabled = false;

    }

    public List<String> SplitString(string text)
    {
        //do a first run, identify where the spaces and cutoff points are located on the string
        List<int> spaces = new List<int>();
        List<int> cutoffs = new List<int>();
        for (int i = 0; i < text.Length; ++i)
        {
            if(text[i] == ' ')
                spaces.Add(i);
            if (i % charLimit == 0 && i != 0)
                cutoffs.Add(i);
        }


        //locate where each line's break should be - calculated to be the last whitespace before cutoff point
        List<int> breaks = new List<int>();
        int line = 0;
        for (int i = 0; i < spaces.Count; ++i)
        {
            //if we've already covered all possible lines, break out of the loop
            if (line >= cutoffs.Count)
                break;

            //if we're at the last space, there's still more text to go & there's exactly one more line to take care off, add the break
            if (i == spaces.Count - 1 && spaces[i] < text.Length && line == cutoffs.Count - 1)
            {

                breaks.Add(spaces[i]);
                line++;
            }
            //else, if we're at any space other than the last:
            //If the current space is before the cutoff point, the next space is past the cutoff point
            // OR the current space itself is at a cutoff point, add the break
            else if (spaces[i] < cutoffs[line] && spaces[i + 1] > cutoffs[line] || spaces[i] == cutoffs[line])
            {
                breaks.Add(spaces[i]);
                line++;
            }
        }

        //create our list of substrings, populate it with the lines generated from the breaks above
        List<String> dst = new List<String>();
        dst.Add("");

        int lineNumber = 0;
        for (int i = 0; i < text.Length; ++i)
        {
            //if we still have lines to account for AND we're currently in the position of a break: add the newline to the old line, create new empty line
            if (lineNumber < cutoffs.Count && i == breaks[lineNumber])
            {
                dst.Add("");
                dst[lineNumber++] += " \n";
            }
            //else, just print the character that was there
            else
                dst[lineNumber] += text[i];
        }

        return dst;
    }

    public int getID()
    {
        return dialogID;
    }

    public void Stop(bool hide)
    {
        message.text = "";
        activeDialog = -1;
        StopAllCoroutines();
        if (hide)
        {
            avatar.enabled = false;
            UICanvas.enabled = false;
        }
    }
}
