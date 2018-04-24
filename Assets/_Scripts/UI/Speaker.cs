using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Speaker", menuName = "Speaker")]
public class Speaker : ScriptableObject {

    [Serializable]
    public class SpeakerData
    {
        public int id;
        public string name;
        public Sprite avatar;
    }

    public List<SpeakerData> speakers;
}
