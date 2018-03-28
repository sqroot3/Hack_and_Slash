using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationPlayer : MonoBehaviour {

    public Conversation conversation;

    public Text text;
    public Image image;


    private int currentMessage = 0;

	// Use this for initialization
	void Start () {
        currentMessage = 0;
        SetMessage(currentMessage);
	}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SetMessage(currentMessage++);
        }
    }

    void SetMessage(int messageNumber)
    {
        text.text = conversation.messages[messageNumber].message;
        setSpeakerPic(conversation.messages[messageNumber].speaker);
    }
	
	void setSpeakerPic(int speaker)
    {
        switch(speaker)
        {
            case 0:
                image.material.color = Color.red;
                break;
            case 1:
                image.material.color = Color.blue;
                break;
            default:
                image.material.color = Color.white;
                break;
        }
    }
}
