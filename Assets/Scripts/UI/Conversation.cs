using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//will live as asset in project

[CreateAssetMenu(fileName = "Conversation", menuName = "Dialog")]
public class Conversation : ScriptableObject {

    [Serializable]
	public class ConversationMessage
    {
        public int speaker;
        [Multiline]
        public string message;
        public Sprite avatar;
    }

    public List<ConversationMessage> messages;
}
