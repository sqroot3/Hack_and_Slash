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
            if(currentMessage + 1 <= conversation.messages.Count)
            SetMessage(currentMessage++);
        }
    }

    void SetMessage(int messageNumber)
    {
        text.text = conversation.messages[messageNumber].message;
        image.sprite = conversation.speak.speakers[conversation.messages[messageNumber].id].avatar;
    }
}
