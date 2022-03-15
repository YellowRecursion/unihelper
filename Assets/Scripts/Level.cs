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

    private LevelInfo _levelInfo;
    private bool _loaded = false;

    public int Stars
    {
        get
        {
            Load();
            return _levelInfo.stars;
        }
        set
        {
            Load();
            _levelInfo.stars = value;
            Save();
        }
    }

    public int CorrectAnswersCount
    {
        get
        {
            Load();
            return _levelInfo.correctAnswersCount;
        }
        set
        {
            Load();
            _levelInfo.correctAnswersCount = value;
            Save();
        }
    }

    public int[] MistakeIndexes
    {
        get
        {
            Load();
            return _levelInfo.mistakeIndexes;
        }
        set
        {
            Load();
            _levelInfo.mistakeIndexes = value;
            Save();
        }
    }



    private void Load()
    {
        if (_loaded) return;
        _loaded = true;

        if (DataSaver.OpenToRead(parentBase.name + "_" + levelIndex))
        {
            _levelInfo = JsonUtility.FromJson<LevelInfo>(DataSaver.ReadLine());
            DataSaver.Close();
        }
        else
        {
            _levelInfo = new LevelInfo();
            Save();
        }
    }
    private void Save()
    {
        DataSaver.OpenToWrite(parentBase.name + "_" + levelIndex);
        DataSaver.WriteLine(JsonUtility.ToJson(_levelInfo));
        DataSaver.Close();
    }
}

[System.Serializable]
public class LevelInfo
{
    public LevelInfo()
    {
        mistakeIndexes = new int[0];
    }

    public int stars;
    public int correctAnswersCount;
    public int[] mistakeIndexes;
}
