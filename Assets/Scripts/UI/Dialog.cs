using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text), typeof(Canvas))]
public class Dialog : MonoBehaviour {
    [SerializeField] private Text message;
    [SerializeField] private Canvas UICanvas;
    [SerializeField] private Sprite character;
    [SerializeField] private Image avatar;
    public float charTime = 1.0f;
    public float expiringTime = 5.0f;
    private float[] positionLeft = { -204f, 4.5f };
    private float[] positionRight = { 214f, 4.5f };


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
        
	}

    public IEnumerator WriteMessage(float charTime, float expiringTime, string text, bool isLeft)
    {
        RectTransform parent = avatar.GetComponentInParent<RectTransform>();
        parent.anchoredPosition3D = (isLeft) ? new Vector3(positionLeft[0], positionLeft[1]) : new Vector3(positionRight[0], positionRight[1]);
        message.text = "";
        UICanvas.enabled = true;
        avatar.sprite = character;
        for (int i = 0; i < text.Length; ++i)
        {
            message.text += text[i];
            //wait for char time
            yield return new WaitForSeconds(charTime);
        }
        StartCoroutine(DeleteMessage(expiringTime));
    }

    public IEnumerator DeleteMessage(float expiringTime)
    {
        yield return new WaitForSeconds(expiringTime);
        message.text = "";
        UICanvas.enabled = false;
    }
}
