using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game Data")]
public class GameData : ScriptableObject
{
    public Color[] difficultyColors = new Color[4];
    [SerializeField] Subject[] subjects;

    private Subject _selectedSubject;
    private DataBase _selectedSection;
    private Level _selectedLevel;

    public Subject[] Subjects { get => subjects; }
    public Subject SelectedSubject
    {
        get 
        {
            if (_selectedSubject == null) _selectedSubject = Subjects[0];
            return _selectedSubject; 
        }
        set
        {
            _selectedSubject = value;
        }
    }
    public DataBase SelectedSection
    {
        get 
        {
            if (!_selectedSection) _selectedSection = Subjects[0].sections[0];
            return _selectedSection; 
        }
        set
        {
            _selectedSection = value;
        }
    }
    public Level SelectedLevel
    {
        get 
        { 
            return _selectedLevel; 
        }
        set
        {
            _selectedLevel = value;
        }
    }
}