using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelsMenu : MonoBehaviour
{
    [Header("Данные")]
    public GameData gameData;
    public DataBase dataBase;

    [Header("Объекты")]
    public GameObject levelPrefab;
    public RectTransform content;
    public LevelLoader levelLoader;
    public TextMeshProUGUI title;

    private GameObject[] elements;

    private void Start()
    {
        RefreshList();
    }

    private void ClearList()
    {
        if (elements == null) return;
        for (int i = 0; i < elements.Length; i++)
        {
            Destroy(elements[i]);
        }
    }

    private void CreateList()
    {
        for (int i = 0; i < dataBase.levels.Count; i++)
        {
            elements[i] = Instantiate(levelPrefab, content);
      
            bool locked = false;
            if (i > 0)
            {
                if (dataBase.levels[i - 1].GetStars() == 0) locked = true;
            }
            
            elements[i].GetComponent<LevelButtonSetuper>().Setup(this, dataBase.levels[i], i + 1, locked);
        }
    }

    public void RefreshList()
    {
        dataBase = gameData.SelectedSection;

        title.text = dataBase.sectionName.ToUpper();

        ClearList();
        elements = new GameObject[dataBase.levels.Count];
        CreateList();
    }
}
