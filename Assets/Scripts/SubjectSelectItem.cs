using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubjectSelectItem : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    private Subject _subject;

    public void Setup(Subject subject)
    {
        _subject = subject;
        nameText.text = subject.name.ToUpper();
    }

    public void OnClick()
    {
        FindObjectOfType<SubjectSelectMenu>().OnClick(_subject);
    }
}
