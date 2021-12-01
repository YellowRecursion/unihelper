using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question
{
    public enum Type
    {
        Expression
    }

    public Type type;

    public string text = "Найдите производную функции";

    public string question = "question";
    public Texture2D questionImage;

    public List <Texture2D> answersImage = new List<Texture2D>();
    public List <string> answers = new List<string>();

    public float time = 5;
}
