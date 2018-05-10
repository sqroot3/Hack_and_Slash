using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class SaveManager : MonoBehaviour {

    [HideInInspector] public SaveData save;
    protected string savePath;


    private void Awake()
    {
        this.savePath = Application.persistentDataPath + "/save.dat";
        this.save = new SaveData();
        this.loadDataFromDisk();
    }

    public void saveDataToDisk()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        file = (File.Exists(savePath)) ? File.Open(savePath, FileMode.Open) : File.Create(savePath);
        bf.Serialize(file, save);
        file.Close();
    }

    public void loadDataFromDisk()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.Open);
            this.save = (SaveData)bf.Deserialize(file);
            file.Close();
        }
    }

    public bool isHighScore(int sKills, int mKills, float time)
    {
        //if kills are the same, define by timer
        if(mKills + sKills == save.magicKills + save.swordKills)
        {
            return (time < save.fTime);
        }
        //else, is a high score only if more kills
        return (mKills + sKills > save.magicKills + save.swordKills);
    }

    public void setSaveData(SaveData data)
    {
        save = data;
    }
}
