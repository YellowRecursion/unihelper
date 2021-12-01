using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DataBase", menuName ="CreateDataBase")]
public class DataBase : ScriptableObject
{
    public string sectionName = "�����������";

    public List<Level> levels = new List<Level>();

    
}
