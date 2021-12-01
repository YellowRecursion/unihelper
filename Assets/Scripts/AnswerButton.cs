using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    [Header("Main")]
    public GameObject[] typeObjects;
    public AnswerBackgroundAnimator backgroundAnimator;

    [Header("Expression Type")]
    public GameObject expressionType;
    public ExpressionDrawer expressionImage;

    private int _answerIndex;

    public int AnswerIndex { get => _answerIndex; private set => _answerIndex = value; }

    public void Setup(Question question, int ansIndex)
    {
        AnswerIndex = ansIndex;

        for (int i = 0; i < typeObjects.Length; i++)
            typeObjects[i].SetActive(false);

        switch (question.type)
        {
            case Question.Type.Expression:
                expressionType.SetActive(true);
                expressionImage.TryGetFormulaImageFromFile(question.answers[ansIndex]);
                break;
        }
    }

    public void GiveAnswer()
    {
        FindObjectOfType<GameManager>().GiveAnswer(AnswerIndex);
    }
}
