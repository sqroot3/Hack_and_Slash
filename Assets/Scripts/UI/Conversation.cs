using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//will live as asset in project

[CreateAssetMenu(fileName = "Conversation", menuName = "Dialog")]
public class Conversation : ScriptableObject {

    public Speaker speak;

    [Serializable]
	public class ConversationMessage
    {
        public int id;
        [Multiline]
        public string message;
    }

    public List<ConversationMessage> messages;
}
