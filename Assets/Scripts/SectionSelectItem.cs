using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SectionSelectItem : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    private DataBase _dataBase;

    public void Setup(DataBase dataBase)
    {
        _dataBase = dataBase;
        nameText.text = dataBase.sectionName.ToUpper();
    }

    public void OnClick()
    {
        FindObjectOfType<SectionSelectMenu>().OnClick(_dataBase);
    }
}
