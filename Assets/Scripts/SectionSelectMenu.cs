using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SectionSelectMenu : MonoBehaviour
{
    public GameData data;
    public SectionSelectItem sectionSelectItemOriginal;
    public Transform content;
    public InterfaceMenuOpenCloseAnimator levelMenuOpener;
    public TextMeshProUGUI title;

    private List<GameObject> elements;

    public void Refresh()
    {
        title.text = data.SelectedSubject.name.ToUpper();

        if (elements != null)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                Destroy(elements[i]);
            }
        }

        elements = new List<GameObject>();

        for (int i = 0; i < data.SelectedSubject.sections.Length; i++)
        {
            var item = Instantiate(sectionSelectItemOriginal, content);
            item.Setup(data.SelectedSubject.sections[i]);
            elements.Add(item.gameObject);
        }
    }

    public void OnClick(DataBase dataBase)
    {
        data.SelectedSection = dataBase;
        levelMenuOpener.Open();
        FindObjectOfType<LevelsMenu>().RefreshList();
    }
}
