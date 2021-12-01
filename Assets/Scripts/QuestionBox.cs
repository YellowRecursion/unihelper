using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionBox : MonoBehaviour
{
    [Header("Main")]
    public GameObject[] typeObjects;

    [Header("Expression Type")]
    public GameObject expressionType;
    public TextMeshProUGUI expressionTitle;
    public ExpressionDrawer expressionImage;

    public void Setup(Question question)
    {
        for (int i = 0; i < typeObjects.Length; i++)
            typeObjects[i].SetActive(false);

        switch (question.type)
        {
            case Question.Type.Expression:
                expressionType.SetActive(true);
                expressionTitle.text = question.text;
                expressionImage.TryGetFormulaImageFromFile(question.question);
                break;
        }
    }
}
