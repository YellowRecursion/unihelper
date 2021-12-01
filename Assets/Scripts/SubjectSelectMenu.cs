using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectSelectMenu : MonoBehaviour
{
    public GameData data;
    public SubjectSelectItem subjectSelectItemOriginal;
    public Transform content;
    public InterfaceMenuOpenCloseAnimator sectionMenuOpener;

    private void Start()
    {
        for (int i = 0; i < data.Subjects.Length; i++)
        {
            var item = Instantiate(subjectSelectItemOriginal, content);
            item.Setup(data.Subjects[i]);
        }
    }

    public void OnClick(Subject subject)
    {
        data.SelectedSubject = subject;
        sectionMenuOpener.Open();
        FindObjectOfType<SectionSelectMenu>().Refresh();
    }
}
