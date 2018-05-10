using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {
    public int magicKills;
    public int swordKills;
    public string sTime;
    public float fTime;
    public string name;
    public string difficulty;
	
    public SaveData() {
        magicKills = swordKills = 0;
        sTime = "N/A";
        name = "N/A";
        difficulty = "N/A";
    }

    public SaveData(int m, int s, string _sTime, float _fTime, string _name, string _difficulty)
    {
        magicKills = m;
        swordKills = s;
        sTime = _sTime;
        fTime = _fTime;
        name = _name;
        difficulty = _difficulty;
    }
}
