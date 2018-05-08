using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        if(currentMessage == conversation.messages.Count)
        {
            StartCoroutine(StartGame(3f));
        }
    }

    void SetMessage(int messageNumber)
    {
        text.text = conversation.messages[messageNumber].message;
        image.sprite = conversation.speak.speakers[conversation.messages[messageNumber].id].avatar;
    }

    IEnumerator StartGame(float wait)
    {
        yield return new WaitForSeconds(wait);
        //change manager to final game scene's name
        SceneManager.LoadScene("Manager", LoadSceneMode.Single);
    }
}
