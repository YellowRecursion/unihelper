using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Impressive
    }

    public DataBase parentBase;
    public int levelIndex = 0;
    public List<Question> questions = new List<Question>();
    public Difficulty difficulty;

    private int stars;
    private bool loaded = false;

    public int GetStars()
    {
        if (!loaded) Load();
        return stars;
    }
    public void SetStars(int stars)
    {
        this.stars = stars;
        Save();
    }

    private void Load()
    {
        if (DataSaver.OpenToRead(parentBase.name + "_" + levelIndex))
        {
            stars = DataSaver.ReadLineInt();
            DataSaver.Close();
        }
        else
        {
            Save();
        }
    }
    private void Save()
    {
        DataSaver.OpenToWrite(parentBase.name + "_" + levelIndex);
        DataSaver.WriteLine(stars);
        DataSaver.Close();
    }
}
