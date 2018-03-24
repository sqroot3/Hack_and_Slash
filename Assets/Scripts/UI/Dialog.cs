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
    public float charTime = 1.0f;
    public float expiringTime = 5.0f;
    private float[] positionLeft = { -204f, 4.5f };
    private float[] positionRight = { 214f, 4.5f };
    private int charLimit = 59;
    


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
            UICanvas.enabled = false;
            StopAllCoroutines();
        }

        //long message test
        if(Input.GetKeyDown(KeyCode.K))
        {
            string longText = "Bacon ipsum dolor amet jowl biltong ham hock turducken trifecta. Flank beef rump, pork loin landjaeger tenderloin biltong turkey short loin pork chop meatball. Doner meatball pig short ribs pancetta. Leberkas fatback pork belly, frankfurter alcatra beef ribs shank meatloaf hamburger. Tenderloin venison shank brisket short ribs frankfurter pork chop hamburger. Tenderloin strip steak bresaola, meatball salami sirloin boudin short ribs meatloaf. Swine prosciutto pork loin short ribs shankle.";
            StartCoroutine(WriteMessage(charTime, expiringTime, longText, false));
        }
        
	}

    
    public IEnumerator WriteMessage(float charTime, float expiringTime, string text, bool isLeft)
    {
        RectTransform parent = avatar.GetComponentInParent<RectTransform>();
        parent.anchoredPosition3D = (isLeft) ? new Vector3(positionLeft[0], positionLeft[1]) : new Vector3(positionRight[0], positionRight[1]);
        message.text = "";
        UICanvas.enabled = true;
        avatar.sprite = character;

        //take into account longer lines - may need to subdivide
        //when we've printed 3 lines already, message needs to be adjusted to include the previous 2 as the start, then print the current, and so on
        List<String> lines = SplitString(text);
        
        //for each line, print each character
        for(int line = 0; line < lines.Count; ++line)
        {
            if(line >= 2)
            {
                message.text = lines[line - 2] + lines[line - 1];
            }
            for(int i = 0; i < lines[line].Length; ++i)
            {
                message.text += lines[line][i];
                yield return new WaitForSeconds(charTime);
            }
        }

        StartCoroutine(DeleteMessage(expiringTime));
    }

    public IEnumerator DeleteMessage(float expiringTime)
    {
        yield return new WaitForSeconds(expiringTime);
        message.text = "";
        UICanvas.enabled = false;
    }

    //Returns a list of substrings generated from the initial string
    public List<String> SplitString(string text)
    {
        List<String> dst = new List<String>();
        dst.Add("");

        int lineNumber = 0;
        for (int i = 0; i < text.Length; ++i)
        {
            //if at char limit, go to new line
            if (i % charLimit == 0 && i != 0)
            {
                dst.Add("");
                //if word isn't completed, add on a hyhon
                if (Char.IsWhiteSpace(text[i + 1]) || Char.IsWhiteSpace(text[i]) || Char.IsWhiteSpace(text[i - 1]))
                    dst[lineNumber++] += "\n";
                else
                    dst[lineNumber++] += "-\n";
            }

            dst[lineNumber] += text[i];
        }
        return dst;
    }

}
